using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Prefab;
using MLAPI;
using MLAPI.Messaging;

abstract public class NetworkPoolableBehaviour : NetworkPoolableChildBehaviour, INetworkPoolable
{
    public ulong PrefabHash => NetworkObject.PrefabHash;
    public void SetActive(bool active) => gameObject.SetActive(active);
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        PrefabGenerator.DespawnPrefabOnServer(NetworkObject);
    }

}
abstract public class NetworkPoolableChildBehaviour : NetworkBehaviour, IPoolableChild
{
    protected CancellationTokenSource m_AliveCTS;
    protected List<IDisposable> Subscriptions = new List<IDisposable>();
    virtual public void OnSpawn()
    {
        m_AliveCTS = new CancellationTokenSource();
    }
    virtual public void OnPool()
    {
        m_AliveCTS?.Cancel();
        m_AliveCTS = null;
        foreach (var subscription in Subscriptions)
            subscription.Dispose();
        Subscriptions.Clear();
    }
    private void OnApplicationQuit()
    {
        m_AliveCTS?.Cancel();
    }
}
abstract public class LocalPoolableBehaviour : LocalPoolableChildBehaviour, ILocalPoolable
{
    abstract public LocalPrefabName PrefabName { get; }
    // virtual protected void Awake()
    // {
    // AfterAwake();
    // OnSpawn();
    // }
    // virtual protected void AfterAwake() { }
    public void SetActive(bool active) => gameObject.SetActive(active);
    virtual public void Despawn() => PrefabGenerator.PoolLocalObject(this);
}

abstract public class LocalPoolableChildBehaviour : MonoBehaviour, IPoolableChild
{
    protected CancellationTokenSource m_AliveCTS;
    // List<IDisposable> m_Subscriptions = new List<IDisposable>();
    // protected List<IDisposable> Subscriptions => m_Subscriptions;
    virtual public void OnSpawn()
    {
        m_AliveCTS = new CancellationTokenSource();
    }
    virtual public void OnPool()
    {
        m_AliveCTS?.Cancel();
        m_AliveCTS = null;
        // foreach (var subscription in m_Subscriptions)
        // subscription.Dispose();
        // m_Subscriptions.Clear();
    }
}