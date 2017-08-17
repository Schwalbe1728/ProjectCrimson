using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ActorEventArgs : EventArgs
{
    public ActorScript Sender { get; protected set; }
}

/// <summary>
/// Actor registered an attack - event fires before the damage and effects are dealt.
/// </summary>
public class BeingAttackedEventArgs : ActorEventArgs
{        
    public Attack TheAttack { get; private set; }

	public BeingAttackedEventArgs(Attack att, ActorScript sender)
	{
		TheAttack = att;
		Sender = sender;
	}
}

public class GotHitEventArgs : ActorEventArgs
{
    public BeingAttackedEventArgs Args { get; private set; }

    public GotHitEventArgs(BeingAttackedEventArgs args)
    {
        Args = args;
    }
}	

public class PerformedRangeAttackActorEventArgs : ActorEventArgs
{
	public Attack Attack { get; private set; }
	public ActorScript Attacker { get; private set; }
	public Vector3 OriginPoint { get; private set; }
	public Vector3 Direction { get; private set; }
	public float SpawnOffset { get; private set; }
	public float ProjectileVelocity { get; private set; }
	public float NumberOfProjectiles { get; set; }
	public float MaxDispersionAngle { get; private set; }

	public PerformedRangeAttackActorEventArgs(Attack att, ActorScript attacker, Vector3 origin, Vector3 dir, float offset, float velocity = float.MaxValue, float dispersion = 60)
	{
		Attack = att;
		Attacker = attacker;
		OriginPoint = origin;
		Direction = dir;
		ProjectileVelocity = velocity;

		SpawnOffset = offset + 0.2f;

		NumberOfProjectiles = 1;

	}
}

public class ActorDiedEventArgs : ActorEventArgs
{
    public ActorScript Killer { get; private set; }    
    public float ExpReward { get; private set; }

	public ActorDiedEventArgs(float exp, ActorScript killer, ActorScript victim)
	{
		ExpReward = exp;
		Killer = killer;
		Sender = victim;
	}
}

public class ActorGainedExperienceEventArgs : ActorEventArgs
{
	public ActorScript Rewarded { get { return Sender; } }
	public float Experience { get; private set; }

	public ActorGainedExperienceEventArgs(ActorScript rewarded, float exp)
	{
		Sender = rewarded;
		Experience = exp;
	}
}

public class ActorGainedLevelEventArgs : ActorEventArgs
{
	public ActorScript ActorPromoted { get { return Sender; } }
	public int Level { get; private set; }
	public bool IsPlayer { get { return ActorPromoted.Type == ActorType.Player; } }

	public ActorGainedLevelEventArgs(ActorScript sender, int levelGained)
	{
		Sender = sender;
		Level = levelGained;
	}

}