using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCombatManager : MonoBehaviour, ActorActionReceiver 
{
	public bool Reloading { get; private set; }

	[SerializeField]
	private ActorScript ActorsPosition;

	[SerializeField]
	private ActorMechanics Mechanics;

	private float attackDelay;
	private int ammoInClip;

	private bool IsPlayer;

	public void InterpretAction(ActorAction action)
	{		
		if (action is FireWeapon && attackDelay <= 0 && (!IsPlayer || ammoInClip > 0)) 
		{
			ammoInClip--;
			Debug.Log ("Fire Weapon: " + ammoInClip + " bullets in clip left");

			attackDelay += Mechanics.Combat.AttackDelay;

			FireWeapon tmp = action as FireWeapon;
			Vector3 direction = tmp.TargetPoint - ActorsPosition.transform.position;

			Attack att = 
				new Attack (
					AttackType.Projectile,
					Mechanics.Combat.Damage,
					Mechanics.Combat.DamageVariance,
					Mechanics.Combat.CriticalChance,
					Mechanics.Combat.CriticalMultiplicator,
					ActorsPosition);

			ActorsPosition.CreateRangedAttack (att, direction);
		}
	}

	public void LoadClip()
	{
		ammoInClip = 10;
		Reloading = false;
	}

	// Use this for initialization
	void Start () 
	{
		attackDelay = 0;
		LoadClip ();

		IsPlayer = ActorsPosition is Player;

		if (IsPlayer) 
		{
			(ActorsPosition as Player).OnReloadFinished += LoadClip;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (attackDelay > 0) 
		{
			attackDelay -= Time.deltaTime;
		}

		if (ammoInClip == 0 && IsPlayer && !Reloading) 
		{
			(ActorsPosition as Player).InitiateReloadingWeapon (Mechanics.Combat.ReloadSpeed);
			Reloading = true;
		}
	}
}
