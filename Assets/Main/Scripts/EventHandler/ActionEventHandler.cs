// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// public class ActionEventHandler : BaseEventHandler, ILaserReceivable
// {
//     UnityEvent<EventInfo> m_InteractEvent = new UnityEvent<EventInfo>();
//     UnityEvent<LaserFocusInfo> m_FocusEvent = new UnityEvent<LaserFocusInfo>();
//     virtual public void SetInteractEvent(UnityAction<EventInfo> action) => m_InteractEvent.AddListener(action);
//     virtual public void SetFocusEvent(UnityAction<LaserFocusInfo> action) => m_FocusEvent.AddListener(action);
//     virtual public void Interact(IInteractor interactor, IActionInfo info) => m_InteractEvent?.Invoke(new EventInfo(interactor, info));
//     virtual public void SendFocusInfo(LaserTargetFinder laser, IGrabbable grabItem) => m_FocusEvent?.Invoke(new LaserFocusInfo(laser, grabItem));

//     override public void OnPool()
//     {
//         m_InteractEvent.RemoveAllListeners();
//         m_FocusEvent.RemoveAllListeners();
//     }
// }
// readonly public struct EventInfo
// {
//     readonly public IInteractor Interactor;
//     readonly public IActionInfo ActionInfo;
//     public EventInfo(IInteractor interactor, IActionInfo actionInfo)
//     {
//         Interactor = interactor;
//         ActionInfo = actionInfo;
//     }
// }
// readonly public struct LaserFocusInfo : IActionInfo
// {
//     readonly public LaserTargetFinder Laser;
//     readonly public IGrabbable GrabItem;
//     public LaserFocusInfo(LaserTargetFinder laser, IGrabbable grabItem)
//     {
//         Laser = laser;
//         GrabItem = grabItem;
//     }
// }