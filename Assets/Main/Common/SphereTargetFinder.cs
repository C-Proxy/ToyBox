using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SphereTargetFinder : MonoBehaviour, ITargetFinder
{
    const int NUM_FINDABLE = 32;
    float m_GrabRadius = 0.1f;
    Collider[] mem_Colliders = new Collider[NUM_FINDABLE];
    public T FindTarget<T>()
    where T : BaseObservable => default;
    public T FindTarget<T>(int layer)
    where T : BaseObservable
    {
        var pos = transform.position;
        var length = Physics.OverlapSphereNonAlloc(pos, m_GrabRadius, mem_Colliders, layer);

        return mem_Colliders
            .Take(length)
            .Select(collider => (collider, collider.GetComponent<T>()))
            .Where(tuple => tuple.Item2?.IsTargettable ?? false)
            .OrderBy(tuple =>
            {
                var collider = tuple.collider;
                return Vector3.SqrMagnitude(collider.ClosestPoint(pos) - collider.transform.position);
            }).FirstOrDefault().Item2;
    }
}
