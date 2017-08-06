using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void BeingHit(BeingAttackedEventArgs args);
public delegate void ActorDied(ActorDiedEventArgs args);

public class ActorScript : MonoBehaviour {

	public event BeingHit OnBeingHit;
	public event ActorDied OnActorDied;
	public event PerformedRangedAttack OnRangedAttack;

	public event ActorGainedLevel OnActorGainedLevel;
	public event ActorGainedExperience OnActorGainedExperience;

    [SerializeField]
    protected ActorMovement Movement;

    [SerializeField]
    protected ActorMechanics Mechanics;

	[SerializeField]
	protected ActorCombatManager CombatManager;

    protected ActorActionCreator ActionCreator;

    protected List<ActorActionReceiver> actionReceivers;

	protected bool Alive = true;
	protected bool ShouldBeKilled = false;

	protected ActorScript LastAttacker;

	public ActorType Type { get; protected set; }

	public ActorExperienceManager ExperienceManager;
	public RangedAttacksManager RangedAttManager;

	[SerializeField]
	protected float ExpReward;

	private float projectileSpawnOffset;

    public void Initialize()
    {
        actionReceivers = new List<ActorActionReceiver>();
        actionReceivers.Add(Movement);
        actionReceivers.Add(Mechanics);
		actionReceivers.Add (CombatManager);

		OnBeingHit += RegisterBeingAttacked;
		OnActorDied += Kill;

		ExperienceManager.Subscribe (this);
		RangedAttManager.Subscribe (this);

		CapsuleCollider temp = GetComponent<CapsuleCollider> ();
		projectileSpawnOffset = temp.radius;
    }

	public float Experience { get { return Mechanics.Experience.Experience; } }
	public int Level { get { return Mechanics.Experience.Level; } }

	public void GainExperience(ActorGainedExperienceEventArgs args)
	{
		Mechanics.Experience.GainExperience (args.Experience);

		if(OnActorGainedExperience != null)
			OnActorGainedExperience (args);
	}

	public void GainLevel(ActorGainedLevelEventArgs args)
	{
		Mechanics.Experience.GainLevel ();

		Debug.Log ("Actor has gained Level " + Mechanics.Experience.Level);

		if(OnActorGainedLevel != null)
			OnActorGainedLevel (args);
	}

	public void CreateRangedAttack(Attack att, Vector3 direction)
	{
		if (att.Type == AttackType.Projectile) 
		{
			PerformedRangeAttackActorEventArgs args = 
				new PerformedRangeAttackActorEventArgs (att, this, this.transform.position, direction, projectileSpawnOffset, 25f);

			if (OnRangedAttack != null) 
			{
				OnRangedAttack (args);
			}
		}
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
		/*
		if (ShouldBeKilled && Alive) 
		{
			ActorDiedEventArgs arg = new ActorDiedEventArgs (ExpReward, LastAttacker, this);
			OnActorDied (arg);
		}
		*/
		if (!Alive) 
		{			
			return;
		}

        while (ActionCreator.CanPopAction)
        {
            ActorAction action = ActionCreator.PopAction;

            foreach (ActorActionReceiver receiver in actionReceivers)
            {
                receiver.InterpretAction(action);
            }
        }
        
		/*
        if(Input.GetKeyDown(KeyCode.P))
        {
			BeingAttackedEventArgs arg = new BeingAttackedEventArgs (new Attack (AttackType.Melee, 2, 0.5f, 0.05f, 2), this);
			OnBeingHit(arg);
        }
		*/
    }

	void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag.Equals ("HostileProjectile")) 
		{
			Debug.Log ("Ouch! Hit by a projectile" + this.tag);

			Attack att = collision.gameObject.GetComponent<ProjectileScript> ().AttackData;

			BeingAttackedEventArgs args = new BeingAttackedEventArgs (att, this);

			OnBeingHit (args);
		}
	}

	public void RegisterBeingAttacked(BeingAttackedEventArgs args)
    {
		List<KeyValuePair<ActorStatsDeclaration, StatMutator>> temp = args.TheAttack.GetStatChanges();

        foreach(KeyValuePair<ActorStatsDeclaration, StatMutator> pair in temp)
        {
            Mechanics.AddMutator(pair.Key, pair.Value);
        }
			
		Mechanics.Health.ApplyDamage(args.TheAttack.CalculateDamage ());

		LastAttacker = args.TheAttack.Attacker;

        // Animator dostaje informacje że postać dostała trafiona
        // Rozpatrz przypadek gdy postać oberwała krytyka
    }   		

	protected IEnumerator WatchForDeath()
	{
		while (Mechanics == null ||
			Mechanics.Health == null ||
			Mechanics.Health.IsAlive) 
		{
			yield return null;
		}

		ActorDiedEventArgs arg = new ActorDiedEventArgs (ExpReward, LastAttacker, this);
		OnActorDied(arg);
	}

	protected void Kill(ActorDiedEventArgs args)
	{
		Alive = false;

		if (args.Killer != null) 
		{
			ActorGainedExperienceEventArgs tempArgs = new ActorGainedExperienceEventArgs (args.Killer, this.ExpReward);
			args.Killer.GainExperience (tempArgs);
		}
	}
}

public enum ActorType
{
	Player,
	Enemy
}
