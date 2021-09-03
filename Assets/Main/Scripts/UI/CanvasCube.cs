using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using MLAPI.NetworkVariable;
using Cysharp.Threading.Tasks;

[RequireComponent(typeof(UICube))]
public class CanvasCube : BaseUICube, IControllable
{
    public static Vector3 CanvasSize = new Vector3(1f, 1f, DEFAULT_SIZE);
    UICanvas m_UICanvas;

    override protected void OnScaleChanged(Vector3 pre, Vector3 cur)
    {
        base.OnScaleChanged(pre, cur);
        m_UICanvas.SetScale(cur);
    }
    override protected void Awake()
    {
        base.Awake();
        m_UICanvas = GetComponentInChildren<UICanvas>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        if (IsServer)
        {
            SetDefault();
            LerpScaleAsync(CanvasSize).Forget();
        }
    }
}