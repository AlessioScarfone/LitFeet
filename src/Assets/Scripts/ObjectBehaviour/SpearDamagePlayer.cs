using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Collider))]
public class SpearDamagePlayer : MonoBehaviour {

	private ShotSpear shoter; 

	private void OnCollisionEnter (Collision other) {
		if (other.gameObject.tag == "Player") {
			shoter.StopShoting(true);

			Messenger.Broadcast (GameEvents.DAMAGE);
			other.gameObject.GetComponent<Animator> ().SetTrigger ("Dying");

			//destroy spear
			// Destroy (gameObject);
		}
	}

	public void setShooter(ShotSpear sp){
		shoter=sp;
	}
}