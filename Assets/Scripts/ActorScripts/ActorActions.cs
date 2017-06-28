﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActorActionReceiver
{
    void InterpretAction(ActorAction action);
}

public abstract class ActorAction
{
    public float BaseTimeDelay { get; protected set; }
}

public abstract class MovementAction : ActorAction
{
    public Vector3 MovementVector { get; protected set; }

    public MovementAction(Vector3 movement)
    {
        MovementVector = movement;
    }
}

public class Move : MovementAction
{
    public Move(Vector3 mov) : base(mov)
    {

    }

    public Move(float x, float y, float z) : base(new Vector3(x,y,z))
    { }
}