using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticleBehaviour : MonoBehaviour {

	public void OnParticleCollision (GameObject other) {
		if (other.tag == "CandleFire") {
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.GetComponent<ActiveCandleFire> ().DeactivateObject ();
		}
	}

}