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
    NetworkVariable<TInfo[]> m_ChildInfosNV = new NetworkVariable<TInfo[]>(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.ServerOnly }, new TInfo[0]);
    public TInfo[] ChildInfos { protected set { m_ChildInfosNV.Value = value; } get { return m_ChildInfosNV.Value; } }
    public int ChildLength => ChildInfos?.Length ?? 0;
    protected List<TChild> m_ChildList;

    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ChildList = new List<TChild>();
        m_ChildInfosNV.OnValueChanged += (pre, cur) =>
        {
            if (cur.Equals(pre))
                return;
            var infoLength = cur.Length;
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
                    m_ChildList[i].Despawn();
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
        if (IsOwner)
            ChildInfos = new TInfo[0];
        foreach (var child in m_ChildList)
            child.Despawn();
        m_ChildList.Clear();
        m_ChildList = null;
        m_ChildInfosNV.OnValueChanged = null;
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
abstract public class StackChildBehaviour<TParent, TChild, TInfo> : LocalPoolableParent, IStackable<TParent, TChild, TInfo>
where TParent : StackParentBehaviour<TParent, TChild, TInfo>
where TChild : StackChildBehaviour<TParent, TChild, TInfo>
where TInfo : INetworkSerializable
{
    protected Collider[] m_Colliders;

    protected TParent m_Parent;
    protected TInfo m_ChildInfo;

    virtual protected void Awake()
    {
        m_Colliders = GetComponentsInChildren<Collider>();
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