using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ActorScript
{
    [SerializeField]
    private PlayerActionCreator ActionCreatorToPass;   

	void Start ()
    {
        Initialize();
        ActionCreator = ActionCreatorToPass;  
	}
}
