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

	public void InitiateReloadingWeapon(float reloadTime)
	{
		if (OnReloadStarted != null) 
		{
			OnReloadStarted ();
		}

		StartCoroutine (AwaitForFinishingReload(reloadTime));
	}

	void Start ()
    {		
        Initialize();
        ActionCreator = ActionCreatorToPass;

		Type = ActorType.Player;

		OnBeingHit += GetFucked;
		OnActorDied += PlayerDeath;

		KeyBindingsWrapper.Mouse.OnMouseDown += MouseClickTest;

		StartCoroutine ("WatchForDeath");
		StartCoroutine ("CheckExperience");
	}

	void Update()
	{		

	}

	private void MouseClickTest() {
		//Debug.Log ("Pew pew muthafucka");
	}

	private void GetFucked(BeingAttackedEventArgs args)
	{
		Debug.Log ("OnBeingHit: Get Fucked");
	}

	private void PlayerDeath(ActorDiedEventArgs args)
	{
		Debug.Log ("YOU DIED");
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
