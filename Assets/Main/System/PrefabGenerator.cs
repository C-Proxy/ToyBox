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

public class PrefabGenerator : SingletonNetworkBehaviour<PrefabGenerator>
{
    override protected void Awake()
    {
        base.Awake();
        m_PrefabHashes = m_NetworkPrefabs.Select(prefab => prefab.GetComponent<NetworkObject>().PrefabHash).ToArray();
        m_NetworkPrefabDicitionary = Enumerable.Zip(m_PrefabHashes, m_NetworkPrefabs, (hash, prefab) => (hash, prefab)).ToDictionary(pair => pair.hash, pair => pair.prefab);
        m_NetworkPoolDictionary = m_PrefabHashes.ToDictionary(hash => hash, _ => new Queue<INetworkPoolableParent>());

        foreach (var hash in m_PrefabHashes)
        {
            NetworkSpawnManager.RegisterSpawnHandler(hash, (position, rotation) => GetSpawnNetworkObject(hash, position, rotation));
            NetworkSpawnManager.RegisterDestroyHandler(hash, PoolNetworkObject);
        }

        m_LocalPools = m_LocalPrefabs.Select(_ => new Queue<ILocalPoolableParent>()).ToArray();
    }
    #region NetworkObject
    [SerializeField, EnumIndex(typeof(NetworkPrefabName))] GameObject[] m_NetworkPrefabs;
    ulong[] m_PrefabHashes;
    Dictionary<ulong, Queue<INetworkPoolableParent>> m_NetworkPoolDictionary;
    Dictionary<ulong, GameObject> m_NetworkPrefabDicitionary;

    NetworkObject GetSpawnNetworkObject(ulong prefabHash, Vector3 position, Quaternion rotation)
    {
        if (TryGetNetworkPoolObject(prefabHash, out var poolable))
        {
            poolable.SetActive(true);
            poolable.transform.SetParent(null);
            poolable.transform.SetPositionAndRotation(position, rotation);
            return poolable.NetworkObject;
        }
        else
        {
            var prefab = m_NetworkPrefabDicitionary[prefabHash];
            return Instantiate(prefab, position, rotation).GetComponent<NetworkObject>();
        }
    }
    void PoolNetworkObject(NetworkObject networkObject)
    {
        var poolable = networkObject.GetComponent<INetworkPoolableParent>();
        if (poolable != null)
        {
            poolable.OnPool();
            poolable.transform.SetParent(transform);
            poolable.SetActive(false);
            m_NetworkPoolDictionary[poolable.PrefabHash].Enqueue(poolable);
        }
    }
    bool TryGetNetworkPoolObject(ulong prefabHash, out INetworkPoolableParent poolable)
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
    public static void SpawnNetworkPrefab(NetworkPrefabName prefabName, Vector3 position = default, Quaternion rotation = default, RpcPackage package = default)
    => _Singleton.SpawnServerRpc(prefabName, position, rotation, package?.ToByteArray());
    public void SpawnNetworkPrefabWithOwnership(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, ulong ownerId, RpcPackage package = default)
    => _Singleton.SpawnWithOwnershipServerRpc(prefabName, position, rotation, ownerId, package?.ToByteArray());
    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, byte[] infos = default)
    {
        var networkObject = GetSpawnNetworkObject(m_PrefabHashes[(int)prefabName], position, rotation);
        networkObject.Spawn();
        PrefabInit(networkObject, new RpcPackage(infos));
    }
    [ServerRpc(RequireOwnership = false)]
    void SpawnWithOwnershipServerRpc(NetworkPrefabName prefabName, Vector3 position, Quaternion rotation, ulong ownerId, byte[] infos)
    {
        var networkObject = GetSpawnNetworkObject(m_PrefabHashes[(int)prefabName], position, rotation);
        networkObject.SpawnWithOwnership(ownerId);
        PrefabInit(networkObject, new RpcPackage(infos));
    }
    public void PrefabInit(NetworkObject prefab, RpcPackage package)
    => prefab.GetComponent<INetworkInitializable>()?.NetworkInit(package);

    public static NetworkObject SpawnPrefabOnServer(ulong prefabHash, Vector3 position = default, Quaternion rotation = default)
    {
        if (!IsServer) throw new Exception("Not Server can't call SpawnPrefabOnServer");
        var networkObject = _Singleton.GetSpawnNetworkObject(prefabHash, position, rotation);
        networkObject.Spawn();
        return networkObject;
    }
    public static NetworkObject SpawnPrefabOnServer(NetworkPrefabName prefabName, Vector3 position = default, Quaternion rotation = default)
    => SpawnPrefabOnServer(_Singleton.m_PrefabHashes[(int)prefabName], position, rotation);
    public static NetworkObject SpawnPrefabOnServer(ulong prefabHash, ulong ownerId, Vector3 position = default, Quaternion rotation = default)
    {
        if (!IsServer) throw new Exception("Not Server can't call SpawnPrefabOnServer");
        var networkObject = _Singleton.GetSpawnNetworkObject(prefabHash, position, rotation);
        networkObject.SpawnWithOwnership(ownerId);
        return networkObject;
    }
    public static NetworkObject SpawnPrefabOnServer(NetworkPrefabName prefabName, ulong ownerId, Vector3 position = default, Quaternion rotation = default)
    => SpawnPrefabOnServer(_Singleton.m_PrefabHashes[(int)prefabName], ownerId, position, rotation);
    public static void DespawnPrefabOnServer(NetworkObject networkObject)
    {
        if (!IsServer) throw new Exception("Not Server can't call DespawnPrefabOnServer");
        networkObject.GetComponent<IGrabbable>()?.ForceRelease();
        networkObject.Despawn();
        _Singleton.PoolNetworkObject(networkObject);
    }
    #endregion

    #region LocalObject
    [SerializeField, EnumIndex(typeof(LocalPrefabName))] GameObject[] m_LocalPrefabs;
    Queue<ILocalPoolableParent>[] m_LocalPools;
    public static GameObject GenerateLocalPrefab(LocalPrefabName prefabName, Vector3 position = default, Quaternion rotation = default)
    {
        var singleton = _Singleton;
        var index = (int)prefabName;
        if (singleton.TryGetLocalPoolObject(index, out var poolable))
        {
            poolable.SetActive(true);
            poolable.transform.SetPositionAndRotation(position, rotation);
            // poolable.OnSpawn();
            return poolable.gameObject;
        }
        else
        {
            return Instantiate(singleton.m_LocalPrefabs[index], position, rotation);
        }
    }
    public static void PoolLocalObject(ILocalPoolableParent poolable)
    {
        var singleton = _Singleton;
        poolable.OnPool();
        poolable.transform.SetParent(singleton.transform);
        poolable.SetActive(false);
        singleton.m_LocalPools[(int)poolable.PrefabName].Enqueue(poolable);
    }
    bool TryGetLocalPoolObject(int index, out ILocalPoolableParent poolable)
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
        Wingman,
        WingmanAmmo,
        NormalBullet,
        FlyingDisc,
    }
    public enum LocalPrefabName
    {
        PlayingCard,
        Coin,
        Mock_WingmanAmmo,
        Eff_Burst,
    }
    // [Serializable]
    // public class PrefabTable : Serialize.TableBase<PrefabName, GameObject, PrefabPair> { }
    // [Serializable]
    // public class PrefabPair : Serialize.KeyAndValue<PrefabName, GameObject>
    // {
    //     public PrefabPair(PrefabName key, GameObject value) : base(key, value) { }
    // }

}
public class RpcPackage
{
    List<byte> m_ByteList;
    byte[] m_Array;
    byte[] Array => m_Array ?? (m_Array = m_ByteList.ToArray());
    int m_Index;
    public RpcPackage()
    {
        m_ByteList = new List<byte>();
    }
    public RpcPackage(byte[] infos)
    {
        m_ByteList = infos?.ToList();
    }
    public void Append(dynamic value) => m_ByteList.AddRange(BitConverter.GetBytes(value));
    public void Append(Vector3 value)
    {
        Append(value.x);
        Append(value.y);
        Append(value.z);

    }
    public void Append(Quaternion value)
    {
        Append(value.x);
        Append(value.y);
        Append(value.z);
        Append(value.w);
    }
    public void Append(int[] value)
    {
        Append(value.Length);
        foreach (var v in value)
            Append(v);
    }
    public bool GetBool()
    {
        var result = BitConverter.ToBoolean(Array, m_Index);
        m_Index += sizeof(bool);
        return result;
    }
    public Char GetChar()
    {
        var result = BitConverter.ToChar(Array, m_Index);
        m_Index += sizeof(char);
        return result;
    }
    public short GetShort()
    {
        var result = BitConverter.ToInt16(Array, m_Index);
        m_Index += sizeof(short);
        return result;
    }
    public ushort GetUshort()
    {
        var result = BitConverter.ToUInt16(Array, m_Index);
        m_Index += sizeof(ushort);
        return result;
    }
    public int GetInt()
    {
        var result = BitConverter.ToInt32(Array, m_Index);
        m_Index += sizeof(int);
        return result;
    }
    public uint GetUint()
    {
        var result = BitConverter.ToUInt32(Array, m_Index);
        m_Index += sizeof(uint);
        return result;
    }
    public long GetLong()
    {
        var result = BitConverter.ToInt64(Array, m_Index);
        m_Index += sizeof(long);
        return result;
    }
    public ulong GetUlong()
    {
        var result = BitConverter.ToUInt64(Array, m_Index);
        m_Index += sizeof(ulong);
        return result;
    }
    public float GetFloat()
    {
        var result = BitConverter.ToSingle(Array, m_Index);
        m_Index += sizeof(float);
        return result;
    }
    public Double GetDouble()
    {
        var result = BitConverter.ToDouble(Array, m_Index);
        m_Index += sizeof(double);
        return result;
    }
    public Vector3 GetVector3()
    => new Vector3(GetFloat(), GetFloat(), GetFloat());
    public void GetQuaternion()
    => new Quaternion(GetFloat(), GetFloat(), GetFloat(), GetFloat());
    public int[] GetIntArray()
    => Enumerable.Range(0, GetInt()).Select(_ => GetInt()).ToArray();
    public byte[] ToByteArray() => m_ByteList.ToArray();

}