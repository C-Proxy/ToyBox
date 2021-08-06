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
abstract public class BaseItem : PoolableNetworkBehaviour, IGrabbable, IInteractor
{
    NetworkVariable<NetworkBehaviour> m_ParentBehaviourNV;
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
    [SerializeField] protected GrabEventHandler m_GrabTarget = default;

    override protected void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_DefaultUseGravity = m_Rigidbody.useGravity;
        m_Defaultkinematic = m_Rigidbody.isKinematic;
        m_HandShapeHandler = GetComponent<HandShapeHandler>();
        base.Awake();
    }

    override public void OnSpawn()
    {
        m_ParentBehaviourNV = new NetworkVariable<NetworkBehaviour>();
        m_ParentBehaviourNV.OnValueChanged += async (pre, cur) => await GrabAsync(cur?.GetComponent<IGrabber>());

        m_Rigidbody.useGravity = m_DefaultUseGravity;
        m_Rigidbody.isKinematic = m_Defaultkinematic;

        m_GrabTarget.SetGrabEvent(grabber => TryGrabServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour)));
    }

    override public void OnPool()
    {
        if (IsGrabbed)
            ForceReleaseServerRpc();
        m_ParentBehaviourNV = null;
        m_Rigidbody.useGravity = false;
        base.OnPool();
    }

    #region Grab

    CancellationTokenSource m_GrabCTS;
    public bool IsGrabbed => Parent != null && Parent.HasTarget(this);

    virtual public void OnGrab(IGrabber parent)
    {
        m_GrabTarget.IsTargettable = false;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;
        EnableCollision(false);
        m_GrabTarget.IsTargettable = false;
        if (parent.HandDominant != HandDominant.Left && !m_IsReversible)
        {
            var scale = transform.localScale;
            transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
        }
    }
    virtual public void OnRelease(IGrabber parent)
    {
        m_GrabTarget.IsTargettable = true;
        m_Rigidbody.isKinematic = m_Defaultkinematic;
        m_Rigidbody.useGravity = m_DefaultUseGravity;
        EnableCollision(true);
        m_GrabTarget.IsTargettable = true;
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
    => RequestChangeParent(networkInfo.ToComponent<IGrabber>());
    [ServerRpc(RequireOwnership = false)]
    virtual public void ReleaseServerRpc(Vector3 velocity, Vector3 angularVelocity)
    {
        RequestChangeParent(null);
        m_Rigidbody.velocity = velocity;
        m_Rigidbody.angularVelocity = angularVelocity;
    }

    public void RequestChangeParent(IGrabber parent)
    {
        if (parent != null)
        {
            if (IsGrabbed) return;

            ParentBehaviour = parent.NetworkBehaviour;
            NetworkObject.ChangeOwnership(parent.NetworkBehaviour.OwnerClientId);
        }
        else
        {
            if (!IsGrabbed) return;

            ParentBehaviour = null;
            NetworkObject.RemoveOwnership();
            transform.SetParent(null);
            m_Rigidbody.isKinematic = m_Defaultkinematic;
        }
    }
    public void Release(IGrabber parent)
    {
        if (m_Parent == parent)
        {
            UniTask.Run(async () =>
            {
                var (velocity, angularVelocity) = await GetMoveInfoAsync();
                ReleaseServerRpc(velocity, angularVelocity);
            });
        }
    }
    [ServerRpc(RequireOwnership = false)]
    public void ForceReleaseServerRpc()
    => RequestChangeParent(null);

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
            catch (OperationCanceledException)
            {
                throw;
            }
            OnRelease(parent);
            m_GrabCTS = null;
        }
    }


    async virtual protected UniTask<(Vector3 velocity, Vector3 angularVelocity)> GetMoveInfoAsync()
    {
        await UniTask.SwitchToMainThread();
        var prePos = transform.position;
        var preRot = transform.rotation.eulerAngles;
        await UniTask.Yield();
        return ((transform.position - prePos) / Time.deltaTime, (transform.rotation.eulerAngles - preRot) / Time.deltaTime);
    }
    #endregion

    public static BaseItem FindFromNetworkId(ulong networkId)
    => NetworkSpawnManager.SpawnedObjects[networkId].GetComponentInChildren<BaseItem>();
}