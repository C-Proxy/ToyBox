using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

public class Watch : MonoBehaviour
{
    [SerializeField] Transform m_ShortAnchor = default, m_LongAnchor = default;
    private void Awake()
    {
        CountAsync(gameObject.GetCancellationTokenOnDestroy()).Forget();
    }
    async UniTaskVoid CountAsync(CancellationToken token)
    {
        var countEnumerable = UniTaskAsyncEnumerable.Create<(Quaternion ShortRot, Quaternion LongRot)>(async (writer, token) =>
        {
            while (!token.IsCancellationRequested)
            {
                var dateTime = DateTime.Now;
                var minute = dateTime.Minute + dateTime.Second / 60f;
                var hour = dateTime.Hour + minute / 60f;
                await writer.YieldAsync((Quaternion.Euler(0, hour * 30f, 0), Quaternion.Euler(0, minute * 6f, 0)));
                await UniTask.Delay(TimeSpan.FromSeconds(1), cancellationToken: token);
            }
        });
        await foreach (var (shortRot, longRot) in countEnumerable.WithCancellation(token))
        {
            m_ShortAnchor.localRotation = shortRot;
            m_LongAnchor.localRotation = longRot;
        }
    }
}
