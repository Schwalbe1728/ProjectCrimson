using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPerk : Perk {

	public override void ApplyPerk(Player player)
	{
		ActorMechanics mechanics = player.transform.gameObject.GetComponent<ActorMechanics> ();

		foreach (StatMutatorPrototype prototype in Mutators) 
		{
			mechanics.AddMutator (prototype.StatMutated, prototype.CreateStatMutatorFromPrototype());
		}
	}

	public override string ToString()
	{
		return DisplayedName;
	}
}
