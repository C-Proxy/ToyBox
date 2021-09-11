using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;

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
    [SerializeField] HandShape m_HandShape = default;
    public IUniTaskAsyncEnumerable<HandShape> HandShapeAsyncEnumerable { private set; get; }
    override public void Init()
    {
        base.Init();
        m_HandGrabber = GetComponentInChildren<HandGrabber>();
        m_HandFollower = GetComponentInChildren<HandFollower>();
        m_HandGrabber.Init();
        m_Laser.Init();
        m_Animator = GetComponent<Animator>();
        HandShapeAsyncEnumerable = UniTaskAsyncEnumerable.Create<HandShape>(async (writer, token) =>
        {
            CancellationTokenSource tokenSource = default;
            await foreach (var handShapeHandler in UniTaskAsyncEnumerable.EveryValueChanged(m_HandGrabber, grabber => grabber.Target?.HandShapeHandler).WithCancellation(token))
            {
                tokenSource?.Cancel();
                tokenSource = new CancellationTokenSource();
                if (handShapeHandler)
                {
                    await foreach (var handShape in handShapeHandler.HandShapeAsyncEnumerable.WithCancellation(tokenSource.Token))
                        await writer.YieldAsync(handShape);
                }
                else
                {
                    await foreach (var handShape in UniTaskAsyncEnumerable.EveryUpdate(PlayerLoopTiming.LastPostLateUpdate).WithCancellation(tokenSource.Token))
                        await writer.YieldAsync(m_HandShape);
                }
            }
        });
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
