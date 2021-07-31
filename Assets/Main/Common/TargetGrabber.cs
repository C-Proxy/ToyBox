using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;

// public readonly struct GrabberHandler
// {
//     readonly List<IGrabber> m_Grabbers;
//     public IGrabber GetTargetGrabber(int index) => m_Grabbers[index];
//     public GrabberHandler()
//     {
//         m_Grabbers = new List<IGrabber>();
//     }
//     public GrabberHandler(List<IGrabber> grabbers)
//     {
//         m_Grabbers = grabbers.ToList();
//     }
//     public void Append(IGrabber grabber)
//     {
//         var nullIndex = m_Grabbers.IndexOf(null);
//         if (nullIndex >= 0)
//         {
//             grabber.GrabberInfo = new GrabberInfo(m_PhotonId, nullIndex);
//             m_Grabbers[nullIndex] = grabber;
//         }
//         else
//         {
//             grabber.GrabberInfo = new GrabberInfo(m_PhotonId, m_Grabbers.Count);
//             m_Grabbers.Add(grabber);
//         }
//     }
//     public void Append(IList<IGrabber> grabbers)
//     {
//         foreach (var grabber in grabbers)
//             Append(grabber);
//     }
//     public void Remove(int index)
//     {
//         m_Grabbers[index] = null;
//     }
//     public void Clear() => m_Grabbers.Clear();
// public static GrabberHandler CreationFromGrabAnchors(bool isLeft, int photonId, Transform grabAnchor, ITargetFinder[] finders)
// {
//     var grabbers = Enumerable.Range(0, finders.Length).Select(index => BaseGrabber.CreationFromGrabAnchor(isLeft, new GrabberInfo(photonId, index), grabAnchor, finders[index]) as IGrabber).ToList();
//     return new GrabberHandler(photonId, grabbers);
// }
// public BaseGrabber Append(bool isLeft, Transform grabAnchor, ITargetFinder finder)
// {
//     // var grabber = BaseGrabber.CreationFromGrabAnchor(isLeft, new GrabberInfo(m_PhotonId, m_TargetGrabbers.Count), grabAnchor, finder);
//     m_TargetGrabbers.Add(grabber);
//     return grabber;
// }
// public HandGrabber Append(bool isLeft, PlayerHand playerHand, LaserTargetFinder laser)
// {
//     // var grabber = new HandGrabber(isLeft, new GrabberInfo(m_PhotonId, m_TargetGrabbers.Count), playerHand, laser);
//     m_TargetGrabbers.Add(grabber);
//     return grabber;
// }
// public BaseGrabber[] AppendRange(bool isLeft, Transform grabAnchor, ITargetFinder[] finders)
// {
//     var photonId = m_PhotonId;
//     var count = m_TargetGrabbers.Count;
//     var grabbers = Enumerable.Range(0, finders.Length).Select(index => BaseGrabber.CreationFromGrabAnchor(isLeft, new GrabberInfo(photonId, count + index), grabAnchor, finders[index])).ToArray();
//     m_TargetGrabbers.AddRange(grabbers);
//     return grabbers;
// }
// }
// public class BaseGrabber : IInteractor, IGrabber
// {
//     readonly public bool m_IsLeft;
//     public bool IsLeft => m_IsLeft;
//     readonly public GrabberInfo m_GrabberInfo;
//     public GrabberInfo GrabberInfo => m_GrabberInfo;
//     ReactiveProperty<IGrabbable> m_TargetRP = new ReactiveProperty<IGrabbable>();
//     public IObservable<IGrabbable> TargetAsObservable => m_TargetRP;
//     public IGrabbable TargetItem { set { m_TargetRP.Value = value; } get { return m_TargetRP.Value; } }
//     Subject<Unit> m_ReleaseSubject = new Subject<Unit>();
//     public IObservable<Unit> ReleaseAsObservable => m_ReleaseSubject;
//     public ITargetFinder TargetFinder { protected set; get; }
//     ReactiveProperty<Transform> m_GrabAnchorRP;
//     public Transform GrabAnchor { protected set { m_GrabAnchorRP.Value = value; } get { return m_GrabAnchorRP.Value; } }
//     public IObservable<Transform> GrabAnchorAsObservable => m_GrabAnchorRP;


//     public BaseGrabber(bool isLeft, GrabberInfo info, Transform grabAnchor, ITargetFinder finder)
//     {
//         m_IsLeft = isLeft;
//         m_GrabberInfo = info;
//         m_GrabAnchorRP = new ReactiveProperty<Transform>(grabAnchor);
//         TargetFinder = finder;

//         TargetAsObservable.Subscribe(target => OnGrab(target)).AddTo(grabAnchor);
//     }
//     public static BaseGrabber CreationFromGrabAnchor(bool isLeft, GrabberInfo info, Transform grabAnchor, ITargetFinder finder) => new BaseGrabber(isLeft, info, grabAnchor, finder);

//     public void Grab() => TargetFinder.FindTarget<GrabObservable>((int)LayerName.GrabTarget)?.Grab(this);
//     public void Release() => m_ReleaseSubject.OnNext(default);

//     virtual public void OnGrab(IGrabbable grabbable) { }
// }
// public class HandGrabber : BaseGrabber
// {
//     BaseHand m_BaseHand;
//     LaserTargetFinder m_LaserFinder;

//     public HandGrabber(bool isLeft, GrabberInfo info, PlayerHand playerHand, LaserTargetFinder laser) : base(isLeft, info, playerHand.GrabAnchor, playerHand.SphereFinder)
//     {
//         m_LaserFinder = laser;
//     }
//     public void ChangeHand(BaseHand hand)
//     {
//         m_BaseHand = hand;
//         TargetFinder = hand.SphereFinder;
//         GrabAnchor = hand.GrabAnchor;
//     }
//     public void Interact(IInteractor interactor, bool isDouble) => m_LaserFinder.Target?.Interact(interactor ?? this, new LaserAction(isDouble));

//     override public void OnGrab(IGrabbable grabbable)
//     {
//         if (m_BaseHand is WispHand wispHand)
//         {
//             var handShapeHandler = grabbable?.HandShapeHandler;
//             if (handShapeHandler != null)
//                 wispHand.SetSpritePosition(handShapeHandler.HandOffset.Position);
//         }
//     }
// }