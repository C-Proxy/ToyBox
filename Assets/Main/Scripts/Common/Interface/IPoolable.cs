using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using Prefab;

public interface IPoolable
{

    void OnPool();
    void OnSpawn();
}
public interface IPoolableChild : IPoolable
{
    void Init();
}
public interface IPoolableParent : IPoolable
{
    Transform transform { get; }
    GameObject gameObject { get; }
    void SetActive(bool active);
}
public interface INetworkPoolableParent : INetworkPoolable, IPoolableParent { }
public interface ILocalPoolableParent : ILocalPoolable, IPoolableParent { }
public interface INetworkPoolable : IPoolable
{
    NetworkObject NetworkObject { get; }
    ulong PrefabHash { get; }
    [ServerRpc(RequireOwnership = false)]
    void DespawnServerRpc();
}
public interface ILocalPoolable : IPoolable
{
    LocalPrefabName PrefabName { get; }
}
