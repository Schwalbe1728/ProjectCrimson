using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIActionCreator : MonoBehaviour, ActorActionCreator {

	public bool CanPopAction { get { return false; } }

	public ActorAction PopAction
	{
		get
		{
			return null;
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
