using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void PerformedRangedAttack(PerformedRangeAttackActorEventArgs args);

public class RangedAttacksManager : MonoBehaviour {

	[SerializeField]
	GameObject projectilePrefab;

	public void Subscribe(ActorScript actor)
	{
		actor.OnRangedAttack += HandleRangedAttack;
	}

	// Use this for initialization
	void Start () {
		
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
			                    args.OriginPoint + args.Direction.normalized * args.SpawnOffset,
			                    new Quaternion ()
		                    );

		ProjectileScript temp = bullet.GetComponent<ProjectileScript> ();

		temp.AttackData = args.Attack;
		temp.Direction = args.Direction;
		temp.Velocity = args.ProjectileVelocity;

		//bullet.GetComponent<Rigidbody> ().velocity = args.Direction.normalized * args.ProjectileVelocity;
	}
}
