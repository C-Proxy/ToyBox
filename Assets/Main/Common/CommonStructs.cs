using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Serialization;

public struct NetworkInfo : INetworkSerializable
{
    ulong NetworkObjectId;
    ushort NetworkBehaviourId;

    NetworkInfo(ulong objectId, ushort behaviourId)
    {
        NetworkObjectId = objectId;
        NetworkBehaviourId = behaviourId;
    }
    public static NetworkInfo CreateFrom(NetworkBehaviour networkBehaviour)
    => new NetworkInfo(networkBehaviour.NetworkObjectId, networkBehaviour.NetworkBehaviourId);

    public void NetworkSerialize(NetworkSerializer serializer)
    {
        serializer.Serialize(ref NetworkObjectId);
        serializer.Serialize(ref NetworkBehaviourId);
    }
    public NetworkBehaviour ToNetworkBehaviour()
    => NetworkSpawnManager.SpawnedObjects[NetworkObjectId].GetComponent<INetworkHandler>().FindNetworkBehaviour(NetworkBehaviourId);
    public T ToComponent<T>()
    => ToNetworkBehaviour().GetComponent<T>();
}