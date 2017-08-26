using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void ReloadStarted();
public delegate void ReloadFinished();

public class Player : ActorScript
{
	public event ReloadStarted OnReloadStarted;
	public event ReloadFinished OnReloadFinished;

    [SerializeField]
    private PlayerActionCreator ActionCreatorToPass;   

	private PerkCatalogueScript PerksCatalogue;
	private List<Perk> PerksActive;

	public void InitiateReloadingWeapon(float reloadTime)
	{
		if (OnReloadStarted != null) 
		{
			OnReloadStarted ();
		}

		StartCoroutine (AwaitForFinishingReload(reloadTime));
	}

	public bool PerkActivated(Perk perk)
	{
		return PerksActive.Find (p => p.ID.Equals (perk.ID)) != null;
	}

	public void ActivatePerk(Perk perk)
	{
		PerksActive.Add (perk);
		PerksCatalogue.MarkPerkAsChosen (perk);

		perk.ApplyPerk (this);
	}

	void Start ()
    {	
		PerksActive = new List<Perk> ();
		PerksCatalogue = GameObject.Find ("Perk Catalogue").GetComponent<PerkCatalogueScript>();

        Initialize();
        ActionCreator = ActionCreatorToPass;

		Type = ActorType.Player;

		OnBeingHit += GetFucked;
		OnActorDied += PlayerDeath;

		OnActorGainedLevel += Player_OnActorGainedLevel;

		KeyBindingsWrapper.Mouse.OnMouseDown += MouseClickTest;

		StartCoroutine ("WatchForDeath");
		StartCoroutine ("CheckExperience");
	}

	void Update()
	{		

	}

	void Player_OnActorGainedLevel (ActorGainedLevelEventArgs args)
	{
		/*
		Perk[] perks = PerksCatalogue.GetPerksForLevelUp ();

		if (perks.Length > 0) 
		{
			int choice = GameRandom.NextInt (perks.Length);

			PerksActive.Add (perks [choice]);
			PerksCatalogue.MarkPerkAsChosen (perks [choice]);

			perks [choice].ApplyPerk (this);

			Debug.Log ("Spośród " + perks.Length + " wybrano perk " + perks [choice] + ". Aktywne perki: " + PerksActive.Count);
		}
		*/

		//GameObject.Find ("Level Up Menu").GetComponent<LevelUpMenuScript> ().SetUpMenu ();
	}

	private void MouseClickTest() {
		//Debug.Log ("Pew pew muthafucka");
	}

	private void GetFucked(BeingAttackedEventArgs args)
	{
		//Debug.Log ("OnBeingHit: Get Fucked");
	}

	private void PlayerDeath(ActorDiedEventArgs args)
	{
		//Debug.Log ("YOU DIED");
		StopCoroutine ("CheckExperience");
	}

	private IEnumerator AwaitForFinishingReload(float reloadTime)
	{
		/*while (reloadTime > 0) 
		{
			reloadTime -= Time.deltaTime;
			yield return null;
		}
		*/
		yield return new WaitForSeconds (reloadTime);

		if (OnReloadFinished != null) 
		{
			OnReloadFinished ();
		}
	}

	// Actually a test function to check if events considering exp gain work as intended
	// To delete
	private IEnumerator CheckExperience()
	{
		while (true) 
		{
			if (Input.GetKeyDown (KeyCode.P)) {
				Debug.Log ("Player EXP: " + Mechanics.Experience.Experience + ", Level: " + Mechanics.Experience.Level);
			}

			if (Input.GetKeyDown (KeyCode.I)) {
				base.GainExperience(new ActorGainedExperienceEventArgs(this, 10));
			}

			yield return null;
		}
	}
}
