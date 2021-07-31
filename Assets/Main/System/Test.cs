using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Test : MonoBehaviour
{
    CancellationTokenSource m_TokenSource;
    public void StartTask()
    {
        m_TokenSource = new CancellationTokenSource();
        UniTask.Run(async () =>
        {
            await UniTask.SwitchToMainThread();
            Debug.Log("Start");
            int count = 0;
            while (!m_TokenSource.Token.IsCancellationRequested)
            {
                count++;
                Debug.Log(count);
                await UniTask.Delay(1000);
            }
            Debug.Log("End");
        });
    }
    public void StopTask() => m_TokenSource?.Cancel();
}