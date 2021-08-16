using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Prefab;
using MLAPI;
using MLAPI.Messaging;

abstract public class NetworkPoolableParent : NetworkPoolableBehaviour, INetworkPoolableParent
{
    public ulong PrefabHash => NetworkObject.PrefabHash;
    void OnEnable()
    {
        OnSpawn();
    }
    public void SetActive(bool active) => gameObject.SetActive(active);
    [ServerRpc(RequireOwnership = false)]
    public void DespawnServerRpc()
    {
        PrefabGenerator.DespawnPrefabOnServer(NetworkObject);
    }

}
abstract public class NetworkPoolableChild : NetworkPoolableBehaviour, IPoolableChild
{
    virtual public void Init() { }
}
abstract public class LocalPoolableParent : LocalPoolableBehaviour, ILocalPoolableParent
{
    abstract public LocalPrefabName PrefabName { get; }
    private void OnEnable()
    {
        OnSpawn();
    }
    public void SetActive(bool active) => gameObject.SetActive(active);
    virtual public void Despawn() => PrefabGenerator.PoolLocalObject(this);
}

abstract public class LocalPoolableChild : LocalPoolableBehaviour, IPoolableChild
{
    virtual public void Init() { }
}

abstract public class LocalPoolableBehaviour : MonoBehaviour
{
    protected CancellationTokenSource m_AliveCTS;
    private void OnEnable()
    {
        OnSpawn();
    }
    virtual public void OnSpawn()
    {
        m_AliveCTS = new CancellationTokenSource();
    }
    virtual public void OnPool()
    {
        m_AliveCTS?.Cancel();
        m_AliveCTS = null;
    }
}
abstract public class NetworkPoolableBehaviour : NetworkBehaviour
{
    protected CancellationTokenSource m_AliveCTS;
    protected List<IDisposable> m_Subscriptions;
    private void OnEnable()
    {
        OnSpawn();
    }
    virtual public void OnSpawn()
    {
        m_AliveCTS = new CancellationTokenSource();
        m_Subscriptions = new List<IDisposable>();
    }
    virtual public void OnPool()
    {
        m_AliveCTS?.Cancel();
        m_AliveCTS = null;
        foreach (var subscription in m_Subscriptions)
            subscription.Dispose();
        m_Subscriptions.Clear();
        m_Subscriptions = null;
    }
}