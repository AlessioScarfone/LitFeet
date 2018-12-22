using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCandleBehaviour : MonoBehaviour {

	public GameObject magicFire;
	public Light mainLight;
	public GameObject fireworksContainer;

	private Vector3 _Pos;
	private bool _actived = false;
	private bool _stop = false;

	// Use this for initialization
	void Start () {
		_Pos = transform.position + new Vector3 (-10, 10, 0);
	}

	// Update is called once per frame
	void Update () {
		if (_actived && !_stop) {
			StartCoroutine (StopLerp ());

			Vector3 vectorRotation = new Vector3 (0, 0, 0);
			Quaternion fromVector = Quaternion.Euler (vectorRotation);
			transform.rotation = Quaternion.Slerp (transform.rotation, fromVector, 1.7f * Time.deltaTime);
			transform.position = Vector3.Lerp (transform.position, _Pos, 2f * Time.deltaTime);

			//get all particle but MagicFire is not active
			ParticleSystem[] particles = gameObject.GetComponentsInChildren<ParticleSystem> ();
			foreach (var p in particles) {
				p.Play ();
			}
			gameObject.GetComponentInChildren<Light> ().intensity = 15;

		}
	}

	private IEnumerator StopLerp () {
		yield return new WaitForSeconds (2.5f);
		_stop = true;
		Rigidbody r = gameObject.GetComponent<Rigidbody> ();
		if (r == null)
			r = gameObject.AddComponent<Rigidbody> ();
		// r.isKinematic = false;
		// r.useGravity = false;
		r.AddForce (new Vector3 (0, 10, 0), ForceMode.Impulse);

	
	}

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player" && !_actived) {
			_actived = true;
			Messenger.Broadcast (GameEvents.HIDE_AND_RESET_TIP);
			//block player movement
			other.GetComponent<CandleInputController> ().SetBlockMovement (true);
		}
	}

	public void LightMagicFire () {
		magicFire.SetActive (true);
		mainLight.intensity = 1;
		StartCoroutine (WaitLightFire ());
	}

	private IEnumerator WaitLightFire () {
		fireworksContainer.SetActive(true);
		yield return new WaitForSeconds (2f);
		Messenger.Broadcast (GameEvents.WIN_GAME);
	}
}