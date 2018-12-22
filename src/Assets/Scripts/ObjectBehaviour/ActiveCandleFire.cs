using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveCandleFire : IActivableObject {
    [Space (10)]
    [Tooltip ("Particle system of the fire")]

    [Space (10)]
    private CandleInputController _candleInputController;
    private Animator _animator;

    // Use this for initialization
    void Start () {
        _animator = GetComponent<Animator> ();
        _candleInputController = gameObject.GetComponent<CandleInputController> ();
        needLookAt = true;
    }

    public override bool ActiveObject () {
        if (!Managers.PlayerManager.burning) {
            _candleInputController.SetBlockMovement (true);
            _animator.SetTrigger ("Start Bow");
            StartCoroutine (LightFireCoroutine ());
            actived = true;
            return true;
        }
        return false;
    }

    public override bool DeactivateObject () {
        if (Managers.PlayerManager.burning) {
            TurnOffFire ();
            actived = false;
            return true;
        }
        return false;
    }

    public void TurnOffFire () {
        Managers.PlayerManager.burning = false;
        Managers.PlayerManager.HideCandleFire ();
        // Debug.Log("SPENTA!");
    }

    private IEnumerator LightFireCoroutine () {
        //WAIT ONE SECOND AND THEN LIGHT THE FIRE
        yield return new WaitForSeconds (1.2f);
        Managers.PlayerManager.burning = true;
        Managers.PlayerManager.ShowCandleFire ();
        // Debug.Log ("ACCESO!");
    }

    public override BalloonType BalloonTypeNeeded () {
        return BalloonType.Fire;
    }

}