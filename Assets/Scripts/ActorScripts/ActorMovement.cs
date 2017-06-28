using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMovement : MonoBehaviour, ActorActionReceiver {

    private Rigidbody rigidBody;

    private Vector3 MostRecentVectorDeclaration;
    private float timeDelay = 0;


    [SerializeField]
    private ActorMechanics Mechanics;

    public void InterpretAction(ActorAction action)
    {
        if(action is Move && timeDelay <= 0)
        {
            DeclareMovement((action as Move).MovementVector);
            timeDelay += 1.0f / 60;
        }
    }

    public void DeclareMovement(Vector3 mov)
    {
        MostRecentVectorDeclaration = mov;
    }

    // Use this for initialization
    void Start ()
    {
        rigidBody = GetComponent<Rigidbody>();
        if (rigidBody == null) Debug.Log("Nie znaleziono RigidBody");

        MostRecentVectorDeclaration = Vector3.zero;
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float speed = Mechanics.GetFloatStatValue(ActorStatsDeclaration.Speed);
        float delta = 1.0f / 60;

        rigidBody.velocity = MostRecentVectorDeclaration.normalized * speed;
        MostRecentVectorDeclaration = Vector3.zero;

        if (timeDelay > 0) timeDelay -= delta;
    }
}
