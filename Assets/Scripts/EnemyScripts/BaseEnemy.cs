using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : ActorScript {

	void Start ()
	{		
		Mechanics = GetComponent<ActorMechanics> ();
		Movement = GetComponent<ActorMovement> ();
		CombatManager = GetComponent<ActorCombatManager> ();	

		ExperienceManager = GameObject.Find ("EXP Manager").GetComponent<ActorExperienceManager> ();
		RangedAttManager = GameObject.Find ("Ranged Attacks Manager").GetComponent<RangedAttacksManager> ();



		Initialize();

		ActionCreator = GetComponent<EnemyActionCreator>();

		Type = ActorType.Enemy;

		OnBeingHit += GetFucked;
		OnActorDied += EnemyDeath;

		StartCoroutine ("WatchForDeath");
	}

	private void GetFucked(BeingAttackedEventArgs args)
	{
		
	}

	private void EnemyDeath(ActorDiedEventArgs args)
	{
		StopCoroutine ("WatchForDeath");
		Destroy (gameObject);
	}
}
