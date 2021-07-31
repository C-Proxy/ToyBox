using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Prefab;
using CoinSpace;

public class Generator : MonoBehaviour
{
    [SerializeField] ButtonAsObservable[] m_ButtonObservables = default;

    private void Awake()
    {
        m_ButtonObservables[0].SetPressEvent(() => PlayingCardStacker.GenerateFullDeck(transform.position));
        m_ButtonObservables[1].SetPressEvent(() => CoinStacker.GenerateCoins(transform.position, transform.rotation, Coinage.Ten, 30));
        m_ButtonObservables[2].SetPressEvent(() => PokerBoard.Generate(transform.position));
        m_ButtonObservables[3].SetPressEvent(() => Dice.Generate(transform.position));
        m_ButtonObservables[4].SetPressEvent(() => CasinoChair.Generate(transform.position));
        m_ButtonObservables[5].SetPressEvent(() => CoinPlate.Generate(transform.position));
        m_ButtonObservables[6].SetPressEvent(() => AttacheCase.Generate(transform.position));
    }
}
