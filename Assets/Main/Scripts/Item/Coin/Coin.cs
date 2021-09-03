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
    static readonly MaterialName[][] m_MaterialList = new MaterialName[][]{
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//1
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//5
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//10
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//25
        new[]{MaterialName.Green,MaterialName.Blue,MaterialName.Silver},//50
        new[]{MaterialName.Silver,MaterialName.BlackMetal,MaterialName.Gold},//100
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//500
        new[]{MaterialName.Red,MaterialName.Black,MaterialName.Silver},//1000
    };
    override public LocalPrefabName PrefabName => LocalPrefabName.Coin;
    public int Value { private set; get; }
    MeshRenderer m_MeshRenderer;
    TextMeshPro[] m_TextMeshes;
    RaycastEventHandler m_RaycastEventHandler;
    override protected void Awake()
    {
        base.Awake();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_TextMeshes = GetComponentsInChildren<TextMeshPro>();
        m_RaycastEventHandler = GetComponent<RaycastEventHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_RaycastEventHandler.SetEvent(info =>
        {
            switch (info.GrabItem)
            {
                case null:
                case CoinStacker stacker:
                    info.Laser.SetSpriteAndText(SpriteManager.LaserIcon.Default, m_Parent.GetSum(this).ToString());
                    break;
            }
        });
        m_RaycastEventHandler.SetEvent(interactAction: info =>
        {
            switch (info.EventSource)
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
    override public void OnSet(CoinInfo info)
    {
        var index = (int)info.Coinage;
        m_MeshRenderer.materials = MaterialManager.GetMaterials(m_MaterialList[index]);
        Value = CoinValues[index];
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