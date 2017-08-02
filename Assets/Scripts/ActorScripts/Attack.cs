using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
	public AttackType Type { get; private set; }

    public float Damage { get; private set; }
    public float DamageVariance { get; private set; }
    public float CritChance { get; private set; }
    public float CritModificator { get; private set; }

    public bool? WasCritical { get; private set; }

    public ActorScript Attacker { get; private set; }

    private List<KeyValuePair<ActorStatsDeclaration, StatMutator>> InflictedStatChanges;

    private bool DamageAlreadyCalculated = false;
    private float finalDamage;


    public Attack(AttackType type, float dmg, float variance, float critChance, float critMultiplier, ActorScript attacker = null, List<KeyValuePair<ActorStatsDeclaration, StatMutator>> inflicted = null)
    {
        Type = type;
        Damage = dmg;
        DamageVariance = variance;
        CritChance = critChance;
        CritModificator = critMultiplier;

        Attacker = attacker;

        InflictedStatChanges = 
            (inflicted == null) ? 
                new List<KeyValuePair<ActorStatsDeclaration, StatMutator>>() : 
                inflicted;

        WasCritical = null;
    }

    public float CalculateDamage()
    {        
        if (!DamageAlreadyCalculated)
        {
            WasCritical = new bool();
            WasCritical = GameRandom.NextFloat() < CritChance;

            finalDamage = Damage *
                      GameRandom.NextFloat(1f - DamageVariance, 1f + DamageVariance) *
                      ((WasCritical.Value) ? CritModificator : 1);            

            DamageAlreadyCalculated = true;
        }

        return finalDamage;
    }

    public List<KeyValuePair<ActorStatsDeclaration, StatMutator>> GetStatChanges()
    {       
        return InflictedStatChanges;
    }
}

public enum AttackType
{
    Melee,
    Projectile,
    AreaOfEffect
}
