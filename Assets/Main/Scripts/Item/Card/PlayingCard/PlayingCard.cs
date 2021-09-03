using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MLAPI.Serialization;
using Prefab;
using PlayingCardSpace;
public class PlayingCard : StackChildBehaviour<PlayingCardStacker, PlayingCard, CardInfo>
{
    override public LocalPrefabName PrefabName => LocalPrefabName.PlayingCard;
    SpriteRenderer m_SpriteRenderer;
    RaycastEventHandler m_RaycastEventHandler;

    override protected void Awake()
    {
        base.Awake();
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_RaycastEventHandler = GetComponent<RaycastEventHandler>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        //None->Hand:Draw,None->Deck:DrawTop,Hand->Hand:Draw,Hand->Deck:DrawTop,Deck->Hand:DrawAll,Deck->Deck:DrawDeck,
        m_RaycastEventHandler.SetEvent(info =>
        {
            var iconId = info.GrabItem switch
            {
                null => m_Parent.IsDeck ? SpriteManager.LaserIcon.DrawTop : SpriteManager.LaserIcon.DrawCard,
                PlayingCardStacker cardController when cardController.IsDeck => m_Parent.IsDeck ? SpriteManager.LaserIcon.SeparateDeck : SpriteManager.LaserIcon.DrawAll,
                PlayingCardStacker cardController => m_Parent.IsDeck ? SpriteManager.LaserIcon.DrawTop : SpriteManager.LaserIcon.DrawCard,
                _ => SpriteManager.LaserIcon.Default,
            };
            info.Laser.SetSpriteAndText(iconId);
        });
        m_RaycastEventHandler.SetEvent(interactAction: info =>
        {
            switch (info.EventSource)
            {
                case HandGrabber grabber:
                    if (m_Parent.IsDeck)
                        m_Parent.HandoverBottomToGrabber(grabber);
                    else
                        m_Parent.HandoverToGrabber(grabber, this);
                    break;
                case PlayingCardStacker cardStacker:
                    if (cardStacker.IsDeck)
                    {
                        if (m_Parent.IsDeck)
                            m_Parent.HandoverBottomChildren(cardStacker, this);
                        else
                            m_Parent.HandoverAll(cardStacker);
                    }
                    else
                    {
                        if (m_Parent.IsDeck)
                            m_Parent.HandoverBottomChild(cardStacker);
                        else
                            m_Parent.HandoverChild(cardStacker, this);
                    }

                    break;
                default:
                    return;
            }

        });
    }
    override public void OnSet(CardInfo info)
    {
        m_SpriteRenderer.sprite = SpriteManager.PlayingCardSprite(info);
    }
    override public string ToString() => m_ChildInfo.ToString();
}

namespace PlayingCardSpace
{
    public struct CardInfo : INetworkSerializable
    {
        const byte JOKER_ID = 52;
        byte m_CardId;

        public int CardId => m_CardId;
        public int Number => m_CardId == JOKER_ID ? 0 : (m_CardId % 13) + 1;
        public SuitName Suit => m_CardId == JOKER_ID ? SuitName.None : (SuitName)(m_CardId / 13);
        public CardInfo(byte cardId)
        {
            m_CardId = cardId;
        }

        public void NetworkSerialize(NetworkSerializer serializer)
        {
            serializer.Serialize(ref m_CardId);
        }
        override public string ToString() => m_CardId == JOKER_ID ? "[Joker]" : $"[Suit:{Suit},{Number}]";
    }
    public enum SuitName
    {
        Spade,
        Heart,
        Diamond,
        Clover,
        None,
    }
}