using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Prefab;
using MLAPI;

abstract public class PoolableNetworkBehaviour : NetworkPoolableChildBehaviour, INetworkPoolable
{
    public ulong PrefabHash => NetworkObject.PrefabHash;
    public void SetActive(bool active) => gameObject.SetActive(active);
    virtual public void Destroy() => NetworkObject.Despawn();
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
abstract public class LocalPoolableBehaviour : PoolableChildBehaviour, ILocalPoolable
{
    abstract public LocalPrefabName PrefabName { get; }
    public void SetActive(bool active) => gameObject.SetActive(active);
    virtual public void Destroy() => PrefabGenerator.PoolLocalObject(this);
    override public void OnPool()
    {
        base.OnPool();
    }
}

abstract public class PoolableChildBehaviour : MonoBehaviour, IPoolableChild
{
    List<IDisposable> m_Subscriptions = new List<IDisposable>();
    protected List<IDisposable> Subscriptions => m_Subscriptions;
    virtual protected void Awake()
    {
        AfterAwake();
        OnSpawn();
    }
    virtual protected void AfterAwake() { }
    virtual public void OnSpawn() { }
    virtual protected void OnEnable()
    {
        OnSpawn();
    }
    virtual protected void OnDisable()
    {
        OnPool();
    }
    private void OnApplicationQuit()
    {
        OnPool();
    }
    virtual public void OnPool()
    {
        foreach (var subscription in m_Subscriptions)
            subscription.Dispose();
        m_Subscriptions.Clear();
    }
}