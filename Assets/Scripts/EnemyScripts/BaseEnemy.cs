using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : ActorScript {

	[SerializeField]
	private AIActionCreator ActionCreatorToPass;   

	void Start ()
	{		
		Initialize();
		ActionCreator = ActionCreatorToPass;

		Type = ActorType.Enemy;

		OnBeingHit += GetFucked;
		OnActorDied += PlayerDeath;

		StartCoroutine ("WatchForDeath");
	}

	private void GetFucked(BeingAttackedEventArgs args)
	{
		Debug.Log ("Enemy OnBeingHit: Get Fucked");
	}

	private void PlayerDeath(ActorDiedEventArgs args)
	{
		Debug.Log ("ENEMY DIED");
	}
}
