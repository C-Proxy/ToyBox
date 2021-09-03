using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetFinder
{
    Transform transform { get; }
    IEventReceivable<T> FindTarget<T>()
    where T : ActionEvent;
    IEventReceivable<T> FindTarget<T>(int layer)
    where T : ActionEvent;
}