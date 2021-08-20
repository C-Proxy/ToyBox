using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using PlayingCardSpace;

public class SpriteManager : SingletonBehaviour<SpriteManager>
{
    [SerializeField] LaserIconTable m_LaserIconTable = default;
    [SerializeField] Sprite[] m_PlayingCardSprites = default;
    Dictionary<LaserIcon, Sprite> m_LaserIconDictionary;
    public static Sprite LaserSprite(LaserIcon spriteId) => _Singleton.m_LaserIconDictionary[spriteId];
    public static Sprite PlayingCardSprite(CardInfo info) => _Singleton.m_PlayingCardSprites[info.CardId];

    override protected void Awake()
    {
        base.Awake();
        m_LaserIconDictionary = m_LaserIconTable.GetTable().ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    [Serializable]
    public class LaserIconTable : Serialize.TableBase<LaserIcon, Sprite, LaserIconPair> { }
    [Serializable]
    public class LaserIconPair : Serialize.KeyAndValue<LaserIcon, Sprite>
    {
        public LaserIconPair(LaserIcon key, Sprite value) : base(key, value) { }
    }
    public enum LaserIcon
    {
        Default,
        DrawCard,
        SetCard,
        DrawTop,
        DrawAll,
        SeparateDeck,
        GyroY,
        LockClose,
        LockOpen,
    }
}
