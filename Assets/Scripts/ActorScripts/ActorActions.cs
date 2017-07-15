using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActorActionReceiver
{
    void InterpretAction(ActorAction action);
}

public abstract class ActorAction
{
    public bool Interpreted { get; private set; }

    public float BaseTimeDelay { get; protected set; }

    public Dictionary<ActorStatsDeclaration, List<StatMutator>> Mutators;

    public void Interpret() { Interpreted = true; }
}

public abstract class MovementAction : ActorAction
{
    public Vector3 MovementVector { get; protected set; }

    public MovementAction(Vector3 movement)
    {
        MovementVector = movement;
    }
}

public class Move : MovementAction
{
    public bool Sprint { get; private set; }

    public Move(Vector3 mov, bool sprint = false) : base(mov)
    {
        Sprint = sprint;
    }

    public Move(float x, float y, float z, bool sprint = false) : base(new Vector3(x,y,z))
    {
        Sprint = sprint;
    }
}

public class Jump : MovementAction
{
    public Jump(Vector3 movement) : base(movement)
    {
    }
}

public class DodgeRoll : MovementAction
{
    public DodgeRoll(Vector3 movement) : base(movement)
    {
        BaseTimeDelay = 0.65f;
        
        Mutators = new Dictionary<ActorStatsDeclaration, List<StatMutator>>();

        Mutators.Add(ActorStatsDeclaration.DamageTakenModificator, new List<StatMutator>());
        Mutators.Add(ActorStatsDeclaration.Speed, new List<StatMutator>());        

        Mutators[ActorStatsDeclaration.DamageTakenModificator].
            Add( StatMutatorFactory.TimeElapsedMutatorMultiplicator(0, 0.5f) );

        Mutators[ActorStatsDeclaration.Speed].
            Add(StatMutatorFactory.TimeElapsedMutatorMultiplicator(2.9f, 0.6f));
    }
}