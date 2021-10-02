using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class Cigarette : BaseItem, IControllable
{
    [SerializeField] float m_FireDuration = 60f;
    [SerializeField] float m_AshLimit = 20f;
    float m_AshValue;
    [SerializeField] Transform m_TipAnchor = default;
    [SerializeField] ParticleSystem m_CigaretteSmoke = default, m_CigaretteAsh = default;
    [SerializeField] float m_TipZ = default, m_RootZ = default;
    SkinnedMeshRenderer m_SkinMeshRenderer;
    NetworkVariableBool m_FireEnabledNV = new NetworkVariableBool();
    bool FireEnabled { set { m_FireEnabledNV.Value = value; } get { return m_FireEnabledNV.Value; } }
    void FireEnabledChanged(bool pre, bool cur)
    {
        if (cur) m_CigaretteSmoke.Play();
        else m_CigaretteSmoke.Stop();
    }
    NetworkVariableFloat m_OuterShapeValueNV = new NetworkVariableFloat();
    float OuterShapeValue { set { m_OuterShapeValueNV.Value = value; } get { return m_OuterShapeValueNV.Value; } }
    void OuterShapeChanged(float pre, float cur)
    {
        m_SkinMeshRenderer.SetBlendShapeWeight(0, cur);
        m_TipAnchor.localPosition = new Vector3(0, 0, Mathf.Lerp(m_TipZ, m_RootZ, cur));
    }
    NetworkVariableFloat m_InnerShapeValueNV = new NetworkVariableFloat();
    float InnerShapeValue { set { m_InnerShapeValueNV.Value = value; } get { return m_InnerShapeValueNV.Value; } }
    void InnerShapeChanged(float pre, float cur)
    {
        m_SkinMeshRenderer.SetBlendShapeWeight(1, cur);
        m_CigaretteAsh.Emit(Mathf.RoundToInt((cur - pre) / 5));
        m_AshValue = 0;
    }
    override protected void Awake()
    {
        base.Awake();
        m_SkinMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_FireEnabledNV.OnValueChanged += FireEnabledChanged;
        m_OuterShapeValueNV.OnValueChanged += OuterShapeChanged;
        m_InnerShapeValueNV.OnValueChanged += InnerShapeChanged;
    }
    override public void OnPool()
    {
        m_FireEnabledNV.OnValueChanged -= FireEnabledChanged;
        m_OuterShapeValueNV.OnValueChanged -= OuterShapeChanged;
        m_InnerShapeValueNV.OnValueChanged -= InnerShapeChanged;
        m_SkinMeshRenderer.SetBlendShapeWeight(0, 0);
        m_SkinMeshRenderer.SetBlendShapeWeight(1, 0);
        m_AshValue = 0;
        base.OnPool();
    }

    CancellationTokenSource m_FireCTS;
    async UniTaskVoid FireAsync()
    {
        m_FireCTS = new CancellationTokenSource();
        var shapeValue = m_SkinMeshRenderer.GetBlendShapeWeight(0);
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                m_FireCTS.Token.ThrowIfCancellationRequested();
                var delta = Time.deltaTime * 100 / m_FireDuration;
                shapeValue += delta;
                m_AshValue += delta;
                if (m_AshValue > m_AshLimit)
                    InnerShapeValue = OuterShapeValue;
                if (shapeValue > 100) break;
                OuterShapeValue = shapeValue;
            }
        }
        catch (OperationCanceledException) { throw; }
        finally
        {
            m_FireCTS = null;
            FireEnabled = false;
        }
        OuterShapeValue = 100;
    }
    [ServerRpc]
    void EnableFireServerRpc()
    {
        FireEnabled = true;
        FireAsync().Forget();
    }
    [ServerRpc]
    void DisableFireServerRpc()
    {
        FireEnabled = false;
        m_FireCTS?.Cancel();
    }

    public void Connect(InputManager.HandInput input)
    {
        input.IndexPress.AddListener(pressed =>
        {
            if (pressed)
                EnableFireServerRpc();
            else
                DisableFireServerRpc();
        });
        input.MainPress.AddListener(pressed =>
        {
            if (!pressed) InnerShapeValue = OuterShapeValue;
        });
        input.SubPress.AddListener(pressed =>
        {
            if (pressed)
            {
                OuterShapeValue = 0;
                InnerShapeValue = 0;
            }
        });
    }


}
