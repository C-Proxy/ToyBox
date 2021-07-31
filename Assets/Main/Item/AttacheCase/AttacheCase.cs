using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

public class AttacheCase : BaseItem
{
    public static void Generate(Vector3 position)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.AttacheCase, position);
}
