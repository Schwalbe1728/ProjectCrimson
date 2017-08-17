using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ActorGainedLevel(ActorGainedLevelEventArgs args);
public delegate void ActorGainedExperience(ActorGainedExperienceEventArgs args);

public class ActorExperienceManager : MonoBehaviour 
{	
	[SerializeField]
	private int[] PlayerNextLevelEXPRequirement;
	[SerializeField]
	private int[] EnemyNextLevelEXPRequirement;

	void Start()
	{
		if (PlayerNextLevelEXPRequirement == null) 
		{
			PlayerNextLevelEXPRequirement = new int[]{ 0 };
		}

		if (EnemyNextLevelEXPRequirement == null) 
		{
			EnemyNextLevelEXPRequirement = new int[]{ 0 };
		}
	}

	public void Subscribe(ActorScript actor)
	{
		actor.OnActorGainedExperience += CheckIfNewLevelReached;
	}

	public float PercentProgressBetweenLevels(float currentExp, int currentLvl)
	{
		if (currentLvl == PlayerNextLevelEXPRequirement.Length)
			return 0;
		
		int prevLvlExp = (currentLvl == 0) ? 0 : PlayerNextLevelEXPRequirement [currentLvl-1];
		int expNeeded = PlayerNextLevelEXPRequirement [currentLvl] - prevLvlExp;

		return (currentExp - prevLvlExp) / (expNeeded);


	}

	private void CheckIfNewLevelReached(ActorGainedExperienceEventArgs args)
	{
		int[] tempExpTable = 
			(args.Rewarded.Type == ActorType.Player) ?
				PlayerNextLevelEXPRequirement :
				EnemyNextLevelEXPRequirement;

		float exp = args.Rewarded.Experience;
		int lvl = args.Rewarded.Level;

		while (lvl < tempExpTable.Length && exp >= tempExpTable [lvl]) 
		{
			ActorGainedLevelEventArgs tempArgs = new ActorGainedLevelEventArgs (args.Rewarded, ++lvl);

			args.Rewarded.GainLevel (tempArgs);
		}
	}
}
