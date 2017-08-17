using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PerformedRangedAttack(PerformedRangeAttackActorEventArgs args);

public class RangedAttacksManager : MonoBehaviour {

	[SerializeField]
	GameObject projectilePrefab;

	float projectileRadius;

	public void Subscribe(ActorScript actor)
	{
		actor.OnRangedAttack += HandleRangedAttack;
	}

	// Use this for initialization
	void Start () {
		projectileRadius = projectilePrefab.GetComponent<SphereCollider> ().radius;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void HandleRangedAttack(PerformedRangeAttackActorEventArgs args)
	{
		SpawnProjectile (args);
	}

	private void SpawnProjectile(PerformedRangeAttackActorEventArgs args)
	{
		GameObject bullet = (GameObject)Instantiate
			(projectilePrefab,
				args.OriginPoint + (new Vector3(args.Direction.x, 0, args.Direction.z)).normalized * (args.SpawnOffset + projectileRadius),
			                    new Quaternion ()
		                    );

		ProjectileScript temp = bullet.GetComponent<ProjectileScript> ();

		//float tmprand = GameRandom.NextFloat (-10, 10);
		Vector3 dir = 
			Quaternion.AngleAxis( GameRandom.NextFloat(-2.5f,2.5f), Vector3.up) * 
			Quaternion.AngleAxis( GameRandom.NextFloat(-5,5), Vector3.right) * 
			args.Direction.normalized;
		
		//Debug.Log (dir);

		temp.AttackData = args.Attack;
		temp.Direction = dir;
		temp.Velocity = args.ProjectileVelocity;

		//bullet.GetComponent<Rigidbody> ().velocity = args.Direction.normalized * args.ProjectileVelocity;
	}
}
