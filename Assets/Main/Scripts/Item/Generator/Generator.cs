using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI.NetworkVariable;
using Prefab;
using TMPro;

public class Generator : BaseItem, IControllable
{
    [SerializeField] NetworkPrefabName[] m_PrefabNames = default;
    TextMeshPro m_TextMesh;
    NetworkVariableInt m_IndexNV = new NetworkVariableInt();
    public int Index { set { m_IndexNV.Value = value; } get { return m_IndexNV.Value; } }
    override protected void Awake()
    {
        base.Awake();
        m_TextMesh = GetComponentInChildren<TextMeshPro>();
    }
    override public void OnSpawn()
    {
        base.OnSpawn();
        m_IndexNV.OnValueChanged += OnChangeIndex;
    }
    override public void OnPool()
    {
        m_IndexNV.OnValueChanged = null;
        base.OnPool();
    }
    void OnChangeIndex(int pre, int cur)
    {
        m_TextMesh.text = m_PrefabNames[cur].ToString();
    }
    public void Connect(InputManager.HandInput input)
    {
        input.MainClick.AddListener(isDouble =>
        {
            if (isDouble) return;
            if (Index == m_PrefabNames.Length - 1)
                Index = 0;
            else
                Index++;
        });
        input.SubClick.AddListener(isDouble =>
        {
            if (isDouble) return;
            if (Index == 0)
                Index = m_PrefabNames.Length - 1;
            else
                Index--;
        });
    }
}
