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

    public void InterpretAction(ActorAction action)
    {

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
}