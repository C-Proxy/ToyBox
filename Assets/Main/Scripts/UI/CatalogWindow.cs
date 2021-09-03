using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI.Messaging;
using Prefab;

public class CatalogWindow : NetworkPoolableChild
{
    [SerializeField] ItemButtonEventHandler m_DeckButton = default, m_10CoinButton = default, m_50CoinButton = default, m_100CoinButton = default;
    [SerializeField] ItemButtonEventHandler[] m_CommonButtons = default;
    override public void OnSpawn()
    {
        base.OnSpawn();

        m_DeckButton.SetEvent(info =>
        {
            var source = info.EventSource;
            if (source is IGrabber grabber)
                SpawnServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), info.Value);
        });
        m_10CoinButton.SetEvent(info =>
        {
            var source = info.EventSource;
            var package = new RpcPackage();
            package.Append(2);
            package.Append(10);
            if (source is IGrabber grabber)
                SpawnServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), info.Value, package.ToByteArray());
        });
        m_50CoinButton.SetEvent(info =>
        {
            var source = info.EventSource;
            var package = new RpcPackage();
            package.Append(4);
            package.Append(10);
            if (source is IGrabber grabber)
                SpawnServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), info.Value, package.ToByteArray());
        });
        m_100CoinButton.SetEvent(info =>
        {
            var source = info.EventSource;
            var package = new RpcPackage();
            package.Append(5);
            package.Append(10);
            if (source is IGrabber grabber)
                SpawnServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), info.Value, package.ToByteArray());
        });
        foreach (var eventHandler in m_CommonButtons)
        {
            eventHandler.SetEvent(info =>
            {
                var source = info.EventSource;
                if (source is IGrabber grabber)
                    SpawnServerRpc(NetworkInfo.CreateFrom(grabber.NetworkBehaviour), info.Value);
            });
        }
    }
    override public void OnPool()
    {
        Debug.Log("OnPool");
        foreach (var eventHandler in m_CommonButtons)
            eventHandler.RemoveEvent();
        base.OnPool();
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnServerRpc(NetworkInfo grabberInfo, NetworkPrefabName prefabName, byte[] infos = default)
    {
        var grabber = grabberInfo.ToComponent<IGrabber>();
        var anchor = grabber.GrabAnchor;
        var item = PrefabGenerator.SpawnPrefabOnServer(prefabName, grabber.NetworkBehaviour.OwnerClientId, anchor.position, anchor.rotation, new RpcPackage(infos)).GetComponent<BaseItem>();
        item.RequestChangeParent(grabber);
    }
    // [ServerRpc]
    // void GenerateSpawnCubeServerRpc(NetworkInfo grabberInfo, NetworkPrefabName prefabName)
    // {
    //     var grabber = grabberInfo.ToComponent<IGrabber>();
    //     var anchor = grabber.GrabAnchor;
    //     var cube = PrefabGenerator.SpawnPrefabOnServer(NetworkPrefabName.SpawnerCube, grabber.NetworkBehaviour.OwnerClientId, anchor.position, anchor.rotation).GetComponent<SpawnerCube>();
    //     cube.RequestChangeParent(grabber);
    //     cube.Set(prefabName);
    //     cube.SetClientRpc(prefabName);
    //     cube.CubeScale = new Vector3(1, 1, 1);
    // }
}
