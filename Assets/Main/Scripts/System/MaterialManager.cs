using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MaterialSpace;

public class MaterialManager : SingletonBehaviour<MaterialManager>
{
    [SerializeField] MaterialTable m_MaterialTable = default;
    Dictionary<MaterialName, Material> m_MaterialDictionary;

    public static Material GetMaterial(MaterialName matName) => _Singleton.m_MaterialDictionary[matName];
    public static Material[] GetMaterials(MaterialName[] matNames) => matNames.Select(matName => GetMaterial(matName)).ToArray();

    override protected void Awake()
    {
        base.Awake();
        m_MaterialDictionary = m_MaterialTable.GetTable().ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    [Serializable]
    public class MaterialTable : Serialize.TableBase<MaterialName, Material, MaterialPair> { }
    [Serializable]
    public class MaterialPair : Serialize.KeyAndValue<MaterialName, Material>
    {
        public MaterialPair(MaterialName key, Material value) : base(key, value) { }
    }

}

namespace MaterialSpace
{
    public enum MaterialName
    {
        Red,
        Green,
        Blue,
        Yellow,
        Cyan,
        Magenta,
        DarkRed,
        DarkGreen,
        DrakBlue,
        White,
        Black,
        Gray,
        Gold,
        Silver,
        Bronze,
        BlackMetal,
        BlueMetal,
    }
}