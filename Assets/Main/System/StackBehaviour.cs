using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using MLAPI.Serialization;
using System.Linq;
using UniRx;
using Prefab;
using MyClass.Rx;
using MyFunc;

public interface IStackable<TParent, TChild, TInfo>
where TParent : StackParentBehaviour<TParent, TChild, TInfo>
where TChild : StackChildBehaviour<TParent, TChild, TInfo>
where TInfo : INetworkSerializable
{
    void Set(TInfo serial, TParent parent);
    void OnSet(TInfo info);
}
public interface IStackRpcCaller
{
    // [ServerRpc(RequireOwnership = false)]
    // void AddChildrenServerRpc(INetworkSerializable[] infos);
    [ServerRpc(RequireOwnership = false)]
    void HandoverServerRpc(ulong receiverId, int index, int count);
    [ServerRpc(RequireOwnership = false)]
    void HandoverToGrabberServerRpc(NetworkInfo networkInfo, int index, int count);

    // [ServerRpc(RequireOwnership = false)]
    // protected void AddChildrenServerRpc(TInfo[] infos)
    // => AddChildInfos(infos);
    // [ServerRpc(RequireOwnership = false)]
    // protected void HandoverServerRpc(ulong receiverId, int index, int count)
    // => Handover(NetworkFunc.GetComponent<TParent>(receiverId), index, count);
    // [ServerRpc(RequireOwnership = false)]
    // protected void HandoverToGrabberServerRpc(NetworkInfo networkInfo, int index, int count)
    // {
    //     var receiver = PrefabGenerator.SpawnPrefabWithoutManager(PrefabHash).GetComponent<TParent>();
    //     receiver.RequestChangeParent(networkInfo.ToComponent<IGrabber>());
    //     Handover(receiver, index, count);
    // }

}
abstract public class StackParentBehaviour<TParent, TChild, TInfo> : BaseItem
where TParent : StackParentBehaviour<TParent, TChild, TInfo>
where TChild : StackChildBehaviour<TParent, TChild, TInfo>
where TInfo : INetworkSerializable
{
    IStackRpcCaller RpcCaller => (IStackRpcCaller)this;
    const int MAX_STACK = byte.MaxValue;
    abstract protected float CHILD_MASS { get; }
    abstract protected LocalPrefabName ChildPrefabName { get; }
    NetworkVariable<TInfo[]> m_ChildInfosNV;
    public TInfo[] ChildInfos { protected set { m_ChildInfosNV.Value = value; } get { return m_ChildInfosNV.Value; } }
    public int ChildLength => ChildInfos?.Length ?? 0;
    protected List<TChild> m_ChildList;

    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ChildInfosNV = new NetworkVariable<TInfo[]>(new TInfo[0]);
        m_ChildList = new List<TChild>();

        m_ChildInfosNV.OnValueChanged += (pre, cur) =>
        {
            var infoLength = cur.Length;
            Debug.Log(infoLength);
            if (infoLength == 0)
            {
                if (IsServer)
                    DespawnServerRpc();
                return;
            }
            m_Rigidbody.mass = CHILD_MASS * infoLength;
            var childCount = m_ChildList.Count;
            if (infoLength > childCount)
                m_ChildList.AddRange(GenerateChildren(infoLength - childCount));
            else if (infoLength < childCount)
            {
                for (int i = infoLength; i < childCount; i++)
                    m_ChildList[i].OnDespawn();
                m_ChildList.RemoveRange(infoLength, childCount - infoLength);
            }
            var stackParent = (TParent)this;
            foreach (var (child, info) in Enumerable.Zip(m_ChildList, cur, (child, info) => (child, info)))
                child.Set(info, stackParent);
            Align();
        };
    }

    override public void OnPool()
    {
        m_ChildInfosNV = null;
        foreach (var child in m_ChildList)
            child.OnDespawn();
        m_ChildList.Clear();
        m_ChildList = null;
        base.OnPool();
    }

    public void HandoverToGrabber(IGrabber grabber, TChild child)
    => RpcCaller.HandoverToGrabberServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), m_ChildList.IndexOf(child), 1);
    public void HandoverTopToGrabber(IGrabber grabber)
    => RpcCaller.HandoverToGrabberServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), m_ChildList.Count - 1, 1);
    public void HandoverBottomToGrabber(IGrabber grabber)
    => RpcCaller.HandoverToGrabberServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), 0, 1);
    public void HandoverTopChildrenToGrabber(IGrabber grabber, TChild child)
    {
        var index = m_ChildList.IndexOf(child);
        RpcCaller.HandoverToGrabberServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), m_ChildList.IndexOf(child), m_ChildList.Count - index);
    }
    public void HandoverBottomChildrenToGrabber(IGrabber grabber, TChild child)
    => RpcCaller.HandoverToGrabberServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), 0, m_ChildList.IndexOf(child) + 1);
    public void HandoverBottomChildren(TParent receiver, int index)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, 0, index + 1);
    public void HandoverTopChildrenRpc(TParent receiver, int index)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, index, m_ChildList.Count - index);

    public void HandoverChild(TParent receiver, TChild child)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, m_ChildList.IndexOf(child), 1);
    public void HandoverTopChild(TParent receiver)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, m_ChildList.Count - 1, 1);
    public void HandoverBottomChild(TParent receiver)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, 0, 1);
    public void HandoverTopChildren(TParent receiver, TChild child)
    {
        var index = m_ChildList.IndexOf(child);
        RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, index, m_ChildList.Count - index);
    }
    public void HandoverBottomChildren(TParent receiver, TChild child)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, 0, m_ChildList.IndexOf(child) + 1);
    public void HandoverAll(TParent receiver)
    => RpcCaller.HandoverServerRpc(receiver.NetworkObjectId, 0, m_ChildList.Count);


    public void AddChildInfos(TInfo[] infos)
    {
        var list = ChildInfos.ToList();
        list.AddRange(infos);
        ChildInfos = list.ToArray();
    }
    public TInfo[] RemoveChildInfos(int index, int count)
    {
        var list = ChildInfos.ToList();
        var removed = list.GetRange(index, count);
        list.RemoveRange(index, count);
        ChildInfos = list.ToArray();
        return removed.ToArray();
    }

    protected void Handover(TParent receiver, int index, int count)
    {
        var childCnt = receiver.ChildLength;
        if (childCnt + count > MAX_STACK)
            count = MAX_STACK - childCnt;
        if (count <= 0) return;
        var removed = RemoveChildInfos(index, count);
        receiver.AddChildInfos(removed);
    }

    abstract protected void Align();

    override protected void EnableCollision(bool enable)
    {
        base.EnableCollision(enable);
        foreach (var child in m_ChildList)
            child.EnableCollision(enable);
    }

    protected TChild GenerateChild()
    => PrefabGenerator.GenerateLocalPrefab(ChildPrefabName).GetComponent<TChild>();
    protected TChild[] GenerateChildren(int count)
    => Enumerable.Range(0, count).Select(_ => GenerateChild()).ToArray();

    override public string ToString() => m_ChildList.ToString();
}
abstract public class StackChildBehaviour<TParent, TChild, TInfo> : LocalPoolableBehaviour, IStackable<TParent, TChild, TInfo>
where TParent : StackParentBehaviour<TParent, TChild, TInfo>
where TChild : StackChildBehaviour<TParent, TChild, TInfo>
where TInfo : INetworkSerializable
{
    protected Collider[] m_Colliders;

    protected TParent m_Parent;
    protected TInfo m_ChildInfo;

    override protected void Awake()
    {
        m_Colliders = GetComponentsInChildren<Collider>();
        base.Awake();
    }
    public void Set(TInfo info, TParent parent)
    {
        m_ChildInfo = info;
        SetParent(parent);
        OnSet(info);
    }
    public void SetParent(TParent parent)
    {
        m_Parent = parent;
        transform.SetParent(parent.transform, false);
        EnableCollision(!parent.IsGrabbed);
    }
    abstract public void OnSet(TInfo info);
    virtual public void EnableCollision(bool enable)
    {
        foreach (var collider in m_Colliders)
            collider.isTrigger = !enable;
    }
}



// public interface IStackable<TParent, TChild, TInfo>
// where TParent : StackParentBehaviour<TParent, TChild, TInfo>
// where TChild : IStackable<TParent, TChild, TInfo>
// where TInfo : INetworkSerializable
// {
//     Transform transform { get; }
//     void OnPool();
//     void Set(TInfo serial, TParent parent);
//     void SetParent(TParent parent);
//     void OnSet(TInfo info);
//     void EnableCollision(bool enable);
// }
// abstract public class StackChildBehaviour<TParent, TChild, TInfo> : LocalPoolableBehaviour, IStackable<TParent, TChild, TInfo>
// where TParent : StackParentBehaviour<TParent, TChild, TInfo>
// where TChild : StackChildBehaviour<TParent, TChild, TInfo>
// where TInfo : INetworkSerializable
// {
//     protected Collider[] m_Colliders;

//     protected TParent m_Parent;
//     protected TInfo m_ChildInfo;

//     override protected void Awake()
//     {
//         m_Colliders = GetComponentsInChildren<Collider>();
//         base.Awake();
//     }
//     public void Set(TInfo info, TParent parent)
//     {
//         m_ChildInfo = info;
//         SetParent(parent);
//         OnSet(info);
//     }
//     public void SetParent(TParent parent)
//     {
//         m_Parent = parent;
//         transform.SetParent(parent.transform, false);
//         EnableCollision(!parent.IsGrabbed);
//     }
//     abstract public void OnSet(TInfo info);
//     virtual public void EnableCollision(bool enable)
//     {
//         foreach (var collider in m_Colliders)
//             collider.isTrigger = !enable;
//     }
// }

// abstract public class StackParentBehaviour<TParent, TChild, TInfo> : BaseItem
// where TParent : StackParentBehaviour<TParent, TChild, TInfo>
// where TChild : IStackable<TParent, TChild, TInfo>
// where TInfo : INetworkSerializable
// {
//     const int MAX_STACK = byte.MaxValue;
//     abstract protected float CHILD_MASS { get; }
//     abstract protected LocalPrefabName ChildPrefabName { get; }
//     protected ReactiveCollectionEx<TChild> m_ChildrenRC;
//     public ReactiveCollectionEx<TChild> ChildrenRC => m_ChildrenRC;

//     override public void OnSpawn()
//     {
//         m_ChildrenRC = new ReactiveCollectionEx<TChild>();
//         Subscriptions.AddRange(new[]{
//             m_ChildrenRC.ObserveCountChanged().Subscribe(count=>{
//                 m_Rigidbody.mass = count*CHILD_MASS;
//                 if(count>0)
//                     Align();
//                 else
//                     OnPool();
//             }),
//             m_ChildrenRC.ObserveRemove().Subscribe(removeEvent=>{
//                 foreach(var removed in removeEvent.Values){
//                     if(removed.transform.parent==transform)
//                         removed.OnPool();
//                 }
//             }),
//         });
//         base.OnSpawn();
//     }

//     override public void OnPool()
//     {
//         foreach (var child in m_ChildrenRC)
//             child.OnPool();
//         m_ChildrenRC.Dispose();
//         m_ChildrenRC = null;
//         base.OnPool();
//     }

//     // override public void OnPlayerEnteredRoom(Player newPlayer)
//     // {
//     //     if (PhotonNetwork.LocalPlayer.IsMasterClient)
//     //         photonView.RPC(nameof(AddChildren), newPlayer, m_ChildrenRC.Select(child => child.Serialize()).ToArray());
//     // }
//     // abstract override public void OnPhotonInstantiate(PhotonMessageInfo info);


//     public void RpcHandover(IGrabber grabber, TChild child)
//     => GenereteParentToGrabber(grabber, m_ChildrenRC.IndexOf(child));
//     public void RpcHandoverTop(IGrabber grabber)
//     => GenereteParentToGrabber(grabber, m_ChildrenRC.Count - 1);
//     public void RpcHandoverBottom(IGrabber grabber)
//     => GenereteParentToGrabber(grabber, 0);

//     // public void RpcHandoverChild(TParent receiver, TChild child)
//     // => photonView.RPC(nameof(HandoverChild), RpcTarget.AllViaServer, receiver.photonView.ViewID, m_ChildrenRC.IndexOf(child));
//     // public void RpcHandoverTopChild(TParent receiver)
//     // => photonView.RPC(nameof(HandoverChild), RpcTarget.AllViaServer, receiver.photonView.ViewID, m_ChildrenRC.Count - 1);
//     // public void RpcHandoverBottomChild(TParent receiver)
//     // => photonView.RPC(nameof(HandoverChild), RpcTarget.AllViaServer, receiver.photonView.ViewID, 0);
//     // public void RpcHandoverTopChildren(TParent receiver, TChild child)
//     // {
//     //     var index = m_ChildrenRC.IndexOf(child);
//     //     photonView.RPC(nameof(HandoverChildren), RpcTarget.AllViaServer, receiver.photonView.ViewID, index, m_ChildrenRC.Count - index);
//     // }
//     // public void RpcHandoverBottomChildren(TParent receiver, TChild child)
//     // => photonView.RPC(nameof(HandoverChildren), RpcTarget.AllViaServer, receiver.photonView.ViewID, 0, m_ChildrenRC.IndexOf(child) + 1);
//     // public void RpcHandoverAll(TParent receiver)
//     // => photonView.RPC(nameof(HandoverChildren), RpcTarget.AllViaServer, receiver.photonView.ViewID, 0, m_ChildrenRC.Count);

//     [ServerRpc(RequireOwnership = false)]
//     protected void AddChildrenServerRpc(TInfo[] infos)
//     {
//         AddChildren(infos);
//         AddChildrenClientRpc(infos);
//     }
//     [ClientRpc]
//     void AddChildrenClientRpc(TInfo[] infos) => AddChildren(infos);
//     void AddChildren(TInfo[] infos) => m_ChildrenRC.AddRange(GenerateChildren(infos));

//     [ServerRpc(RequireOwnership = false)]
//     protected void HandoverChildServerRpc(ulong receiverId, int index)
//     {
//         var receiver = NetworkFunc.GetComponent<TParent>(receiverId);
//         if (receiver.m_ChildrenRC.Count + 1 > MAX_STACK) return;
//         HandoverChild(receiver, index);
//         HandoverChildClientRpc(receiverId, index);
//     }
//     [ClientRpc]
//     void HandoverChildClientRpc(ulong receiverId, int index)
//     => HandoverChild(NetworkFunc.GetComponent<TParent>(receiverId), index);
//     void HandoverChild(TParent receiver, int index)
//     {
//         var child = m_ChildrenRC[index];
//         child.SetParent(receiver);
//         receiver.ChildrenRC.Add(child);
//         m_ChildrenRC.RemoveAt(index);
//     }

//     [ServerRpc(RequireOwnership = false)]
//     protected void HandoverChildrenServerRpc(ulong receiverId, int index, int count)
//     {
//         var receiver = NetworkFunc.GetComponent<TParent>(receiverId);
//         var childCnt = receiver.ChildrenRC.Count;
//         if (childCnt + count > MAX_STACK)
//             count = MAX_STACK - childCnt;
//         if (count <= 0) return;
//         HandoverChildren(receiver, index, count);
//     }
//     [ClientRpc]
//     void HandoverChildrenClientRpc(ulong receiverId, int index, int count)
//     => HandoverChildren(NetworkFunc.GetComponent<TParent>(receiverId), index, count);
//     void HandoverChildren(TParent receiver, int index, int count)
//     {
//         var children = m_ChildrenRC.GetRange(index, count);
//         foreach (var child in children)
//             child.SetParent(receiver);
//         receiver.ChildrenRC.AddRange(children);
//         m_ChildrenRC.RemoveRange(index, count);
//     }

//     protected void HandoverBottomChildrenRpc(ulong receiverId, int index)
//     => HandoverChildrenServerRpc(receiverId, 0, index + 1);
//     protected void HandoverTopChildrenRpc(ulong receiverId, int index)
//     => HandoverChildrenServerRpc(receiverId, index, m_ChildrenRC.Count - index);

//     protected void GenereteParentToGrabber(IGrabber grabber, int boundIndex)
//     {
//         var receiver = PrefabGenerator.SpawnNetworkPrefabInServer(PrefabHash, default, default).GetComponent<TParent>();
//         receiver.RequestChangeParent(grabber);
//         var count = m_ChildrenRC.Count - boundIndex;
//         HandoverChildren(receiver, boundIndex, count);
//         HandoverChildrenClientRpc(receiver.NetworkObjectId, boundIndex, count);
//     }



//     abstract protected void Align();

//     override protected void EnableCollision(bool enable)
//     {
//         base.EnableCollision(enable);
//         foreach (var child in m_ChildrenRC)
//             child.EnableCollision(enable);
//     }

//     protected TChild GenerateChild(TInfo info)
//     {
//         var child = PrefabGenerator.GenerateLocalPrefab(ChildPrefabName).GetComponent<TChild>();
//         child.Set(info, this as TParent);
//         return child;
//     }
//     protected TChild[] GenerateChildren(TInfo[] infos)
//     => infos.Select(GenerateChild).ToArray();

//     override public string ToString() => m_ChildrenRC.ToString();
// }