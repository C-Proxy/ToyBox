using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICanvas : NetworkPoolableChild
{
    RectTransform m_RectTransform;

    override public void Init()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    public void SetScale(Vector3 scale)
    {
        m_RectTransform.sizeDelta = scale * 1000f;
        m_RectTransform.localPosition = new Vector3(0, 0, -scale.z / 2);
    }
}
