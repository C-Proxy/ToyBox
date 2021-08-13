using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.NetworkVariable;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using Prefab;

public class FlyingDisc : BaseItem
{
    [SerializeField] float m_Force = default, m_FlyableLimit = default;
    [SerializeField] ActionEventHandler m_ActionHandler = default;
    NetworkVariableBool m_IsFlyingNV = new NetworkVariableBool();
    public bool IsFlying { set { m_IsFlyingNV.Value = value; } get { return m_IsFlyingNV.Value; } }

    override public void OnSpawn()
    {
        base.OnSpawn();

        m_IsFlyingNV.OnValueChanged += OnFlyingChanged;
        m_ActionHandler.SetInteractEvent(info =>
        {
            var interactor = info.Interactor;
            var action = info.ActionInfo;
            switch (action)
            {
                case DamageAction damageAction:
                    ReceiveDamage();
                    break;
            }
        });
    }

    override public void OnPool()
    {
        if (IsOwner)
            IsFlying = false;
        m_IsFlyingNV.OnValueChanged -= OnFlyingChanged;
        PrefabGenerator.GenerateLocalPrefab(LocalPrefabName.Eff_Burst, transform.position, transform.rotation);
        base.OnPool();
    }
    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
        IsFlying = false;
    }
    override public void OnRelease(IGrabber parent)
    {
        base.OnRelease(parent);
        var velocity = m_Rigidbody.velocity;
        transform.Rotate(transform.up, Vector3.Angle(transform.forward, velocity));
        if (IsServer && Vector3.Dot(transform.forward, velocity) > m_FlyableLimit)
        {
            IsFlying = true;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
    void ReceiveDamage()
    {
        DespawnServerRpc();
    }
    CancellationTokenSource m_FlyingCTS;
    void OnFlyingChanged(bool previous, bool current)
    {
        if (current)
        {
            if (m_FlyingCTS != null)
                return;
            m_FlyingCTS = new CancellationTokenSource();
            FlyAsync(m_FlyingCTS.Token).Forget();
            Debug.Log("StartFlying");
        }
        else
        {
            m_FlyingCTS?.Cancel();
            m_FlyingCTS = null;
            Debug.Log("StopFlying");
        }
    }
    async UniTaskVoid FlyAsync(CancellationToken token)
    {
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                var force = -Vector3.Dot(transform.up, m_Rigidbody.velocity) * m_Force;
                m_Rigidbody.AddForce(transform.up * force, ForceMode.Force);
                token.ThrowIfCancellationRequested();
                await UniTask.Yield();
            }
        }
        catch { }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (IsServer)
            IsFlying = false;
    }

    public static void Generate(Vector3 position = default, Quaternion rotation = default)
    => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.FlyingDisc, position, rotation);
}
