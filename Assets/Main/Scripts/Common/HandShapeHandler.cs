using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class HandShapeHandler : MonoBehaviour
{
    [SerializeField] HandShape m_HandShape = default;
    public HandShape HandShape => m_HandShape;
    public IUniTaskAsyncEnumerable<HandShape> HandShapeAsyncEnumerable { private set; get; }
    [SerializeField] HandOffset m_HandOffset = default;
    public HandOffset HandOffset => m_HandOffset;
    [SerializeField] bool m_UseWorldRotation = default;
    public bool UseWorldRotation => m_UseWorldRotation;

    private void Awake()
    {
        HandShapeAsyncEnumerable = UniTaskAsyncEnumerable.Create<HandShape>(async (writer, token) =>
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.LastPostLateUpdate).WithCancellation(token))
            {
                await writer.YieldAsync(m_HandShape);
            }
        });
    }
}
[Serializable]
public struct HandOffset
{
    [SerializeField] Vector3 m_OffsetPosition, m_OffsetEulerAngles;
    public Vector3 Position => m_OffsetPosition;
    public Vector3 EulerAngles => m_OffsetEulerAngles;

    public HandOffset(Vector3 position, Vector3 eulerAngles)
    {
        m_OffsetPosition = position;
        m_OffsetEulerAngles = eulerAngles;
    }
}