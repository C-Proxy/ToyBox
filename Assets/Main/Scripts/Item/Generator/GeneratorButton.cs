using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Prefab;
using CoinSpace;

public class GeneratorButton : MonoBehaviour
{
    [SerializeField] ButtonEventHandler[] m_ButtonObservables = default;

    private void Awake()
    {
        m_ButtonObservables[0].SetEvent(_ => PlayingCardStacker.GenerateFullDeck(transform.position));
        m_ButtonObservables[1].SetEvent(_ => CoinStacker.GenerateCoins(transform.position, transform.rotation, Coinage.Ten, 30));
        m_ButtonObservables[2].SetEvent(_ => PokerBoard.Generate(transform.position));
        m_ButtonObservables[3].SetEvent(_ => Dice.Generate(transform.position));
        m_ButtonObservables[4].SetEvent(_ => CasinoChair.Generate(transform.position));
        m_ButtonObservables[5].SetEvent(_ => CoinPlate.Generate(transform.position));
        m_ButtonObservables[6].SetEvent(_ => AttacheCase.Generate(transform.position));
        m_ButtonObservables[7].SetEvent(_ => Wingman.Generate(transform.position));
        m_ButtonObservables[8].SetEvent(_ => WingmanAmmo.Generate(transform.position));
        m_ButtonObservables[9].SetEvent(_ => FlyingDisc.Generate(transform.position));

    }
}
