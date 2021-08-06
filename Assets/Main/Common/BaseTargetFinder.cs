using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// abstract public class BaseTargetFinder : MonoBehaviour
// {
//     virtual public T FindTarget<T>()
//     where T : BaseObservable
//     => default;
//     virtual public T FindTarget<T>(int layer)
//     where T : BaseObservable
//     => default;
// }
public interface ITargetFinder
{
    Transform transform { get; }
    T FindTarget<T>()
    where T : BaseEventHandler;
    T FindTarget<T>(int layer)
    where T : BaseEventHandler;
}