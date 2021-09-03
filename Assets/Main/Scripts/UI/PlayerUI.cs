using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : NetworkPoolableChild
{
    [SerializeField] ButtonEventHandler[] m_HeaderButtons = default;
    [SerializeField]
    override public void OnSpawn()
    {
        base.OnSpawn();
        var length = m_HeaderButtons.Length;
        for (var i = 0; i < length;i++){

        }
    }
    override public void OnPool()
    {
        base.OnPool();
    }
}
