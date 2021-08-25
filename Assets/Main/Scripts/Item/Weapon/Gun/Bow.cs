using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.Spawning;
using MLAPI.Messaging;
using MLAPI.NetworkVariable;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class Bow : BaseItem
{
    const float SHOT_POWER = 50.0f;
    const float MAX_PULL = 1f;
    [SerializeField] ArrowHandle m_ArrowHandle = default;
    Transform HandleAnchor => m_ArrowHandle.transform;
    Animator m_Animator;
    Vector3 m_DefaultHandlePosition;
    NetworkVariable<NetworkObject> m_ArrowObjectNV = new NetworkVariable<NetworkObject>();
    NetworkObject ArrowObject { set { m_ArrowObjectNV.Value = value; } get { return m_ArrowObjectNV.Value; } }
    void OnArrowObjectChanged(NetworkObject pre, NetworkObject cur)
    {
        if (cur == null)
            m_HoldCTS?.Cancel();
        else if (pre != cur)
        {
            if (cur.TryGetComponent<Arrow>(out var arrow))
                HoldAsync(arrow).Forget();
        }
    }

    CancellationTokenSource m_HoldCTS;

    override protected void Awake()
    {
        base.Awake();
        m_Animator = GetComponent<Animator>();
        m_DefaultHandlePosition = HandleAnchor.localPosition;
        m_ArrowHandle.SetArrowEvent(TrySetArrow);
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_ArrowObjectNV.OnValueChanged += OnArrowObjectChanged;
    }
    override public void OnPool()
    {
        m_ArrowObjectNV.OnValueChanged -= OnArrowObjectChanged;
        base.OnPool();
    }
    override public void OnGrab(IGrabber parent)
    {
        base.OnGrab(parent);
    }
    override public void OnRelease(IGrabber parent)
    {
        m_HoldCTS?.Cancel();
        base.OnRelease(parent);
    }
    public void TrySetArrow(Arrow arrow)
    {
        if (m_HoldCTS != null) return;
        SetArrowServerRpc(arrow.NetworkObjectId);
    }
    [ServerRpc]
    void SetArrowServerRpc(ulong networkObjectId)
    {
        if (m_HoldCTS != null) return;
        ArrowObject = NetworkSpawnManager.SpawnedObjects[networkObjectId];
    }
    async public UniTaskVoid HoldAsync(Arrow arrow)
    {
        if (m_HoldCTS != null) return;
        m_HoldCTS = new CancellationTokenSource();
        var arrowAnchor = arrow.transform;
        var meshAnchor = arrow.TailAnchor;
        var power = 0f;
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PostLateUpdate))
            {
                if (!arrow.IsGrabbed) break;
                m_HoldCTS.Token.ThrowIfCancellationRequested();
                var pulled = m_DefaultHandlePosition.z - HandleAnchor.localPosition.z;
                if (IsServer && pulled < -.025f) throw new OperationCanceledException();
                power = Mathf.Clamp01(pulled / MAX_PULL);
                m_Animator.SetFloat("PullValue", power);
                HandleAnchor.position = arrow.TailAnchor.position;
                meshAnchor.LookAt(transform, HandleAnchor.up);
                await UniTask.Yield();
            }
        }
        catch (OperationCanceledException)
        {
            arrow.ResetMeshAnchor();
            ResetHandlePosition();
        }
        finally
        {
            m_Animator.SetFloat("PullValue", 0);
            m_HoldCTS = null;
            if (IsServer)
                ArrowObject = null;
        }
        if (IsOwner)
        {
            arrowAnchor.SetPositionAndRotation(meshAnchor.position, meshAnchor.rotation);
            arrow.ResetMeshAnchor();
            ResetHandlePosition();
            arrow.SyncTransform();
        }
        if (IsServer)
            arrow.ProjectAsync(power * SHOT_POWER).Forget();
    }
    public void ResetHandlePosition()
    {
        HandleAnchor.localPosition = m_DefaultHandlePosition;
        HandleAnchor.localRotation = Quaternion.identity;
    }
}
