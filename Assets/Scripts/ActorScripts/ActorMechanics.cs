using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMechanics : MonoBehaviour, ActorActionReceiver {

    [SerializeField]    
    private ActorStatsToFloatDictionary FloatStats;

    private Dictionary<ActorStatsDeclaration, StatMutatorBus> StatMutators;
    //private Dictionary<ActorStatsDeclaration, float> FloatStatsForThisUpdate;
    [SerializeField]
    private ActorStatsToFloatDictionary FloatStatsForThisUpdate;

    public MovementStats Movement { get; private set; }
    public HealthStats Health { get; private set; }
    public CombatStats Combat { get; private set; }

    public void InterpretAction(ActorAction action)
    {
        if(action.Mutators != null)
        {
            foreach(ActorStatsDeclaration stat in action.Mutators.Keys)
            {
                if(!StatMutators.ContainsKey(stat))
                {
                    StatMutators.Add(stat, new StatMutatorBus(stat));
                }

                foreach(StatMutator mutator in action.Mutators[stat])
                {
                    StatMutators[stat].InsertMutator(mutator);
                }
            }
        }
    }

    public float GetFloatStatValue(ActorStatsDeclaration stat)
    {
        return (FloatStats.ContainsKey(stat)) ? FloatStats[stat] : 0;
    }

	// Use this for initialization
	void Start ()
    {
        if(FloatStats == null)
        {
            InitFloatStats();
        }

        StatMutators = new Dictionary<ActorStatsDeclaration, StatMutatorBus>();
        FloatStatsForThisUpdate = new ActorStatsToFloatDictionary();  //new Dictionary<ActorStatsDeclaration, float>();

        Movement = new MovementStats(StatMutators, FloatStatsForThisUpdate);
        Health = new HealthStats(StatMutators, FloatStatsForThisUpdate);
        Combat = new CombatStats(StatMutators, FloatStatsForThisUpdate);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        float delta = 1.0f / 60;

        UpdateFloatStats(delta);
    }

    private void InitFloatStats()
    {
        FloatStats = new ActorStatsToFloatDictionary();

        FloatStats.Add(ActorStatsDeclaration.Speed, 1);        
    }

    private void UpdateFloatStats(float delta)
    {
        FloatStatsForThisUpdate.Clear();

        foreach(KeyValuePair<ActorStatsDeclaration, float> stat in FloatStats)
        {            
            FloatStatsForThisUpdate.Add(
                stat.Key, 
                (StatMutators.ContainsKey(stat.Key))?
                    StatMutators[stat.Key].MutateValue(stat.Value, delta) :
                    stat.Value
                );
        }

        foreach(KeyValuePair<ActorStatsDeclaration, StatMutatorBus> statMutation in StatMutators)
        {
            if(!FloatStatsForThisUpdate.ContainsKey(statMutation.Key))
            {
                FloatStatsForThisUpdate.Add(statMutation.Key, statMutation.Value.MutateValue(0));
            }
        }

        FloatStats[ActorStatsDeclaration.CurrentHealthPoints] =            
            Mathf.Clamp(Health.CurrentHealth + Health.Regeneration * delta, Health.CurrentHealth, Health.MaxHealth);
    }
}

public class MovementStats
{    
    private Dictionary<ActorStatsDeclaration, StatMutatorBus> statMutators;
    private IDictionary<ActorStatsDeclaration, float> finalStats;

    public MovementStats(Dictionary<ActorStatsDeclaration, StatMutatorBus> mutators, IDictionary<ActorStatsDeclaration, float> statsAfterModif)
    {
        statMutators = mutators;
        finalStats = statsAfterModif;
    }

    public void AddMutator(ActorStatsDeclaration stat, StatMutator mut)
    {
        if(!statMutators.ContainsKey(stat))
        {
            statMutators.Add(stat, new StatMutatorBus(stat));
        }

        statMutators[stat].InsertMutator(mut);
    }

    public float Speed { get {
            return (finalStats.ContainsKey(ActorStatsDeclaration.Speed)) ?
                     finalStats[ActorStatsDeclaration.Speed] : 0;
        } }

    public float SprintSpeed
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.SprintSpeed)) ?
                     finalStats[ActorStatsDeclaration.SprintSpeed] : 0;
        }
    }

    public float JumpSpeed
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.JumpSpeed)) ?
                     finalStats[ActorStatsDeclaration.JumpSpeed] : 0;
        }
    }

    public float JumpsAvailable
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.JumpsAllowed)) ?
                     finalStats[ActorStatsDeclaration.JumpsAllowed] : 0;
        }        
    }
}

public class HealthStats
{
    private Dictionary<ActorStatsDeclaration, StatMutatorBus> statMutators;
    private IDictionary<ActorStatsDeclaration, float> finalStats;

    public HealthStats(Dictionary<ActorStatsDeclaration, StatMutatorBus> mutators, IDictionary<ActorStatsDeclaration, float> statsAfterModif)
    {
        statMutators = mutators;
        finalStats = statsAfterModif;
    }

    public void AddMutator(ActorStatsDeclaration stat, StatMutator mut)
    {
        if (!statMutators.ContainsKey(stat))
        {
            statMutators.Add(stat, new StatMutatorBus(stat));
        }
        
        statMutators[stat].InsertMutator(mut);
    }

    public void ApplyDamage(float dmg)
    {
        dmg = DamageTakenModificator * dmg;

        AddMutator(ActorStatsDeclaration.CurrentHealthPoints, StatMutatorFactory.ImmidiateMutatorFlat(-dmg));
    }

    public void ApplyImmidiateHeal(float heal)
    {
        AddMutator(ActorStatsDeclaration.CurrentHealthPoints, StatMutatorFactory.ImmidiateMutatorFlat(heal));
    }

    public void ApplyHealOverTime(float heal, float time)
    {
        AddMutator(ActorStatsDeclaration.CurrentHealthPoints, StatMutatorFactory.TimeElapsedMutatorValuePerTime(heal, time));
    }

    public bool IsAlive { get { return CurrentHealthDisplayed > 0; } }

    public int CurrentHealthDisplayed { get { return Mathf.RoundToInt(CurrentHealth); } }

    public float CurrentHealth
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.CurrentHealthPoints)) ?
                     finalStats[ActorStatsDeclaration.CurrentHealthPoints] : 0;
        }
    }

    public float MaxHealth
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.MaxHealthPoints)) ?
                     finalStats[ActorStatsDeclaration.MaxHealthPoints] : 0;
        }
    }

    public float Regeneration
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.HealthPointsRegeneration)) ?
                     finalStats[ActorStatsDeclaration.HealthPointsRegeneration] : 0;
        }
    }    

    public float DamageTakenModificator
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.DamageTakenModificator)) ?
                finalStats[ActorStatsDeclaration.DamageTakenModificator] : 1;
        }
    }
}

public class CombatStats
{
    private Dictionary<ActorStatsDeclaration, StatMutatorBus> statMutators;
    private IDictionary<ActorStatsDeclaration, float> finalStats;

    public CombatStats(Dictionary<ActorStatsDeclaration, StatMutatorBus> mutators, IDictionary<ActorStatsDeclaration, float> statsAfterModif)
    {
        statMutators = mutators;
        finalStats = statsAfterModif;
    }

    public void AddMutator(ActorStatsDeclaration stat, StatMutator mut)
    {
        if (!statMutators.ContainsKey(stat))
        {
            statMutators.Add(stat, new StatMutatorBus(stat));
        }

        statMutators[stat].InsertMutator(mut);
    }
    
    public float Damage
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.Damage)) ?
                     finalStats[ActorStatsDeclaration.Damage] : 0;
        }
    }

    public float DamageVariance
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.DamageVariance)) ?
                     finalStats[ActorStatsDeclaration.DamageVariance] : 0;
        }
    }

    public float CriticalChance
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.CriticalChance)) ?
                     finalStats[ActorStatsDeclaration.CriticalChance] : 0.01f;
        }
    }

    public float CriticalMultiplicator
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.CriticalMultiplicator)) ?
                     finalStats[ActorStatsDeclaration.CriticalMultiplicator] : 2f;
        }
    }

    public float ReloadSpeed
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.ReloadSpeed)) ?
                     finalStats[ActorStatsDeclaration.ReloadSpeed] : 0;
        }
    }

    public float FireRate
    {
        get
        {
            return (finalStats.ContainsKey(ActorStatsDeclaration.FireRate)) ?
                     finalStats[ActorStatsDeclaration.FireRate] : 90;
        }
    }

    public float AttackDelay
    {
        get { return 60.0f / FireRate; }
    }

}