using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Spawning;
using UniRx;
using Cysharp.Threading.Tasks;
using Prefab;

[RequireComponent(typeof(Rigidbody))]
abstract public class BaseItem : NetworkPoolableParent, IGrabbable, IEventSource
{
    NetworkVariable<NetworkBehaviour> m_ParentBehaviourNV = new NetworkVariable<NetworkBehaviour>();
    NetworkBehaviour ParentBehaviour { set { m_ParentBehaviourNV.Value = value; } get { return m_ParentBehaviourNV.Value; } }
    HandShapeHandler m_HandShapeHandler;
    public HandShapeHandler HandShapeHandler => m_HandShapeHandler;
    public IObservable<HandShape> HandShapeAsObservable => m_HandShapeHandler.HandShapeAsObservable;
    [SerializeField] bool m_IsReversible;
    [SerializeField] Collider[] m_Colliders = default;
    protected Rigidbody m_Rigidbody;
    bool m_DefaultUseGravity;
    bool m_Defaultkinematic;
    IGrabber m_Parent;
    public IGrabber Parent => m_Parent;
    [SerializeField] protected GrabEventHandler m_GrabEventHandler = default;

    override protected void Awake()
    {
        base.Awake();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_DefaultUseGravity = m_Rigidbody.useGravity;
        m_Defaultkinematic = m_Rigidbody.isKinematic;
        m_HandShapeHandler = GetComponent<HandShapeHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ParentBehaviourNV.OnValueChanged += OnParentChanged;
        m_Rigidbody.useGravity = m_DefaultUseGravity;
        m_Rigidbody.isKinematic = m_Defaultkinematic;

        m_GrabEventHandler.SetEvent(info => TryGrabServerRpc(NetworkInfo.CreateFrom(info.Grabber.NetworkBehaviour)));
    }

    override public void OnPool()
    {
        if (IsOwner)
            ParentBehaviour = null;
        m_ParentBehaviourNV.OnValueChanged -= OnParentChanged;

        m_Rigidbody.useGravity = false;
        m_Rigidbody.velocity = Vector3.zero;
        m_Rigidbody.angularVelocity = Vector3.zero;
        base.OnPool();
    }

    void OnParentChanged(NetworkBehaviour previous, NetworkBehaviour current)
    {
        if (current != previous)
        {
            var parent = current?.GetComponent<IGrabber>();
            GrabAsync(parent).Forget();
        }
    }

    #region Grab

    CancellationTokenSource m_GrabCTS;
    public bool IsGrabbed => Parent != null && Parent.HasTarget(this);

    virtual public void OnGrab(IGrabber parent)
    {
        m_GrabEventHandler.IsActive = false;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
        EnableCollision(false);
        m_GrabEventHandler.IsActive = false;
        if (parent.HandDominant != HandDominant.Left && !m_IsReversible)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
    virtual public void OnRelease(IGrabber parent)
    {
        m_GrabEventHandler.IsActive = true;
        m_Rigidbody.isKinematic = m_Defaultkinematic;
        m_Rigidbody.useGravity = m_DefaultUseGravity;
        EnableCollision(true);
        m_GrabEventHandler.IsActive = true;
        parent.RemoveTarget(this);
        transform.SetParent(null, true);
    }

    public void SetOffsetPosition(IGrabber grabber)
    {
        if (m_HandShapeHandler)
        {
            var offset = m_HandShapeHandler.HandOffset;

            transform.localPosition = grabber.UseHandOffset ? offset.Position : default;
            if (!m_HandShapeHandler.UseWorldRotation)
                transform.localEulerAngles = offset.EulerAngles;
        }
    }
    virtual protected void EnableCollision(bool enable)
    {
        if (m_Colliders != null)
            foreach (var collider in m_Colliders)
                collider.enabled = enable;
    }



    [ServerRpc(RequireOwnership = false)]
    virtual public void TryGrabServerRpc(NetworkInfo networkInfo)
    {
        var grabber = networkInfo.ToComponent<IGrabber>();
        var success = RequestChangeParent(grabber);
        if (success)
            NetworkObject.ChangeOwnership(grabber.NetworkBehaviour.OwnerClientId);
    }
    [ServerRpc(RequireOwnership = false)]
    virtual public void ReleaseServerRpc(Vector3 velocity, Vector3 angularVelocity)
    {
        var success = RequestChangeParent(null);
        if (success)
        {
            NetworkObject.RemoveOwnership();
            m_Rigidbody.velocity = velocity;
            m_Rigidbody.angularVelocity = angularVelocity;
        }
    }

    public bool RequestChangeParent(IGrabber parent)
    {
        if (parent != null)
        {
            if (IsGrabbed) return false;

            ParentBehaviour = parent.NetworkBehaviour;
        }
        else
        {
            if (!IsGrabbed) return false;

            ParentBehaviour = null;
            transform.SetParent(null);
            m_Rigidbody.isKinematic = m_Defaultkinematic;
        }
        return true;
    }
    public void Release(IGrabber parent)
    {
        if (m_Parent == parent)
        {
            UniTask.Run(async () =>
            {
                var (velocity, angularVelocity) = await GetMoveInfoAsync();
                ReleaseServerRpc(velocity, angularVelocity);
                // SyncTransform();
            });
        }
    }
    public void ForceRelease()
    {
        var success = RequestChangeParent(null);
        if (success)
            NetworkObject.RemoveOwnership();
    }

    async protected UniTask GrabAsync(IGrabber parent)
    {
        await UniTask.SwitchToMainThread();
        m_Parent = parent;
        if (m_Parent != null)
        {
            if (m_GrabCTS == null)
                OnGrab(m_Parent);
            else
                m_GrabCTS.Cancel();
            m_Parent.SetTarget(this);
            transform.SetParent(m_Parent.GrabAnchor, true);
            SetOffsetPosition(m_Parent);
            m_GrabCTS = new CancellationTokenSource();
            try
            {
                await UniTask.WaitWhile(() => IsGrabbed, PlayerLoopTiming.Update, m_GrabCTS.Token);
            }
            catch (OperationCanceledException) { throw; }
            OnRelease(parent);
            m_GrabCTS = null;
        }
    }


    async virtual protected UniTask<(Vector3 velocity, Vector3 angularVelocity)> GetMoveInfoAsync()
    {
        await UniTask.SwitchToMainThread();
        var prePos = transform.position;
        var preRot = transform.rotation;
        await UniTask.Yield();
        (transform.rotation * Quaternion.Inverse(preRot)).ToAngleAxis(out var angle, out var axis);
        var velocity = (transform.position - prePos) / Time.deltaTime;
        var angularVelocity = axis * (angle * Mathf.Deg2Rad / Time.deltaTime);
        return (velocity, angularVelocity);
    }
    #endregion

    public static BaseItem FindFromNetworkId(ulong networkId)
    => NetworkSpawnManager.SpawnedObjects[networkId].GetComponentInChildren<BaseItem>();

    public void SyncTransform() => SetTransformServerRpc(transform.position, transform.rotation, NetworkManager.NetworkTime);
    void SetTransform(Vector3 position, Quaternion rotation) => transform.SetPositionAndRotation(position, rotation);
    [ServerRpc]
    void SetTransformServerRpc(Vector3 position, Quaternion rotation, float time)
    {
        var deltaTime = NetworkManager.NetworkTime - time;
        var deltaPos = m_Rigidbody.velocity * deltaTime;
        var angularVelocity = m_Rigidbody.angularVelocity;
        var deltaRot = Quaternion.AngleAxis(angularVelocity.magnitude * Mathf.Rad2Deg * deltaTime, angularVelocity);
        SetTransform(position, deltaRot * rotation);
    }

}