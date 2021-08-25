using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Display : BaseItem
{
    [SerializeField] ButtonEventHandler m_ButtonEventHandler = default;
    CustomMonitorTexture m_MonitorTexture;
    NetworkVariable<Color32[]> m_PixelColorsNV = new NetworkVariable<Color32[]>();
    public Color32[] PixelColors { set { m_PixelColorsNV.Value = value; } get { return m_PixelColorsNV.Value; } }
    NetworkVariableULong m_SenderIdNV = new NetworkVariableULong();
    ulong SenderId { set { m_SenderIdNV.Value = value; } get { return m_SenderIdNV.Value; } }
    override protected void Awake()
    {
        base.Awake();
        m_MonitorTexture = GetComponentInChildren<CustomMonitorTexture>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ButtonEventHandler.SetEvent(_ => ChangeSenderServerRpc(NetworkManager.LocalClientId));
    }
    override public void OnPool()
    {
        base.OnPool();
    }
    [ServerRpc] void ChangeSenderServerRpc(ulong senderId) => SenderId = senderId;
}
