using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;

public class PlayerHand : BaseHand, IHumanHand
{
    [SerializeField] bool m_IsLeft;
    public bool IsLeft => m_IsLeft;
    [SerializeField] protected WispHand m_WispHand = default;
    public WispHand WispHand => m_WispHand;
    HandGrabber m_HandGrabber;
    public HandGrabber HandGrabber => m_HandGrabber;
    HandFollower m_HandFollower;
    public HandFollower HandFollower => m_HandFollower;
    [SerializeField] protected LaserTargetFinder m_Laser = default;
    public LaserTargetFinder LaserTargetFinder => m_Laser;
    [SerializeField] protected HandTransform m_HandTransform = default;
    public HandTransform HandTransform => m_HandTransform;
    Animator m_Animator;
    public Animator Animator => m_Animator;
    [SerializeField] HandShapeReactiveProperty m_HandShapeRP = new HandShapeReactiveProperty();
    public IObservable<HandShape> HandShapeAsObservable => m_HandShapeRP;
    override public void Init()
    {
        base.Init();
        m_HandGrabber = GetComponentInChildren<HandGrabber>();
        m_HandFollower = GetComponentInChildren<HandFollower>();
        m_HandGrabber.Init();
        m_Laser.Init();
        m_Animator = GetComponent<Animator>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_HandGrabber.OnSpawn();
        m_Laser.OnSpawn();
        m_WispHand.OnSpawn();
    }
    public void EnableLaser(bool enable) => m_Laser.gameObject.SetActive(enable);
    public void Interact() => LaserTargetFinder.Target?.SendEvent(new InteractEvent(m_HandGrabber.Target as IEventSource ?? m_HandGrabber));
}
