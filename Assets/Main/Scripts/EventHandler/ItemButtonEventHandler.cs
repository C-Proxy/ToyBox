using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prefab;

public class ItemButtonEventHandler : BaseButtonEventHandler<ValueEvent<NetworkPrefabName>>
{
    [SerializeField] NetworkPrefabName m_PrefabName = default;
    [SerializeField] SpriteManager.LaserIcon m_FocusIcon = default;
    [SerializeField] string m_FocusText = default;
    override protected SpriteManager.LaserIcon GetFocusIcon() => m_FocusIcon;
    override protected string GetFocusText() => m_FocusText;
    override protected ValueEvent<NetworkPrefabName> CreateSendInfo(ActionEvent info)
    => new ValueEvent<NetworkPrefabName>(info.EventSource, m_PrefabName);
    override public void SendEvent(DamageEvent info) { }
}
