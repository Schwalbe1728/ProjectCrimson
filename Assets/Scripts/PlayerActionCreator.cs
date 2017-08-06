using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCreator : MonoBehaviour, ActorActionCreator
{
    private List<ActorAction> DeclaredActions;

    [SerializeField]
    private int MaxActionsToPop = 3;

    private int ActionsLeftForThisFrame;

    public bool CanPopAction { get { return DeclaredActions != null && DeclaredActions.Count > 0 && ActionsLeftForThisFrame > 0; } }

    public ActorAction PopAction
    {
        get
        {
            bool check = CanPopAction;
            ActorAction result = (check) ?
                DeclaredActions[0] : null;

            if(check)
            {
                DeclaredActions.RemoveAt(0);
                ActionsLeftForThisFrame--;
            }

            return result;
        }
    }

	// Use this for initialization
	void Start () {
        DeclaredActions = new List<ActorAction>();
        ActionsLeftForThisFrame = MaxActionsToPop;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        ActionsLeftForThisFrame = MaxActionsToPop;

        if(KeyBindingsWrapper.Movement.MoveKeyPressed)
        {
            MovementAction tmp = null;

            if(KeyBindingsWrapper.Movement.DodgeRollKeyDown)
            {
                tmp = new DodgeRoll(KeyBindingsWrapper.Movement.MoveVector);
            }
            else
            {
                tmp = new Move(
                    KeyBindingsWrapper.Movement.MoveVector,
                    KeyBindingsWrapper.Movement.SprintKeyPressed);
            }

            DeclaredActions.Add(tmp);                   
        }

        if(KeyBindingsWrapper.Movement.JumpKeyDown)
        {
            DeclaredActions.Add(new Jump(Vector3.zero));
        }

		if (KeyBindingsWrapper.Mouse.FireMouseButtonPressed) 
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);

			if(Physics.Raycast(ray, out hit, 100.0f))
			{
				DeclaredActions.Add (new FireWeapon (hit.point));
			}				
		}
	}
}
