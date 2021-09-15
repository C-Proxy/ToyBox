using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;

public class Zippo : BaseItem, IControllable
{
    [SerializeField] ParticleSystem m_SparkParticle = default, m_FireParticle = default;
    Animator m_Animator;
    NetworkVariableBool m_IsOpenNV = new NetworkVariableBool(new NetworkVariableSettings { WritePermission = NetworkVariablePermission.OwnerOnly });
    bool IsOpen { set { m_IsOpenNV.Value = value; } get { return m_IsOpenNV.Value; } }
    void IsOpenChanged(bool pre, bool cur)
    {
        m_Animator.SetBool("IsOpen", cur);
        if (!cur && IsServer)
            FireEnabled = false;
    }
    NetworkVariableBool m_FireEnabled = new NetworkVariableBool();
    bool FireEnabled { set { m_FireEnabled.Value = value; } get { return m_FireEnabled.Value; } }
    void FireEnabledChanged(bool pre, bool cur)
    {
        if (cur)
            m_FireParticle.Play();
        else
            m_FireParticle.Stop();
    }

    override protected void Awake()
    {
        base.Awake();
        m_Animator = GetComponentInChildren<Animator>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_IsOpenNV.OnValueChanged += IsOpenChanged;
        m_FireEnabled.OnValueChanged += FireEnabledChanged;
    }
    override public void OnPool()
    {
        m_IsOpenNV.OnValueChanged -= IsOpenChanged;
        m_FireEnabled.OnValueChanged -= FireEnabledChanged;
        base.OnPool();
    }

    [ServerRpc]
    void HitFlintServerRpc()
    {
        if (!IsOpen) return;
        if (!FireEnabled) FireEnabled = true;
        HitFlintClientRpc();
    }
    [ClientRpc] void HitFlintClientRpc() => m_SparkParticle.Emit(8);
    public void Connect(InputManager.HandInput input)
    {
        input.IndexPress.AddListener(pressed => IsOpen = pressed);
        input.MainPress.AddListener(pressed => { if (IsOpen && !pressed) HitFlintServerRpc(); });
    }
}
