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
	protected string[] RequiredPerkIDs;

	[SerializeField]
	protected string PerkID;

	[SerializeField]
	protected string DisplayedName;

	[SerializeField]
	protected string Description;

	[SerializeField]
	protected StatMutatorPrototype[] Mutators;

	public abstract void ApplyPerk (Player player);

	public bool RequirementsMet(Player player)
	{
		//sprawdź czy gracz ma wymagane perki
		//sprawdź czy gracz już ten perk ma

		//TODO: To jest tylko mockup

		return !(RequiredPerkIDs != null && RequiredPerkIDs.Length > 0);
	}
}
