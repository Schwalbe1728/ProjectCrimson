using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelUpMenuButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
	[SerializeField]
	private LevelUpMenuScript menu;
	private Button button;

	public void OnPointerClick(PointerEventData ev)
	{
		menu.ChosenPerk (button);
	}

	public void OnPointerEnter(PointerEventData ev)
	{
		menu.DisplayPerkDescription (button);
	}

	// Use this for initialization
	void Start () 
	{
		button = gameObject.GetComponent<Button> ();
	}
}
