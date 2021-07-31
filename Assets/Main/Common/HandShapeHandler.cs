using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using MLAPIPlayerSpace;

public class HandShapeHandler : MonoBehaviour
{
    [SerializeField] HandShape m_HandShape = default;
    public IObservable<HandShape> HandShapeAsObservable;
    [SerializeField] HandOffset m_HandOffset = default;
    public HandOffset HandOffset => m_HandOffset;
    [SerializeField] bool m_UseWorldRotation = default;
    public bool UseWorldRotation => m_UseWorldRotation;

    private void Awake()
    {
        HandShapeAsObservable = this.UpdateAsObservable().Select(_ => m_HandShape).Publish().RefCount();
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