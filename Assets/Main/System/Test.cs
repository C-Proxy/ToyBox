using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System.Linq;
using Cysharp.Threading.Tasks;

public class Test : MonoBehaviour
{
    public void OnInput(InputAction.CallbackContext context) => Debug.Log("");
    public void OnInputFloat(InputAction.CallbackContext context) => Debug.Log(context.ReadValue<float>());
    public void OnInputVector2(InputAction.CallbackContext context) => Debug.Log(context.ReadValue<Vector2>());
}