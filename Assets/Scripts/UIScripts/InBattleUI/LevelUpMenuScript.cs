using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelUpMenuScript : MonoBehaviour {

	[SerializeField]
	private Button[] Buttons;

	[SerializeField]
	private Text DescriptionDisplayText;

	private PerkCatalogueScript PerkCatalogue;

	private Dictionary<Button, int> ChoiceHelper;
	private Dictionary<Button, Perk> DisplayHelper;

	public void DisplayPerkDescription(Button button)
	{
		if (DisplayHelper.ContainsKey (button)) 
		{
			DescriptionDisplayText.text = DisplayHelper [button].Description;
		}
		else 
		{
			DescriptionDisplayText.text = "Nigga what?";
		}
	}

	public void ChosenPerk(Button button)
	{
		GameObject.Find ("Player").GetComponent<Player> ().ActivatePerk (DisplayHelper [button]);

		gameObject.SetActive (false);
		Time.timeScale = 1;
	}

	// Use this for initialization
	void Start () 
	{
		GameObject.Find ("Player").GetComponent<Player> ().OnActorGainedLevel += SetUpMenu;

		PerkCatalogue = GameObject.Find ("Perk Catalogue").GetComponent<PerkCatalogueScript>();

		DisplayHelper = new Dictionary<Button, Perk> ();
		ChoiceHelper = new Dictionary<Button, int> ();

		for (int i = 0; i < 5; i++) 
		{
			ChoiceHelper.Add (Buttons [i], i);		
		}

		gameObject.SetActive (false);
	}

	private void SetUpMenu(ActorGainedLevelEventArgs args)
	{
		Perk[] perks = PerkCatalogue.GetPerksForLevelUp ();

		if (perks.Length > 0) 
		{
			Time.timeScale = 0f;
			gameObject.SetActive (true);

			DisplayHelper.Clear ();

			for (int i = 0; i < Buttons.Length; i++) 
			{
				if (i < perks.Length) 
				{
					DisplayHelper.Add (Buttons [i], perks [i]);
					Buttons [i].gameObject.SetActive (true);// = true;

					Buttons [i].GetComponentInChildren<Text> ().text = perks [i].Name;
				} 
				else 
				{
					Buttons [i].gameObject.SetActive(false);
				}
			}
		}
	}
}
