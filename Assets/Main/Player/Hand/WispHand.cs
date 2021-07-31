using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UniRx;
using UniRx.Triggers;

public class WispHand : MonoBehaviour, IPoolableChild
{
    bool m_WispActive;
    const float SPEED = 10f;
    SpriteRenderer m_SpriteRenderer;
    private void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_SpriteRenderer.gameObject.SetActive(false);
    }
    public void SetSpritePosition(Vector3 position) => m_SpriteRenderer.transform.localPosition = position;
    CancellationTokenSource m_TraceCTS;
    public void EnableTrace(Transform handAnchor, Vector3 itemPosition)
    {
        m_WispActive = true;
        // SetSpritePosition(itemPosition);
        m_SpriteRenderer.gameObject.SetActive(true);
        m_TraceCTS = new CancellationTokenSource();
        var token = m_TraceCTS.Token;
        UniTask.Run(async () =>
        {
            await UniTask.SwitchToMainThread();
            var startPos = handAnchor.localPosition;
            while (m_WispActive)
            {
                transform.localPosition = startPos + (handAnchor.localPosition - startPos) * SPEED;
                transform.localRotation = handAnchor.localRotation;
                await UniTask.Yield();
            }
            if (token.IsCancellationRequested)
                throw new OperationCanceledException(token);
            m_TraceCTS = null;
            transform.localPosition = default;
            m_SpriteRenderer.gameObject.SetActive(false);
        }, false, token);
    }
    public void StopTrace() => m_WispActive = false;
    public void OnSpawn() { }
    public void OnPool()
    {
        m_WispActive = false;
        m_TraceCTS?.Cancel();
    }
    private void OnDisable()
    {
        OnPool();
    }
    private void OnApplicationQuit()
    {
        OnPool();
    }
}
