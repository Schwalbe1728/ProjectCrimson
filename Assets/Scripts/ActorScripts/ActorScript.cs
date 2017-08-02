using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorScript : MonoBehaviour {

    [SerializeField]
    protected ActorMovement Movement;

    [SerializeField]
    protected ActorMechanics Mechanics;

    protected ActorActionCreator ActionCreator;

    protected List<ActorActionReceiver> actionReceivers;

    public void Initialize()
    {
        actionReceivers = new List<ActorActionReceiver>();
        actionReceivers.Add(Movement);
        actionReceivers.Add(Mechanics);
    }

    // Use this for initialization
    void Start ()
    {
        Initialize();

        Debug.Log("Actor.Start");
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {        
        while (ActionCreator.CanPopAction)
        {
            ActorAction action = ActionCreator.PopAction;

            foreach (ActorActionReceiver receiver in actionReceivers)
            {
                receiver.InterpretAction(action);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            RegisterBeingAttacked(
                new Attack(AttackType.Melee, 2, 0.5f, 0.05f, 2)
                );
        }
    }    

    public void RegisterBeingAttacked(Attack attack)
    {
        List<KeyValuePair<ActorStatsDeclaration, StatMutator>> temp = attack.GetStatChanges();

        foreach(KeyValuePair<ActorStatsDeclaration, StatMutator> pair in temp)
        {
            Mechanics.AddMutator(pair.Key, pair.Value);
        }

        Mechanics.Health.ApplyDamage(attack.CalculateDamage());

        // Animator dostaje informacje że postać dostała trafiona
        // Rozpatrz przypadek gdy postać oberwała krytyka
    }    
}
