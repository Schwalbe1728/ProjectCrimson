using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActionCreator : MonoBehaviour, ActorActionCreator
{
    private List<ActorAction> DeclaredActions;

    [SerializeField]
    private static int MaxActionsToPop = 3;

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

        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            DeclaredActions.Add(
                                    new Move(
                                            new Vector3(
                                                        Input.GetAxisRaw("Horizontal"), 
                                                        0f,
                                                        Input.GetAxisRaw("Vertical")
                                                        )
                                            )
                                );
        }
	}
}
