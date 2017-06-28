using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    private ActorMovement Movement;

    [SerializeField]
    private ActorMechanics Mechanics;

    [SerializeField]
    private PlayerActionCreator ActionCreator;

    private List<ActorActionReceiver> actionReceivers;    

	void Start ()
    {
        actionReceivers = new List<ActorActionReceiver>();
        actionReceivers.Add(Mechanics);
        actionReceivers.Add(Movement);
	}

    private void FixedUpdate()
    {
        while(ActionCreator.CanPopAction)
        {
            ActorAction action = ActionCreator.PopAction;

            foreach(ActorActionReceiver receiver in actionReceivers)
            {
                receiver.InterpretAction(action);
            }
        }
        
    }
}
