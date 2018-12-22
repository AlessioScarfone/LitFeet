using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveOnDestroyFountain : MonoBehaviour {
	public GameObject waitDestructionOf;

	public GameObject magicFire;
	public GameObject gutterWater;

	private bool actived = false;

	// Update is called once per frame
	void Update () {
		if (waitDestructionOf == null && !actived)
			Active ();
	}

	private void Active () {
		actived = true;
		ParticleSystem particleSystem1 = gameObject.GetComponent<ParticleSystem> ();
		if (particleSystem1 != null) {
			particleSystem1.Play ();
			StartCoroutine (StopMagicFire ());
		}

	}

	private IEnumerator StopMagicFire () {
		Destroy (gutterWater);

		yield return new WaitForSeconds (1f);
		// Destroy(magicFire.gameObject);
		ParticleSystem.MainModule magicFireMainModule = magicFire.GetComponentInChildren<ParticleSystem> ().main;
		magicFireMainModule.loop = false;
		magicFire.GetComponent<BoxCollider> ().enabled = false;

		yield return new WaitForSeconds (4f);
		Destroy (magicFire);
		yield return new WaitForSeconds (4f);
		Destroy(gameObject);
	}
}