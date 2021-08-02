using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

public class Wingman : BaseItem
{
    public static void Generate(Vector3 position = default, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.Wingman, position, rotation);
}
