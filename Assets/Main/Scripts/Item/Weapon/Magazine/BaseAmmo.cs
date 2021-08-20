using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GunSpace
{
    abstract public class BaseAmmo : BaseItem
    {
        abstract protected BulletType BulletType { get; }
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<ReloadEventHandler>(out var reloadHandler) && reloadHandler.IsReloadable(BulletType))
            {
                reloadHandler.SendEvent(new ReloadEvent(this, BulletType, 8));
            }
        }
    }
}