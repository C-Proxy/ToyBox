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
    [SerializeField] int m_MaxCount;
    LineRenderer m_LineRenderer;
    Queue<Vector3[]> m_PointsQueue;

    override public void Init()
    {
        m_LineRenderer = GetComponent<LineRenderer>();
    }
    override public void OnSpawn()
    {
        m_PointsQueue = new Queue<Vector3[]>();
    }
    override public void OnPool()
    {
        m_PointsQueue.Clear();
        m_PointsQueue = null;
        m_LineRenderer.positionCount = 0;
    }
    public void AddPosition(Vector3 position)
    => AddPositions(new[] { position });
    public void AddPositions(Vector3[] positions)
    {
        m_PointsQueue.Enqueue(positions);
        if (m_PointsQueue.Count > m_MaxCount)
            m_PointsQueue.Dequeue();
        var array = m_PointsQueue.SelectMany(_ => _).ToArray();
        m_LineRenderer.positionCount = array.Length;
        m_LineRenderer.SetPositions(array);
    }
}
