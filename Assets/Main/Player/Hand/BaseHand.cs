using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

abstract public class BaseHand : MonoBehaviour
{

}

[Serializable]
public struct HandTransform
{
    public Transform Hand => m_Hand;
    public Transform[] GetFingerTransforms(FingerName fingerName)
    {
        switch (fingerName)
        {
            case FingerName.Thumb:
                return m_Thumb;
            case FingerName.Index:
                return m_Index;
            case FingerName.Middle:
                return m_Middle;
            case FingerName.Ring:
                return m_Ring;
            case FingerName.Pinky:
                return m_Pinky;
            default:
                return null;
        }
    }
    public Transform GetFingerTransform(FingerName fingerName, int index)
    => GetFingerTransforms(fingerName)[index];
    [SerializeField] Transform m_Hand;
    [SerializeField] Transform[] m_Thumb, m_Index, m_Middle, m_Ring, m_Pinky;
    public HandTransform(Transform hand, Transform[] thumb, Transform[] index, Transform[] middle, Transform[] ring, Transform[] pinky)
    {
        m_Hand = hand;
        m_Thumb = thumb;
        m_Index = index;
        m_Middle = middle;
        m_Ring = ring;
        m_Pinky = pinky;
    }

    public void CopyHandRotation(HandTransform parent)
    {
        m_Hand.rotation = parent.Hand.rotation;
        for (int i = 0; i < 5; i++)
        {
            var fingerName = (FingerName)i;
            foreach (var pair in Enumerable.Zip(GetFingerTransforms(fingerName), parent.GetFingerTransforms(fingerName), (target, source) => (target, source)))
            {
                pair.target.localRotation = pair.source.localRotation;
            }
        }
    }
}
public enum FingerName
{
    Thumb,
    Index,
    Middle,
    Ring,
    Pinky,
}