using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx.Triggers;
using UniRx;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class LaserTargetFinder : LocalPoolableChild, IEventSource
{
    const float MAX_DISTANCE = 2f;
    LineRenderer m_LineRenderer;
    [SerializeField] LayerName m_TargetLayer = default;
    [SerializeField] Transform m_TargetAnchor = default;
    SpriteRenderer m_TargetSpriteRenderer;
    TextMeshPro m_TextMesh;
    ReactiveProperty<IRaycastReceivable> m_TargetRP = new ReactiveProperty<IRaycastReceivable>();
    public IObservable<IRaycastReceivable> TargetAsObservable => m_TargetRP;
    public IRaycastReceivable Target { private set { m_TargetRP.Value = value; } get { return m_TargetRP.Value; } }

    override public void Init()
    {
        base.Init();
        m_LineRenderer = GetComponent<LineRenderer>();
        m_TargetSpriteRenderer = m_TargetAnchor.GetComponent<SpriteRenderer>();
        m_TextMesh = m_TargetAnchor.GetComponentInChildren<TextMeshPro>();

    }
    private void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out var hitInfo, MAX_DISTANCE, (int)m_TargetLayer))
        {
            m_TargetAnchor.position = hitInfo.point;
            Target = hitInfo.collider.GetComponent<IRaycastReceivable>();
        }
        else
        {
            m_TargetAnchor.localPosition = new Vector3(0, 0, MAX_DISTANCE);
            Target = null;
        }
        m_LineRenderer.SetPositions(new[] { transform.position, m_TargetAnchor.position });
    }
    public void SetSpriteAndText(SpriteManager.LaserIcon iconId, string text = "")
    {
        m_TargetSpriteRenderer.sprite = SpriteManager.LaserSprite(iconId);
        m_TextMesh.text = text;
    }
    public void ClearTarget() => Target = null;

    // public T FindTarget<T>()
    // where T : BaseObservable
    //  => Target as T;
    // public T FindTarget<T>(int layer)
    // where T : BaseObservable
    // => Target as T;
}
