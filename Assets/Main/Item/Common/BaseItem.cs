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
using MLAPIPlayerSpace;

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
    [SerializeField] protected GrabObservable m_GrabTarget = default;

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
            var offset = grabber.UseHandOffset ? m_HandShapeHandler.HandOffset : default;
            transform.localPosition = offset.Position;
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

// abstract public class BaseItem : PoolablePunBehaviour, IGrabbable, IInteractor
// {
//     HandShapeHandler m_HandShapeHandler;
//     public HandShapeHandler HandShapeHandler => m_HandShapeHandler;
//     public IObservable<HandShape> HandShapeAsObservable => m_HandShapeHandler.HandShapeAsObservable;
//     [SerializeField] bool m_IsReversible;
//     [SerializeField] Collider[] m_Colliders = default;
//     protected Rigidbody m_Rigidbody;
//     bool m_DefaultUseGravity;
//     bool m_Defaultkinematic;
//     ReactiveProperty<IGrabber> m_ParentRP = new ReactiveProperty<IGrabber>();
//     public IGrabber Parent { set { m_ParentRP.Value = value; } get { return m_ParentRP.Value; } }
//     IDisposable m_ReleaseSubscription;
//     public bool IsGrabbed => Parent != null && Parent.HasTarget(this);
//     [SerializeField] protected GrabObservable m_GrabTarget = default;

//     override protected void Awake()
//     {
//         m_Rigidbody = GetComponent<Rigidbody>();
//         m_DefaultUseGravity = m_Rigidbody.useGravity;
//         m_Defaultkinematic = m_Rigidbody.isKinematic;
//         m_HandShapeHandler = GetComponent<HandShapeHandler>();
//         base.Awake();
//     }

//     override public void Init()
//     {
//         base.Init();
//         m_Rigidbody.useGravity = m_DefaultUseGravity;
//         m_Rigidbody.isKinematic = m_Defaultkinematic;
//         Subscriptions.AddRange(new[]{
//             m_ParentRP.Pairwise().Subscribe(parents =>
//             {
//                 var pre = parents.Previous;
//                 var cur = parents.Current;
//                 if (pre != null)
//                 {
//                     if (m_ReleaseSubscription != null)
//                         m_ReleaseSubscription.Dispose();
//                     if (pre.HasTarget(this))
//                         pre.SetTarget(null);
//                 }
//                 if (cur != null)
//                 {
//                     m_ReleaseSubscription = cur.ReleaseAsObservable.First().Subscribe(_ => RequestRelease(cur));
//                     if (pre == null){
//                         OnGrab(cur);
//                     cur.SetTarget(this);
//                     }
//                 }
//                 else
//                 {
//                     OnRelease(pre);
//                 }
//             }),
//             m_GrabTarget.GrabAsObservable.Subscribe(grabber => RequestGrabberChangeToMaster(grabber)),
//         });
//     }

//     override public void Destroy()
//     {
//         Parent = null;
//         m_Rigidbody.useGravity = false;
//         base.Destroy();
//     }

//     virtual public void OnGrab(IGrabber parent)
//     {
//         m_GrabTarget.IsTargettable = false;
//         m_Rigidbody.isKinematic = true;
//         m_Rigidbody.useGravity = false;
//         EnableCollision(false);
//         m_GrabTarget.IsTargettable = false;
//         if (parent.HandDominant != HandDominant.Left && !m_IsReversible)
//         {
//             var scale = transform.localScale;
//             transform.localScale = new Vector3(-scale.x, scale.y, scale.z);
//         }
//         transform.SetParent(parent.GrabAnchor, true);
//         SetOffsetPosition(parent);
//     }
//     virtual public void OnRelease(IGrabber parent)
//     {
//         m_GrabTarget.IsTargettable = true;
//         m_Rigidbody.isKinematic = m_Defaultkinematic;
//         m_Rigidbody.useGravity = m_DefaultUseGravity;
//         EnableCollision(true);
//         m_GrabTarget.IsTargettable = true;
//         transform.SetParent(null, true);
//     }
//     async virtual protected UniTask ThrowAsync()
//     {
//         await UniTask.SwitchToMainThread();
//         var prePos = transform.position;
//         var preRot = transform.rotation.eulerAngles;
//         await UniTask.Yield();
//         Parent = null;
//         m_Rigidbody.velocity = (transform.position - prePos) / Time.deltaTime;
//         m_Rigidbody.angularVelocity = (transform.rotation.eulerAngles - preRot) / Time.deltaTime;

//     }
//     public void SetOffsetPosition(IGrabber grabber)
//     {
//         if (m_HandShapeHandler)
//         {
//             var offset = grabber.UseHandOffset ? m_HandShapeHandler.HandOffset : default;
//             transform.localPosition = offset.Position;
//             if (!m_HandShapeHandler.UseWorldRotation)
//                 transform.localEulerAngles = offset.EulerAngles;
//         }
//     }
//     virtual protected void EnableCollision(bool enable)
//     {
//         if (m_Colliders != null)
//             foreach (var collider in m_Colliders)
//                 collider.enabled = enable;
//     }
//     public void RequestGrabberChangeToMaster(IGrabber grabber) => photonView.RPC(nameof(RequestGrabberChange), RpcTarget.MasterClient, grabber?.GrabberInfo.Serialize() ?? default);
//     public void RequestRelease(IGrabber parent)
//     {
//         if (Parent == parent)
//         {
//             UniTask.Run(async () =>
//             {
//                 await ThrowAsync();
//                 RequestGrabberChangeToMaster(null);
//             });
//         }
//     }
//     public void ForceRelease()
//     => RequestGrabberChangeToMaster(null);
//     [PunRPC]
//     protected void RequestGrabberChange(short serial, PhotonMessageInfo info)
//     {
//         if (serial == 0)
//             photonView.TransferOwnership(PhotonNetwork.MasterClient);
//         else
//             photonView.TransferOwnership(info.Sender);
//         photonView.RPC(nameof(SetGrabber), RpcTarget.AllViaServer, serial);
//     }
//     [PunRPC]
//     protected void SetGrabber(short serial)
//     => Parent = GrabberManager.Find(GrabberInfo.Deserialize(serial));


//     override public void OnPlayerEnteredRoom(Player newPlayer)
//     {
//         if (PhotonNetwork.IsMasterClient)
//             photonView.RPC(nameof(SetGrabber), newPlayer, Parent.GrabberInfo.Serialize());
//     }

//     private void OnApplicationQuit()
//     {
//         Destroy();
//     }
// }
