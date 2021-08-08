using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Prefab;

public class CoinPlate : BaseItem
{
    ActionEventHandler[] m_CoinStackObservables;

    override protected void Awake()
    {
        base.Awake();
        m_CoinStackObservables = GetComponentsInChildren<ActionEventHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        foreach (var stacker in m_CoinStackObservables)
        {
            stacker.SetInteractEvent(info =>
            {
                var interactor = info.Interactor;
                switch (interactor)
                {
                    case CoinStacker coinStacker:
                        break;
                }
            });
        }
    }

    public static void Generate(Vector3 position)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.CoinPlate, position);
}
