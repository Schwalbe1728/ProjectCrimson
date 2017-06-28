using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActorActionCreator
{
    bool CanPopAction { get; }
    ActorAction PopAction { get; }
}
