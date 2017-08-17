using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour {

	public AttackType ProjectileType;

	public float Damage { get { return damage; } }
	public float DamageVariance { get { return damageVariance; } }

	public float ClipSize { get { return clipSize; } }
	public float RoundsPerMinute { get { return roundsPerMinute; } }
	public float ReloadSpeed { get { return reloadSpeed; } }
	public int ProjectilesPerShot { get { return projectilesPerShot; } }


	public float CriticalChance { get { return critChance; } }
	public float CriticalMulti { get { return critMultiplier; } }

	public float ProjectileVelocity { get { return projectileVelocity; } }

	public AudioClip AttackClip { get { return attackClip; } }

	[SerializeField]
	protected float damage;
	[SerializeField]
	protected float damageVariance;
	[SerializeField]
	protected float clipSize;
	[SerializeField]
	protected float roundsPerMinute;
	[SerializeField]
	protected int projectilesPerShot;
	[SerializeField]
	protected float reloadSpeed;
	[SerializeField]
	protected float critChance;
	[SerializeField]
	protected float critMultiplier;
	[SerializeField]
	protected float projectileVelocity;
	[SerializeField]
	protected AudioClip attackClip;
	private List<KeyValuePair<ActorStatsDeclaration, StatMutator>> InflictedStatChangesForEachShot;
	/*
	public Attack CreateAttack(ActorScript attacker)
	{
		Attack result = 
			new Attack (
				ProjectileType,
				Damage,
				DamageVariance,
				CriticalChance,
				CriticalMulti,
				attacker,
				InflictedStatChangesForEachShot);

		return result;
	}
	*/

}
