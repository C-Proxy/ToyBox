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
    const float SHOT_POWER = 4000.0f;
    [SerializeField] ArrowHandle m_ArrowHandle = default;
    Transform HandleAnchor => m_ArrowHandle.transform;
    Vector3 m_DefaultPosition;
    NetworkVariable<NetworkObject> m_ArrowObjectNV = new NetworkVariable<NetworkObject>();
    NetworkObject ArrowObject { set { m_ArrowObjectNV.Value = value; } get { return m_ArrowObjectNV.Value; } }
    void OnArrowObjectChanged(NetworkObject pre, NetworkObject cur)
    {
        if (pre != cur && cur)
        {
            if (cur.TryGetComponent<Arrow>(out var arrow))
                HoldAsync(arrow).Forget();
        }
    }

    CancellationTokenSource m_HoldCTS;

    override protected void Awake()
    {
        base.Awake();
        m_DefaultPosition = HandleAnchor.localPosition;
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
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.PostLateUpdate))
            {
                if (!arrow.IsGrabbed) break;
                m_HoldCTS.Token.ThrowIfCancellationRequested();
                HandleAnchor.position = arrow.TailAnchor.position;
                meshAnchor.LookAt(transform, HandleAnchor.up);
                await UniTask.Yield();
            }
        }
        catch (OperationCanceledException) { throw; }
        finally
        {
            m_HoldCTS = null;
            arrow.ResetMeshAnchor();
            ResetHandlePosition();
            if (IsServer)
                ArrowObject = null;
        }
        if (IsOwner)
        {
            arrowAnchor.position = (transform.position + HandleAnchor.position) / 2;
            arrowAnchor.LookAt(transform, HandleAnchor.up);
            arrow.SyncTransform();
        }
        if (IsServer)
            arrow.ProjectAsync((transform.position - HandleAnchor.position).sqrMagnitude * SHOT_POWER).Forget();
    }
    public void ResetHandlePosition()
    {
        HandleAnchor.localPosition = m_DefaultPosition;
        HandleAnchor.localRotation = Quaternion.identity;
    }
}
