using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Prefab;
using CoinSpace;

public class MyDebugger : MonoBehaviour
{
    IObservable<Unit> m_UpdateAsObservable;
    IObservable<Unit> KeyDownAsObservable(KeyCode key) => m_UpdateAsObservable.Where(_ => Input.GetKeyDown(key));
    private void Awake()
    {
        // m_UpdateAsObservable = this.UpdateAsObservable().Publish().RefCount();
        // KeyDownAsObservable(KeyCode.Q).Subscribe(_ => PlayingCardStacker.GenerateFullDeck(Vector3.up, default)).AddTo(this);
        // KeyDownAsObservable(KeyCode.W).Subscribe(_ => CoinStacker.GenerateCoins(Vector3.up, default, Coinage.Ten, 30)).AddTo(this);
        // KeyDownAsObservable(KeyCode.E).Subscribe(_ => PokerBoard.Generate()).AddTo(this);
        // KeyDownAsObservable(KeyCode.R).Subscribe(_ => Dice.Generate()).AddTo(this);
    }
}
