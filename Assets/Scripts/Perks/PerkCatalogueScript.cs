using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkCatalogueScript : MonoBehaviour 
{
	[SerializeField]
	private Player player;

	[SerializeField]
	private int PerkSelectionPerLevel;

	private List<Perk> NotPickedPerks;

	public Perk[] GetPerksForLevelUp()
	{
		List<Perk> result = new List<Perk>(PerkSelectionPerLevel);

		List<Perk> copy = new List<Perk> (NotPickedPerks);

		int chosenPerks = 0;

		while (copy.Count > 0 && chosenPerks < PerkSelectionPerLevel) 
		{
			int choice = GameRandom.NextInt (copy.Count);

			if (copy [choice].RequirementsMet (player)) 
			{
				result.Add(copy [choice]);
				chosenPerks++;
			}

			copy.RemoveAt (choice);
		}

		return result.ToArray();
	}

	public void MarkPerkAsChosen(Perk perk)
	{
		if (!NotPickedPerks.Remove (perk)) {
			Debug.LogError ("MarkPerkAsChosen!!");
		}
	}

	void Start()
	{
		NotPickedPerks = new List<Perk> ();

		CreatePerkList (transform);
	}

	private void CreatePerkList(Transform transform)
	{
		foreach (Transform child in transform) 
		{
			//if (child != transform [0]) 
			{
				Perk temp = child.gameObject.GetComponent<Perk> ();

				if (temp != null) 
				{
					NotPickedPerks.Add (temp);
				}

				CreatePerkList (child);
			}
		}
	}
}
