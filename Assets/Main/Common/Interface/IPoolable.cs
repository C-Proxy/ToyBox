using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using Prefab;

public interface IPoolable : IPoolableChild
{
    Transform transform { get; }
    GameObject gameObject { get; }
    void SetActive(bool active);
}
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
public interface IPoolableChild
{
    void OnPool();
    void OnSpawn();
}