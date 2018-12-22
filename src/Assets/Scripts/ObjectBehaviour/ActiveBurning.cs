using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveBurning : IActivableObject {
	[Tooltip ("Particle system of the fire")]
	[SerializeField] private GameObject fire;

    private GameObject player;
	private bool _objAlreadyBurning = false;
	private CandleInputController _candleInputController;
	private Animator _animator;
	private BalloonType ballonType = BalloonType.Fire;

	[Tooltip ("Pass to GameObject that is the parent of the other objects to which propate the fire.")]
	public GameObject chainOfBurningParent;

	void Start () {
		GameObject player = GameObject.FindGameObjectWithTag ("Player");
		if (player != null) {
			_candleInputController = player.GetComponent<CandleInputController> ();
			_animator = player.GetComponent<Animator> ();
		}
		needLookAt = true;
	}

	public override bool ActiveObject () {
		// Debug.Log ("TRY TO BURN");
		return Burn ();
	}

	public override bool DeactivateObject () { return true; }

	public bool Burn () {
		if (_objAlreadyBurning == false) {
			if (Managers.PlayerManager.burning == true) {
                // Managers.Audio.PlaySound(sound);
                _candleInputController.SetBlockMovement (true);
				_animator.SetTrigger ("Start Bow");
				StartCoroutine (HideBurnedObj ());
				_objAlreadyBurning = true;
				return true;
			} else {
				Debug.Log ("candle off");
				return false;
			}
		}
		return false;
	}

	private IEnumerator HideBurnedObj () {
       
		Managers.BalloonManager.HideBalloon (BalloonTypeNeeded ());
		ballonType = BalloonType.NOACTION;
		yield return new WaitForSeconds (1f);
		fire.SetActive (true);
		StartCoroutine (StartChainOfBurning ());
		yield return new WaitForSeconds (4);

		//Destroy light
		Light pointLight = fire.GetComponentInChildren<Light> ();
		if (pointLight != null)
			Destroy (pointLight);

        //Destroy Hide object
        // gameObject.GetComponent<MeshRenderer> ().enabled = false;
        // gameObject.GetComponent<BoxCollider> ().enabled = false;
        // yield return new WaitForSeconds(2);
       Destroy (gameObject);
	}

	public override BalloonType BalloonTypeNeeded () {
		return ballonType;
	}



	private IEnumerator StartChainOfBurning () {
		if (chainOfBurningParent != null) {
			yield return new WaitForSeconds (0.5f);
			ParticleSystem[] pss = chainOfBurningParent.GetComponentsInChildren<ParticleSystem> ();
			foreach (var ps in pss) {
				ps.Play ();
			}
			yield return new WaitForSeconds (3);
			Destroy (chainOfBurningParent);
		}
	}
}