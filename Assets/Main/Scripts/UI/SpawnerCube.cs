using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using Prefab;

public class SpawnerCube : BaseUICube
{
    NetworkPrefabName m_SpawnPrefabName;
    [ClientRpc]
    public void SetClientRpc(NetworkPrefabName prefabName) => Set(prefabName);
    public void Set(NetworkPrefabName prefabName) => m_SpawnPrefabName = prefabName;
    override public void OnRelease(IGrabber parent)
    {
        base.OnRelease(parent);
        if (IsOwner)
            DespawnServerRpc();
    }
}