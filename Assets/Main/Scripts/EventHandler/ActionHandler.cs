// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Events;

// public class ActionHandler : MonoBehaviour
// {
//     UnityEvent<IActionEvent> m_Event = new UnityEvent<IActionEvent>();
//     public void SetEvent(UnityAction<IActionEvent> action) => m_Event.AddListener(action);
//     public void SendEvent(IActionEvent info) => m_Event.Invoke(info);
//     private void OnDisable()
//     {
//         m_Event.RemoveAllListeners();
//     }
// }
