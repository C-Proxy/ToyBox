using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using Prefab;

public class CoinPlate : BaseItem
{
    InteractEventHandler[] m_CoinStackObservables;

    override protected void Awake()
    {
        base.Awake();
        m_CoinStackObservables = GetComponentsInChildren<InteractEventHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        foreach (var stacker in m_CoinStackObservables)
        {
            stacker.SetEvent(info =>
            {
                switch (info)
                {
                    case InteractEvent interactEvent:
                        switch (interactEvent.Interactor)
                        {
                            case CoinStacker coinStacker:
                                break;
                        }
                        break;
                }

            });
        }
    }

    public static void Generate(Vector3 position)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.CoinPlate, position);
}
