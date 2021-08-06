using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Prefab;
using MLAPI;
using MLAPI.Messaging;

abstract public class PoolableNetworkBehaviour : NetworkPoolableChildBehaviour, INetworkPoolable
{
    public ulong PrefabHash => NetworkObject.PrefabHash;
    public void SetActive(bool active) => gameObject.SetActive(active);
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        PrefabGenerator.DespawnPrefabOnServer(NetworkObject);
    }
    override public void OnPool()
    {
        foreach (var subscription in Subscriptions)
            subscription.Dispose();
        Subscriptions.Clear();
    }
    private void OnApplicationQuit()
    {
        OnPool();
    }
}
abstract public class NetworkPoolableChildBehaviour : NetworkBehaviour, IPoolableChild
{
    protected List<IDisposable> Subscriptions = new List<IDisposable>();
    virtual protected void Awake()
    {
        AfterAwake();
        OnSpawn();
    }
    virtual protected void AfterAwake() { }
    abstract public void OnSpawn();
    abstract public void OnPool();
}
abstract public class LocalPoolableBehaviour : LocalPoolableChildBehaviour, ILocalPoolable
{
    abstract public LocalPrefabName PrefabName { get; }
    virtual protected void Awake()
    {
        AfterAwake();
        OnSpawn();
    }
    virtual protected void AfterAwake() { }
    public void SetActive(bool active) => gameObject.SetActive(active);
    virtual public void Despawn() => PrefabGenerator.PoolLocalObject(this);
    override public void OnPool() { }
}

abstract public class LocalPoolableChildBehaviour : MonoBehaviour, IPoolableChild
{

    // List<IDisposable> m_Subscriptions = new List<IDisposable>();
    // protected List<IDisposable> Subscriptions => m_Subscriptions;
    virtual public void Init() { }
    virtual public void OnSpawn() { }
    virtual public void OnPool()
    {
        // foreach (var subscription in m_Subscriptions)
        // subscription.Dispose();
        // m_Subscriptions.Clear();
    }
}