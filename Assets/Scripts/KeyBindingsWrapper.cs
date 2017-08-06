using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingsWrapper : MonoBehaviour {
    public static MovementBindings Movement = new MovementBindings();
	public static MouseBidnings Mouse = new MouseBidnings();

	void Update()
	{
		Mouse.Update();
	}
}

public delegate void MouseDown();
public delegate void MouseUp();

public class MouseBidnings
{
	public event MouseDown OnMouseDown;
	public event MouseUp OnMouseUp;

	private enum MouseFunctions
	{
		Fire,
		OpenLevelUpMenu
	}

	private Dictionary<MouseFunctions, string> MouseFunctionsToAxisName
	= new Dictionary<MouseFunctions, string> () {
		{ MouseFunctions.Fire, "Fire1" },
		{ MouseFunctions.OpenLevelUpMenu, "Fire2" }
	};

	private Dictionary<MouseFunctions, bool> MouseButtonAvailability 
	= new Dictionary<MouseFunctions, bool> () {
		{ MouseFunctions.Fire, true },
		{ MouseFunctions.OpenLevelUpMenu, true }
	};

	public void Update()
	{
		if (FireMouseButtonDown) 
		{
			MouseButtonAvailability [MouseFunctions.Fire] = false;

			if (OnMouseDown != null) 
			{
				OnMouseDown ();
			}
		}

		if (FireMouseButtonUp) 
		{
			MouseButtonAvailability [MouseFunctions.Fire] = true;

			if (OnMouseUp != null) 
			{
				OnMouseUp ();
			}
		}
			
	}

	public bool FireMouseButtonPressed
	{
		get 
		{
			return Input.GetAxis (MouseFunctionsToAxisName [MouseFunctions.Fire]) != 0;
		}
	}

	public bool FireMouseButtonDown
	{
		get 
		{
			return FireMouseButtonPressed && MouseButtonAvailability [MouseFunctions.Fire];
		}
	}

	public bool FireMouseButtonUp
	{
		get 
		{
			return !FireMouseButtonPressed && !MouseButtonAvailability [MouseFunctions.Fire];
		}
	}
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