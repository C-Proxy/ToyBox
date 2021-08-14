using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Serialization;
using UniRx;
using TMPro;
using Prefab;
using MaterialSpace;
using CoinSpace;
public class Coin : StackChildBehaviour<CoinStacker, Coin, CoinInfo>
{
    static readonly int[] CoinValues = new[] { 1, 5, 10, 25, 50, 100, 500, 1000 };
    override public LocalPrefabName PrefabName => LocalPrefabName.Coin;
    public int Value { private set; get; }
    MeshRenderer m_MeshRenderer;
    TextMeshPro[] m_TextMeshes;
    ActionEventHandler m_ActionObservable;
    override protected void Awake()
    {
        base.Awake();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_TextMeshes = GetComponentsInChildren<TextMeshPro>();
        m_ActionObservable = GetComponent<ActionEventHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ActionObservable.SetFocusEvent(info =>
        {
            switch (info.GrabItem)
            {
                case null:
                case CoinStacker stacker:
                    info.Laser.SetSpriteAndText(SpriteManager.LaserIcon.Default, m_Parent.GetSum(this).ToString());
                    break;
            }
        });
        m_ActionObservable.SetInteractEvent(info =>
          {
              switch (info.Interactor)
              {
                  case HandGrabber grabber:
                      m_Parent.HandoverTopChildrenToGrabber(grabber, this);
                      break;
                  case CoinStacker coinStacker:
                      m_Parent.HandoverTopChildren(coinStacker, this);
                      break;
              }
          });
    }
    override public void OnPool()
    {
        m_ActionObservable.OnPool();
        base.OnPool();
    }
    override public void OnSet(CoinInfo info)
    {
        var coinage = info.Coinage;
        m_MeshRenderer.materials = coinage switch
        {
            Coinage.One => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.Five => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.Ten => MaterialManager.GetMaterials(new[] { MaterialName.Black, MaterialName.Red, MaterialName.Silver }),
            Coinage.Quater => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.Fifty => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.Hundred => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.FiveHundred => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            Coinage.Thousand => MaterialManager.GetMaterials(new[] { MaterialName.Red, MaterialName.Black, MaterialName.Silver }),
            _ => MaterialManager.GetMaterials(new[] { MaterialName.White, MaterialName.White, MaterialName.Silver }),
        };
        Value = CoinValues[(int)coinage];
        foreach (var textMesh in m_TextMeshes)
            textMesh.text = Value.ToString();
    }
    override public string ToString() => Value.ToString();
}
namespace CoinSpace
{
    public struct CoinInfo : INetworkSerializable
    {
        public Coinage Coinage;
        public CoinInfo(Coinage coinage)
        {
            Coinage = coinage;
        }
        public void NetworkSerialize(NetworkSerializer serializer)
        => serializer.Serialize(ref Coinage);
    }
    public enum Coinage
    {
        One,
        Five,
        Ten,
        Quater,
        Fifty,
        Hundred,
        FiveHundred,
        Thousand,
    }
}