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
    NetworkVariableBool m_IsFlyingNV;
    public bool IsFlying { set { m_IsFlyingNV.Value = value; } get { return m_IsFlyingNV.Value; } }

    override public void OnSpawn()
    {
        base.OnSpawn();
        m_IsFlyingNV = new NetworkVariableBool();
        m_IsFlyingNV.OnValueChanged += (pre, cur) =>
        {
            if (cur)
            {
                m_FlyingCTS = new CancellationTokenSource();
                FlyAsync(m_FlyingCTS.Token).Forget();
            }
            else
            {
                m_FlyingCTS?.Cancel();
                m_FlyingCTS = null;
            }
        };
    }
    override public void OnPool()
    {
        m_IsFlyingNV = null;
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
        if (IsServer && Vector3.Dot(transform.forward, m_Rigidbody.velocity) > m_FlyableLimit)
        {
            IsFlying = true;
            m_Rigidbody.angularVelocity = Vector3.zero;
        }
    }
    CancellationTokenSource m_FlyingCTS;
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
