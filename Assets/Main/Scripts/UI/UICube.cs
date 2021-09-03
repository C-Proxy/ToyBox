using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using MaterialSpace;

public class UICube : MonoBehaviour
{
    [SerializeField] MeshRenderer[] m_Meshes = default;
    [SerializeField] MeshRenderer m_CenterCubeMesh = default;
    Transform m_CenterAnchor = default;
    Transform[] m_MeshAncors = default;

    Vector3 Scale => m_CenterAnchor.localScale;

    void Awake()
    {
        m_MeshAncors = m_Meshes.Select(mesh => mesh.transform).ToArray();
        m_CenterAnchor = m_CenterCubeMesh.transform;
    }

    public void SetScale(Vector3 scale)
    {
        m_CenterAnchor.localScale = scale;
        var half = scale / 2;
        var pos0 = new Vector3(half.x, half.y, half.z);
        var pos1 = new Vector3(-half.x, half.y, half.z);
        var pos2 = new Vector3(half.x, -half.y, half.z);
        var pos3 = new Vector3(half.x, half.y, -half.z);

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
    public void SetCenterMaterial(MaterialName materialName) => m_CenterCubeMesh.material = MaterialManager.GetMaterial(materialName);

}
