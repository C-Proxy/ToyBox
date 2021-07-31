using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickable
{
    BaseGrabber Parent { get; }
    void RequestPick(BaseGrabber parent, bool isDouble);
}