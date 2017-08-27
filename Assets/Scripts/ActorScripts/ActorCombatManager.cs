using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorCombatManager : MonoBehaviour, ActorActionReceiver 
{
	public bool Reloading { get; private set; }

	public int AmmoInClip { get { return ammoInClip; } }
	public int MaxAmmo { get { return (int)Weapon.ClipSize; } }

	[SerializeField]
	private ActorScript ActorsPosition;

	[SerializeField]
	private ActorMechanics Mechanics;

	[SerializeField]
	private WeaponStats Weapon;

	private float attackDelay;
	private int ammoInClip;

	private bool IsPlayer;

	private float audioSourceVolume;
	private AudioSource audioSource;

	public void InterpretAction(ActorAction action)
	{		
		if (action is FireWeapon && attackDelay <= 0 && (!IsPlayer || ammoInClip > 0)) 
		{
			ammoInClip--;

			attackDelay += Mechanics.Combat.AttackDelay;

			FireWeapon tmp = action as FireWeapon;
			Vector3 direction = tmp.TargetPoint - ActorsPosition.transform.position;

			Attack att = 
				new Attack (
					Weapon.ProjectileType,
					Mechanics.Combat.Damage,
					Mechanics.Combat.DamageVariance,
					Mechanics.Combat.CriticalChance,
					Mechanics.Combat.CriticalMultiplicator,
					ActorsPosition);

			ActorsPosition.CreateRangedAttack (att, direction);

			audioSource.volume = audioSourceVolume * GameRandom.NextFloat (0.9f, 1);
			audioSource.pitch = GameRandom.NextFloat (0.8f, 1.2f);
			audioSource.Play ();
		}

		if (action is MeleeAttack && attackDelay <= 0) 
		{
			attackDelay += (action as MeleeAttack).WindUpTime + Mechanics.Combat.AttackDelay;

			StartCoroutine ("MeleeAttackWithWindUp", action as MeleeAttack);
		}

		if (action is ReloadWeapon) 
		{
			StartCoroutine ("StartReloadingAfterDelay");
		}
	}

	public void LoadClip()
	{
		ammoInClip = MaxAmmo;
		Reloading = false;
	}

	public void GiveWeapon(WeaponStats weaponStats)
	{
		Weapon = weaponStats;
		Mechanics.Combat.ApplyStatsForNewWeapon (weaponStats);
		audioSource.clip = weaponStats.AttackClip;
	}

	// Use this for initialization
	void Start () 
	{
		audioSource = GetComponent<AudioSource> ();
		audioSourceVolume = audioSource.volume;

		attackDelay = 0;
		LoadClip ();

		IsPlayer = ActorsPosition is Player;

		if (IsPlayer) 
		{
			(ActorsPosition as Player).OnReloadFinished += LoadClip;
		}

		GiveWeapon (Weapon);
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

	private IEnumerator MeleeAttackWithWindUp(MeleeAttack att)
	{
		yield return new WaitForSeconds (att.WindUpTime);

		Attack attack = new Attack (
			                AttackType.Melee, 
			                Mechanics.Combat.Damage,
			                Mechanics.Combat.DamageVariance,
			                Mechanics.Combat.CriticalChance,
			                Mechanics.Combat.CriticalMultiplicator,
			                ActorsPosition);

		ActorsPosition.CreateMeleeAttack (attack, att.TargetTag);
	}

	private IEnumerator StartReloadingAfterDelay()
	{
		if (IsPlayer && AmmoInClip < MaxAmmo) 
		{
			if (attackDelay <= 0)
				attackDelay = 0.1f;

			yield return new WaitForSeconds (attackDelay);

			attackDelay += Mechanics.Combat.ReloadSpeed;
			(ActorsPosition as Player).InitiateReloadingWeapon (Mechanics.Combat.ReloadSpeed);
		}
	}
}
