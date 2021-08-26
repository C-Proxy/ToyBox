using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UniRx;
using UniRx.Triggers;

public class WispHand : NetworkPoolableChild
{
    const float SPEED = 10f;
    SpriteRenderer m_SpriteRenderer;
    protected void Awake()
    {
        m_SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_SpriteRenderer.gameObject.SetActive(false);
    }
    public void SetSpritePosition(Vector3 position) => m_SpriteRenderer.transform.localPosition = position;

    public void EnableTrace(Transform handAnchor, CancellationToken token)
    {
        m_SpriteRenderer.gameObject.SetActive(true);
        UniTask.Run(async () =>
        {
            await UniTask.SwitchToMainThread();
            var startPos = handAnchor.localPosition;
            try
            {
                await UniTaskAsyncEnumerable.EveryUpdate().ForEachAsync(_ =>
                {
                    transform.localPosition = startPos + (handAnchor.localPosition - startPos) * SPEED;
                    transform.localRotation = handAnchor.localRotation;
                }, token);
            }
            catch (OperationCanceledException) { }
            finally
            {
                if (UnityEditor.EditorApplication.isPlaying)
                {
                    transform.localPosition = default;
                    m_SpriteRenderer.gameObject.SetActive(false);
                }
            }
        });
    }
}
