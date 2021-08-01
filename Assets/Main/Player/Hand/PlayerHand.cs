using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class PlayerHand : BaseHand, IHumanHand
{
    [SerializeField] protected WispHand m_WispHand = default;
    public WispHand WispHand => m_WispHand;
    HandGrabber m_HandGrabber;
    public HandGrabber HandGrabber => m_HandGrabber;
    [SerializeField] protected LaserTargetFinder m_Laser = default;
    public LaserTargetFinder LaserTargetFinder => m_Laser;
    [SerializeField] protected HandTransform m_HandTransform = default;
    public HandTransform HandTransform => m_HandTransform;
    Animator m_Animator;
    public Animator Animator => m_Animator;
    [SerializeField] HandShapeReactiveProperty m_HandShapeRP = new HandShapeReactiveProperty();
    public IObservable<HandShape> HandShapeAsObservable => m_HandShapeRP;
    protected void Awake()
    {
        m_HandGrabber = GetComponentInChildren<HandGrabber>();
        m_Animator = GetComponent<Animator>();
    }
    public void EnableLaser(bool enable) => m_Laser.gameObject.SetActive(enable);
    public void Interact(IInteractor interactor, bool isDouble) => LaserTargetFinder.Target?.Interact(interactor ?? m_HandGrabber, new LaserAction(isDouble));
}
