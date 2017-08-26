using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPerk
{
	void ApplyPerk(Player player);
	bool RequirementsMet (Player player);
}

public abstract class Perk :  MonoBehaviour, IPerk
{
	[SerializeField]
	protected Perk[] RequiredPerks;

	[SerializeField]
	protected string PerkID;

	[SerializeField]
	protected string DisplayedName;

	[SerializeField]
	protected string DisplayedDescription;

	[SerializeField]
	protected StatMutatorPrototype[] Mutators;

	public string ID {get {return PerkID;}}
	public string Name { get { return DisplayedName; } }
	public string Description { get { return DisplayedDescription; } }

	public abstract void ApplyPerk (Player player);

	public bool RequirementsMet(Player player)
	{
		bool result = !player.PerkActivated (this);

		if(RequiredPerks != null)
		{
			for (int i = 0; i < RequiredPerks.Length && result; i++) 
			{
				result = result && player.PerkActivated (RequiredPerks [i]);
			}
		}			

		return result;
	}
}
