using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MaterialSpace;

public class UICube : MonoBehaviour
{
    const float CUBE_RADIUS = 0.1f;
    [SerializeField] MeshRenderer[] m_Meshes = default;
    Transform[] m_MeshAncors = default;
    private void Awake()
    {
        for (var i = 0; i < 8; i++)
        {
            m_MeshAncors[i] = m_Meshes[i].transform;
        }
    }
    public void SetDefault()
    {
        SetPosition(new Vector3(CUBE_RADIUS, CUBE_RADIUS, CUBE_RADIUS));
        SetMaterial(MaterialName.White);
    }
    public void SetPosition(Vector3 position)
    {
        var pos0 = new Vector3(position.x, position.y, position.z);
        var pos1 = new Vector3(-position.x, position.y, position.z);
        var pos2 = new Vector3(position.x, -position.y, position.z);
        var pos3 = new Vector3(position.x, position.y, -position.z);

        m_MeshAncors[0].localPosition = pos0;
        m_MeshAncors[1].localPosition = pos1;
        m_MeshAncors[2].localPosition = pos2;
        m_MeshAncors[3].localPosition = pos3;
        m_MeshAncors[4].localPosition = -pos0;
        m_MeshAncors[5].localPosition = -pos1;
        m_MeshAncors[6].localPosition = -pos2;
        m_MeshAncors[7].localPosition = -pos3;
    }
    public void SetMaterial(MaterialName materialName)
    {
        var material = MaterialManager.GetMaterial(materialName);
        foreach (var mesh in m_Meshes)
        {
            mesh.material = material;
        }
    }
}
