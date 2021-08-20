using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

public class CasinoChair : BaseItem
{
    public static void Generate(Vector3 position) => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.CasinoChair, position);
}
