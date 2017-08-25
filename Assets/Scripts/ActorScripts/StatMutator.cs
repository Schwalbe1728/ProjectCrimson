using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public enum MutatorBehaviour
    {
        ValueConstant,
        ValueDecreasing,
        ValuePerSecond
    }

    public enum MutatorDuration
    {
        Immidiate,
        TimeElapsed,
        Constant
    }

    public enum MutatorType
    {
        Flat,
        Multiplicator
    }

	[Serializable]
    public class StatMutator
    {
        private MutatorBehaviour behaviour = MutatorBehaviour.ValueConstant;
        private MutatorDuration duration = MutatorDuration.Immidiate;
        private MutatorType type = MutatorType.Flat;

        private float maxTime = 0;
        private float TimeLeft = 0;
        private float maxValue;
        private float Value;

        private static long CurrentMutatorID = 0;

        public long MutatorID { get; private set; }  

        /// <summary>
        /// Creates Immidiate mutator, invalidated after first check. If given bool value of true,
        /// creates Constant mutator, valid until removed from the bus.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="constant"></param>
        public StatMutator(float value, bool constant = false)
        {            
            Value = value;
            maxValue = value;

            duration = (constant) ? MutatorDuration.Constant : MutatorDuration.Immidiate;

            if(constant)
            {
                MutatorID = CurrentMutatorID++;
            }
        }

        /// <summary>
        /// Creates TimeElapsed Mutator. Mutator is being given ID.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        public StatMutator(float value, float time)
        {
            Value = value;
            maxValue = value;
            TimeLeft = time;
            maxTime = time;

            duration = MutatorDuration.TimeElapsed;

            MutatorID = CurrentMutatorID++;
        }

        public void SetType(MutatorType t)
        {
            type = t;
        }

        public MutatorType Type { get { return type; } }

        public void SetBehaviour(MutatorBehaviour b)
        {
            behaviour = b;
        }

        /// <summary>
        /// Returns if Mutator should be removed from MutatorBus and value of the mutator at the moment of check
        /// as an out parametre.
        /// </summary>
        /// <param name="delta"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public bool Update(float delta, out float val)
        {            
            val = (behaviour == MutatorBehaviour.ValuePerSecond) ? Value * delta : Value;

            TimeLeft -= delta;

            if (behaviour == MutatorBehaviour.ValueDecreasing)
            {
                Value = maxValue * TimeLeft / maxTime;
            }            

            return (duration == MutatorDuration.Constant)? false : (TimeLeft < 0);
        }
    }

    public class StatMutatorBus
    {
        public ActorStatsDeclaration Stat { get; private set; }

        private List<StatMutator> Mutators;        

        public StatMutatorBus(ActorStatsDeclaration stat)
        {
            Stat = stat;

            Mutators = new List<StatMutator>();
        }

        public void InsertMutator(StatMutator mutator)
        {
            Mutators.Add(mutator);
        }

        public float MutateValue(float value, float delta = 1.0f/60)
        {
            float flatBonus;
            float multiplicator;

            GetMutatorBonuses(delta, out flatBonus, out multiplicator);

            return value * multiplicator + flatBonus;
        }

        public void GetMutatorBonuses(float delta, out float flat, out float multi)
        {
            flat = 0;
            multi = 1;

            float tmpVal;

            for(int i = 0; i < Mutators.Count;)
            {
                MutatorType type = Mutators[i].Type;

                if(Mutators[i].Update(delta, out tmpVal))
                {
                    Mutators.RemoveAt(i);
                }
                else
                {
                    i++;
                }

                switch(type)
                {
                    case MutatorType.Flat:
                        flat += tmpVal;
                        break;

                    case MutatorType.Multiplicator:
                        multi *= tmpVal;
                        break;
                }
            }
        }
    }

public class StatMutatorFactory
{    
    /// <summary>
    /// Creates Immidiate mutator representing Flat bonus
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StatMutator ImmidiateMutatorFlat(float val)
    {
        return new StatMutator(val, false);
    }

    /// <summary>
    /// Creates Immidiate mutator representing Multiplicator bonus
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StatMutator ImmidiateMutatorMultiplicator(float val)
    {
        StatMutator result = new StatMutator(val, false);
        result.SetType(MutatorType.Multiplicator);

        return result;
    }

    /// <summary>
    /// Creates Constant mutator representing Flat bonus
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StatMutator ConstantMutatorFlat(float val)
    {
        return new StatMutator(val, true);
    }

    /// <summary>
    /// Creates Constant mutator representing Multiplicator bonus
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StatMutator ConstantMutatorMultiplicator(float val)
    {
        StatMutator result = new StatMutator(val, true);
        result.SetType(MutatorType.Multiplicator);

        return result;
    }

    /// <summary>
    /// Creates Constant mutator representing Flat bonus with value distributed per each second
    /// </summary>
    /// <param name="val"></param>
    /// <returns></returns>
    public static StatMutator ConstantMutatorFlatValuePerSecond(float val)
    {
        StatMutator result = new StatMutator(val, true);
        result.SetBehaviour(MutatorBehaviour.ValuePerSecond);

        return result;
    }

    /// <summary>
    /// Creates mutator that is meant to last limited amount of time, represents Flat bonus
    /// </summary>
    /// <param name="val"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static StatMutator TimeElapsedMutatorFlat(float val, float time)
    {
        return new StatMutator(val, time);
    }

    public static StatMutator TimeElapsedMutatorMultiplicator(float val, float time)
    {
        StatMutator result = new StatMutator(val, time);
        result.SetType(MutatorType.Multiplicator);

        return result;
    }

    /// <summary>
    /// Creates mutator that is meant to last limited amount of time, represents Flat bonus
    /// with it's value decreasing linearly over set time
    /// </summary>
    /// <param name="val"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static StatMutator TimeElapsedMutatorValueDecrease(float val, float time)
    {
        StatMutator result = new StatMutator(val, time);
        result.SetBehaviour(MutatorBehaviour.ValueDecreasing);

        return result;
    }

    /// <summary>
    /// Creates mutator that is meant to last limited amount of time, represents Flat bonus
    /// with it's value distributed within a second
    /// </summary>
    /// <param name="val"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static StatMutator TimeElapsedMutatorValuePerSecond(float val, float time)
    {
        StatMutator result = new StatMutator(val, time);
        result.SetBehaviour(MutatorBehaviour.ValuePerSecond);

        return result;
    }    

    /// <summary>
    /// Example: Actor is poisoned by poison that is meant to last 3 seconds and take
    /// 8 HP from it in total.
    /// </summary>
    /// <param name="val"></param>
    /// <param name="time"></param>
    /// <returns></returns>
    public static StatMutator TimeElapsedMutatorValuePerTime(float val, float time)
    {
        StatMutator result = new StatMutator(val/time, time);
        result.SetBehaviour(MutatorBehaviour.ValuePerSecond);

        return result;
    }

}

