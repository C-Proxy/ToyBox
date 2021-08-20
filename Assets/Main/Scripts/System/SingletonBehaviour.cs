using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;


abstract public class SingletonBehaviour<T> : MonoBehaviour
where T : SingletonBehaviour<T>
{
    protected static T _Singleton;
    virtual protected void Awake()
    {
        if (_Singleton == null)
            _Singleton = this as T;
        else
            Destroy(this);
    }
}
abstract public class SingletonNetworkBehaviour<T> : NetworkBehaviour
where T : SingletonNetworkBehaviour<T>
{
    protected static T _Singleton;
    virtual protected void Awake()
    {
        if (_Singleton == null)
            _Singleton = this as T;
        else
            Destroy(this);
    }
}