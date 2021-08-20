using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using MaterialSpace;

public class ToggleEventHandler : BaseButtonEventHandler<BooleanEvent>, IRaycastReceivable
{
    [SerializeField] SpriteManager.LaserIcon m_UpLaserIcon = default, m_DownLaserIcon = default;
    override protected SpriteManager.LaserIcon GetFocusIcon() => m_IsDown ? m_UpLaserIcon : m_DownLaserIcon;
    override protected string GetFocusText() => "";
    override protected BooleanEvent SendInfo => new BooleanEvent(!m_IsDown);
    SkinnedMeshRenderer m_SkinnedMeshrenderer;
    Material m_UpMaterial, m_DownMaterial, m_BaseMaterial;

    bool m_IsDown;

    override public void Init()
    {
        base.Init();
        m_SkinnedMeshrenderer = GetComponent<SkinnedMeshRenderer>();
        var materials = m_SkinnedMeshrenderer.sharedMaterials;
        m_BaseMaterial = materials[0];
        m_UpMaterial = materials[1];
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        Set(false);
        m_SkinnedMeshrenderer.materials = new[] { m_BaseMaterial, m_UpMaterial };
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

}
