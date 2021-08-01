using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using System.Linq;
using Prefab;
using CoinSpace;
using MLAPIPlayerSpace;

public class PrefabGenerator : SingletonNetworkBehaviour<PrefabGenerator>
{
    override protected void Awake()
    {
        base.Awake();
        m_PrefabHashes = m_NetworkPrefabs.Select(prefab => prefab.GetComponent<NetworkObject>().PrefabHash).ToArray();
        m_NetworkPrefabDicitionary = Enumerable.Zip(m_PrefabHashes, m_NetworkPrefabs, (hash, prefab) => (hash, prefab)).ToDictionary(pair => pair.hash, pair => pair.prefab);
        m_NetworkPoolDictionary = m_PrefabHashes.ToDictionary(hash => hash, _ => new Queue<INetworkPoolable>());

        foreach (var hash in m_PrefabHashes)
        {
            NetworkSpawnManager.RegisterSpawnHandler(hash, (position, rotation) => GetSpawnNetworkObject(hash, position, rotation));
            NetworkSpawnManager.RegisterDestroyHandler(hash, PoolNetworkObject);
        }
    }
    #region NetworkObject
    [SerializeField, EnumIndex(typeof(NetworkPrefabName))] GameObject[] m_NetworkPrefabs;
    ulong[] m_PrefabHashes;
    Dictionary<ulong, Queue<INetworkPoolable>> m_NetworkPoolDictionary;
    Dictionary<ulong, GameObject> m_NetworkPrefabDicitionary;

    NetworkObject GetSpawnNetworkObject(ulong prefabHash, Vector3 position, Quaternion rotation)
    {
        if (TryGetNetworkPoolObject(prefabHash, out var poolable))
        {
            poolable.SetActive(true);
            poolable.OnSpawn();
            poolable.transform.SetPositionAndRotation(position, rotation);
            return poolable.NetworkObject;
        }
        else
        {
            var prefab = m_NetworkPrefabDicitionary[prefabHash];
            var obj = Instantiate(prefab, position, rotation);
            return obj.GetComponent<NetworkObject>();
        }
    }
    void PoolNetworkObject(NetworkObject networkObject)
    {
        var poolable = gameObject.GetComponent<INetworkPoolable>();
        if (poolable != null)
        {
            poolable.OnPool();
            poolable.transform.SetParent(transform);
            poolable.SetActive(false);
            m_NetworkPoolDictionary[poolable.PrefabHash].Enqueue(poolable);
        }
    }
    bool TryGetNetworkPoolObject(ulong prefabHash, out INetworkPoolable poolable)
    {
        try
        {
            var queue = m_NetworkPoolDictionary[prefabHash];
            if (queue.Count > 0)
            {
                poolable = queue.Dequeue();
                return true;
            }
            else
            {
                poolable = null;
                return false;
            }
        }
        catch
        {
            throw;
        }
    }
    public static void SpawnNetworkPrefab(NetworkPrefabName prefabName, Vector3 position = default, Quaternion rotation = default, int[] infos = default)
    => _Singleton.SpawnServerRpc(prefabName, position, rotation, infos);
    public void SpawnNetworkPrefabWithOwnership(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, ulong ownerId, int[] infos = default)
    => _Singleton.SpawnWithOwnershipServerRpc(prefabName, position, rotation, ownerId, infos);
    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, int[] infos = default)
    {
        var networkObject = GetSpawnNetworkObject(m_PrefabHashes[(int)prefabName], position, rotation);
        networkObject.Spawn();
        PrefabInit(networkObject, infos);
    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnWithOwnershipServerRpc(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, ulong ownerId, int[] infos)
    {
        var networkObject = GetSpawnNetworkObject(m_PrefabHashes[(int)prefabName], position, rotation);
        networkObject.SpawnWithOwnership(ownerId);
        PrefabInit(networkObject, infos);
    }
    public void PrefabInit(NetworkObject prefab, int[] infos)
    => prefab.GetComponent<INetworkInitializable>()?.NetworkInit(infos);

    public static NetworkObject SpawnPrefabWithoutRpc(ulong prefabHash, Vector3 position = default, Quaternion rotation = default)
    {
        if (!IsServer) throw new Exception("Only Server can call SpawnPrefabWithoutRpc");
        var singleton = _Singleton;
        var networkObject = singleton.GetSpawnNetworkObject(prefabHash, position, rotation);
        networkObject.Spawn();
        return networkObject;
    }
    #endregion

    #region LocalObject
    [SerializeField, EnumIndex(typeof(LocalPrefabName))] GameObject[] m_LocalPrefabs;
    Queue<ILocalPoolable>[] m_LocalPools;
    public static GameObject GenerateLocalPrefab(LocalPrefabName prefabName, Vector3 position = default, Quaternion rotation = default)
    {
        var singleton = _Singleton;
        var index = (int)prefabName;
        if (singleton.TryGetLocalPoolObject(index, out var poolable))
        {
            poolable.SetActive(true);
            poolable.OnSpawn();
            poolable.transform.SetPositionAndRotation(position, rotation);
            return poolable.gameObject;
        }
        else
        {
            return Instantiate(singleton.m_LocalPrefabs[index], position, rotation);
        }
    }
    public static void PoolLocalObject(ILocalPoolable poolable)
    {
        var singleton = _Singleton;
        poolable.OnPool();
        poolable.transform.SetParent(singleton.transform);
        poolable.SetActive(false);
        singleton.m_LocalPools[(int)poolable.PrefabName].Enqueue(poolable);
    }
    bool TryGetLocalPoolObject(int index, out ILocalPoolable poolable)
    {
        try
        {
            var queue = m_LocalPools[index];
            if (queue.Count > 0)
            {
                poolable = queue.Dequeue();
                return true;
            }
            else
            {
                poolable = null;
                return false;
            }
        }
        catch
        {
            throw;
        }
    }
    #endregion
}

namespace Prefab
{
    public enum NetworkPrefabName
    {
        Player,
        Test,
        PlayingCardStacker,
        CoinStacker,
        PokerBoard,
        Dice,
        CasinoChair,
        CoinPlate,
        AttacheCase,
    }
    public enum LocalPrefabName
    {
        PlayingCard,
        Coin,

    }
    // [Serializable]
    // public class PrefabTable : Serialize.TableBase<PrefabName, GameObject, PrefabPair> { }
    // [Serializable]
    // public class PrefabPair : Serialize.KeyAndValue<PrefabName, GameObject>
    // {
    //     public PrefabPair(PrefabName key, GameObject value) : base(key, value) { }
    // }
}