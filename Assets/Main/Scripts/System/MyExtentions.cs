using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Serialize
{

    /// <summary>
    /// テーブルの管理クラス
    /// </summary>
    [System.Serializable]
    public class TableBase<TKey, TValue, Type> where Type : KeyAndValue<TKey, TValue>
    {
        [SerializeField]
        private List<Type> list;
        private Dictionary<TKey, TValue> table;


        public Dictionary<TKey, TValue> GetTable()
        {
            if (table == null)
            {
                table = ConvertListToDictionary(list);
            }
            return table;
        }

        /// <summary>
        /// Editor Only
        /// </summary>
        public List<Type> GetList()
        {
            return list;
        }

        static Dictionary<TKey, TValue> ConvertListToDictionary(List<Type> list)
        {
            Dictionary<TKey, TValue> dic = new Dictionary<TKey, TValue>();
            foreach (KeyAndValue<TKey, TValue> pair in list)
            {
                dic.Add(pair.Key, pair.Value);
            }
            return dic;
        }
    }

    /// <summary>
    /// シリアル化できる、KeyValuePair
    /// </summary>
    [System.Serializable]
    public class KeyAndValue<TKey, TValue>
    {
        public TKey Key;
        public TValue Value;

        public KeyAndValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }
        public KeyAndValue(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }


    }
}
static class MyExtensions
{
    public static void Suffle<T>(this IList<T> list, int seed)
    {
        UnityEngine.Random.InitState(seed);
        for (int i = list.Count - 1; i > 1; i--)
        {
            var rnd = UnityEngine.Random.Range(0, i + 1);
            var tmp = list[rnd];
            list[rnd] = list[i];
            list[i] = tmp;
        }
    }
    // public static void ModifiedToAngleAxis(this Quaternion q, out float radian, out Vector3 axis)
    // {
    //     var arcCosW = Mathf.Acos(q.w);
    //     radian = arcCosW * 2;
    //     var sinACosW = Mathf.Sin(arcCosW);
    //     var vector = new Vector3(q.x, q.y, q.z).normalized;
    //     axis = sinACosW >= 0 ? vector : -vector;
    //     if (radian > Mathf.PI)
    //     {
    //         radian -= Mathf.PI * 2;
    //         axis = -axis;
    //     }
    // }
}
