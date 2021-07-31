using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using Cysharp.Threading.Tasks;
using System.Linq;
using TMPro;
using Prefab;
public class Dice : BaseItem
{
    const float UPWARD_LENGTH = 0.8f;
    const float ROTATE_SPEED = 10f;
    TextMeshPro m_TextMesh;
    NetworkVariableByte m_DiceNumberNV;
    byte DiceNumber { set { m_DiceNumberNV.Value = value; } }
    CancellationTokenSource m_ThrowCTS;

    override protected void Awake()
    {
        m_TextMesh = GetComponentInChildren<TextMeshPro>();
        base.Awake();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_DiceNumberNV = new NetworkVariableByte(0);
        m_DiceNumberNV.OnValueChanged += (pre, cur) => SetText(cur);
    }
    override public void OnPool()
    {
        m_DiceNumberNV = null;
        m_ThrowCTS?.Cancel();
        base.OnPool();
    }

    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
        m_ThrowCTS?.Cancel();
    }
    override public void OnRelease(IGrabber parent)
    {
        base.OnRelease(parent);
        SetText(0);
        if (IsServer)
        {
            if (!m_ThrowCTS?.IsCancellationRequested ?? false)
                m_ThrowCTS.Cancel();
            m_ThrowCTS = new CancellationTokenSource();
            var token = m_ThrowCTS.Token;
            UniTask.Run(async () =>
            {
                await UniTask.WaitWhile(() => !m_Rigidbody.IsSleeping(), PlayerLoopTiming.Update, token);
                Vector3 up = transform.up, right = transform.right, forward = transform.forward;
                var vectors = new[] { up, forward, right, -right, -forward, -up };
                byte num = 0;
                for (byte i = 1; i < 7; i++)
                {
                    if (Vector3.Dot(vectors[i - 1], Vector3.up) > UPWARD_LENGTH)
                    {
                        num = i;
                        break;
                    }
                }
                DiceNumber = num;
            }, false, token);
        }
    }
    async override protected UniTask<(Vector3 velocity, Vector3 angularVelocity)> GetMoveInfoAsync()
    {
        var (velocity, angularVelocity) = await base.GetMoveInfoAsync();
        angularVelocity = Random.insideUnitSphere * ROTATE_SPEED;
        return (velocity, angularVelocity);
    }

    void SetText(byte number) => m_TextMesh.text = number != 0 ? number.ToString() : "";

    public static void Generate(Vector3 position) => PrefabGenerator.SpawnNetworkPrefab(NetworkPrefabName.Dice, position);
}
