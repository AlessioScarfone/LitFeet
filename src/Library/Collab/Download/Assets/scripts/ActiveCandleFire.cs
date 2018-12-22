using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCandleFire : IActivableObject {

	private PlayerState playerState;
    public override void ActiveObject()
    {
        if (!playerState.burning){
            playerState.LightFire();
        }
    }

    public override void DisableObject()
    {
        if (playerState.burning){
            playerState.TurnOffFire();
        }
    }

    // Use this for initialization
    void Start () {
		playerState = gameObject.GetComponent<PlayerState>();
	}
	
}
