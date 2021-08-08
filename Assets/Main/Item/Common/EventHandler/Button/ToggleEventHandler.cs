using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MaterialSpace;

public class ToggleEventHandler : BaseEventHandler, ILaserReceivable
{
    [SerializeField] SpriteManager.LaserIcon m_UpLaserIcon = default, m_DownLaserIcon = default;
    SkinnedMeshRenderer m_SkinnedMeshrenderer;
    Material m_UpMaterial, m_DownMaterial, m_BaseMaterial;

    bool m_IsDown;

    void Awake()
    {
        m_SkinnedMeshrenderer = GetComponent<SkinnedMeshRenderer>();
        var materials = m_SkinnedMeshrenderer.sharedMaterials;
        m_BaseMaterial = materials[0];
        m_UpMaterial = materials[1];
    }
    private void Start()
    {
        m_DownMaterial = MaterialManager.GetMaterial(MaterialName.Red);
    }
    public void Set(bool isDown)
    {
        m_IsDown = isDown;
        m_SkinnedMeshrenderer.materials = new[] { m_BaseMaterial, m_IsDown ? m_DownMaterial : m_UpMaterial };
        m_SkinnedMeshrenderer.SetBlendShapeWeight(0, m_IsDown ? 100f : 0f);
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_SkinnedMeshrenderer.SetBlendShapeWeight(0, 0f);
        m_SkinnedMeshrenderer.materials = new[] { m_BaseMaterial, m_UpMaterial };
    }
    override public void OnPool()
    {
        m_ButtonEvent.RemoveAllListeners();
        base.OnPool();
    }

    UnityEvent<bool> m_ButtonEvent = new UnityEvent<bool>();
    public void SetButtonEvent(UnityAction<bool> action) => m_ButtonEvent.AddListener(action);
    public void Interact(IInteractor interactor, IActionInfo info) => m_ButtonEvent.Invoke(!m_IsDown);
    public void SendFocusInfo(LaserTargetFinder laser, IGrabbable grabbable) => laser.SetSpriteAndText(m_IsDown ? m_DownLaserIcon : m_UpLaserIcon);
}
