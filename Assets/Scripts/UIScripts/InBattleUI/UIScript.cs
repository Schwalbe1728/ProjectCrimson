using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour {

	[SerializeField]
	private Player player;

	[SerializeField]
	private ActorCombatManager CombatManager;

	[SerializeField]
	private ActorMechanics Mechanics;

	[SerializeField]
	private ActorExperienceManager ExpManager;

	[SerializeField]
	private float SplashFadeInDuration;

	[SerializeField]
	private float SplashFadeOutDuration;

	[SerializeField]
	private float SplashStayDuration;

	private Image[] ClipRepresentation;
	private Text ClipStatusText;
	private Text HealthPointsText;
	private Text LevelText;

	private Slider ExpSlider;
	private Slider HealthSlider;

	private int numberOfBulletReps = 30;

	private Image LevelUpSplash;

	// Use this for initialization
	void Start () 
	{
		ClipRepresentation = new Image[numberOfBulletReps];

		for (int i = 0; i < numberOfBulletReps; i++) 
		{
			ClipRepresentation [i] = GameObject.Find ("Bullet" + (i + 1).ToString ()).GetComponent<Image>();
		}

		ClipStatusText = GameObject.Find ("Clip Status Text").GetComponent<Text> ();
		ExpSlider = GameObject.Find ("EXP Slider").GetComponent<Slider> ();
		HealthSlider = GameObject.Find ("Health Slider").GetComponent<Slider> ();
		HealthPointsText = GameObject.Find ("HP Details").GetComponent<Text> ();
		LevelUpSplash = GameObject.Find ("Level Up Image").GetComponent<Image> ();
		LevelText = GameObject.Find ("Level Text").GetComponent<Text> ();

		LevelText.text = "LVL: 0";
		ClipReloaded ();

		player.OnReloadFinished += ClipReloaded;
		player.OnRangedAttack += FireWeapon;

		player.OnActorGainedExperience += ExperienceGained;
		player.OnActorGainedLevel += LevelGained;
	}

	void Update()
	{
		float currHP = Mechanics.Health.CurrentHealth;
		float maxHP = Mechanics.Health.MaxHealth;

		HealthSlider.value = currHP / maxHP;

		HealthPointsText.text = Mechanics.Health.CurrentHealthDisplayed + "/" + Mechanics.Health.MaxHealth;
	}

	private void ExperienceGained(ActorGainedExperienceEventArgs args)
	{
		ExpSlider.value = ExpManager.PercentProgressBetweenLevels (player.Experience, player.Level);
	}

	private void LevelGained(ActorGainedLevelEventArgs args)
	{
		ExpSlider.value = ExpManager.PercentProgressBetweenLevels (player.Experience, player.Level);
		LevelText.text = "LVL: " + player.Level;

		StartCoroutine ("DisplayLevelUp");
	}

	private void ClipReloaded()
	{
		for (int i = 0; i < CombatManager.MaxAmmo && i < numberOfBulletReps; i++) 
		{
			ClipRepresentation [i].enabled = (i < CombatManager.MaxAmmo);
		}

		if (CombatManager.MaxAmmo > numberOfBulletReps) 
		{
			ClipStatusText.text = "+ " + (CombatManager.MaxAmmo - numberOfBulletReps).ToString ();
		}
	}

	private void FireWeapon(PerformedRangeAttackActorEventArgs args)
	{
		ClipStatusText.text = 
			(CombatManager.AmmoInClip > numberOfBulletReps) ?
			"+ " + (CombatManager.AmmoInClip - numberOfBulletReps).ToString () : "";

		if (CombatManager.AmmoInClip < numberOfBulletReps) 
		{
			ClipRepresentation [CombatManager.AmmoInClip].enabled = false;
		}
	}

	private IEnumerator DisplayLevelUp()
	{
		while (LevelUpSplash.color.a < 1) 
		{
			LevelUpSplash.color = new Color(1, 1, 1, LevelUpSplash.color.a + Time.deltaTime / SplashFadeInDuration);
			yield return new WaitForSeconds (Time.deltaTime);
		}

		yield return new WaitForSeconds (SplashStayDuration);

		while (LevelUpSplash.color.a > 0) 
		{
			LevelUpSplash.color = new Color(1, 1, 1, Mathf.Max(0, LevelUpSplash.color.a - Time.deltaTime / SplashFadeOutDuration));		

			yield return new WaitForSeconds (Time.deltaTime);
		}
	}
}
