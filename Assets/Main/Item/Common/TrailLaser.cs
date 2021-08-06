using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

[RequireComponent(typeof(LineRenderer))]
public class TrailLaser : LocalPoolableChildBehaviour
{
    [SerializeField] float m_DisplayTime = 1f;
    LineRenderer m_LineRenderer;
    Queue<Vector3[]> m_PositionsQueue;

    CancellationTokenSource m_RemoveCTS;
    override public void Init()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }
    override public void OnSpawn()
    {
        m_PositionsQueue = new Queue<Vector3[]>();
        AddPosition(transform.position);
        m_RemoveCTS = new CancellationTokenSource();
        RemoveAsync(m_RemoveCTS.Token);
    }
    override public void OnPool()
    {
        ClearPositions();
        m_PositionsQueue = null;
        m_LineRenderer.positionCount = 0;
        m_RemoveCTS?.Cancel();
        m_RemoveCTS = null;
    }
    public void AddPosition(Vector3 position)
    => AddPositions(new[] { position });
    public void AddPositions(Vector3[] positions)
    {
        m_PositionsQueue.Enqueue(positions);
        UpdateLaser();
    }
    public void RemovePosition()
    {
        m_PositionsQueue.Dequeue();
        UpdateLaser();
    }
    public void ClearPositions() => m_PositionsQueue.Clear();
    async void RemoveAsync(CancellationToken token)
    {
        try
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate())
            {
                await UniTask.Delay(TimeSpan.FromSeconds(m_DisplayTime), false, PlayerLoopTiming.Update, token);
                RemovePosition();
            }
        }
        catch
        {

        }
    }
    void UpdateLaser()
    {
        var array = m_PositionsQueue.SelectMany(_ => _).ToArray();
        m_LineRenderer.positionCount = array.Length;
        m_LineRenderer.SetPositions(array);
    }
}
