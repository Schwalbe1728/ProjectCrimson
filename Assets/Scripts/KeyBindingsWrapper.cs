using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingsWrapper : MonoBehaviour {
    public static MovementBindings Movement = new MovementBindings();
}

public class MovementBindings
{
    public bool MoveKeyPressed
    {
        get
        {
            return Input.GetButton("Horizontal") || Input.GetButton("Vertical");
        }
    }

    public bool MoveForwardKeyPressed { get { return Input.GetAxisRaw("Vertical") > 0; } }
    public bool MoveBackwardKeyPressed { get { return Input.GetAxisRaw("Vertical") < 0; } }
    public bool StrafeRightKeyPressed { get { return Input.GetAxisRaw("Horizontal") > 0; } }
    public bool StrafeLeftKeyPressed { get { return Input.GetAxisRaw("Horizontal") < 0; } }

    public bool SprintKeyPressed { get { return Input.GetButton("Sprint"); } }

    public bool JumpKeyDown { get { return Input.GetButtonDown("Jump"); } }

    public bool DodgeRollKeyDown { get { return Input.GetButtonDown("DodgeRoll"); } }

    public Vector3 MoveVector
    {
        get
        {
            return new Vector3(Input.GetAxisRaw("Horizontal"),
                               0f,
                               Input.GetAxisRaw("Vertical")
                               );
        }
    }
}