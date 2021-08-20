using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;
using GunSpace;
public class WingmanAmmo : BaseAmmo
{
    override protected BulletType BulletType => BulletType.WingmanAmmo;
    public static void Generate(Vector3 position = default, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.WingmanAmmo, position, rotation);
}
