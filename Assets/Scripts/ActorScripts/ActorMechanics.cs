using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorMechanics : MonoBehaviour, ActorActionReceiver {

    [SerializeField]    
    private ActorStatsToFloatDictionary FloatStats;

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
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
       
	}

    private void InitFloatStats()
    {
        FloatStats = new ActorStatsToFloatDictionary();

        FloatStats.Add(ActorStatsDeclaration.Speed, 1);
    }
}
