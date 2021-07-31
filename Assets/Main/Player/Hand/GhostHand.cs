using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class GhostHand : WispHand, IHumanHand
{
    [SerializeField] protected HandTransform m_HandTransform = default;
    public HandTransform HandTransform => m_HandTransform;
    private void Update()
    {
        // m_HandTransform.CopyHandRotation(m_PlayerHand.HandTransform);
    }
}
