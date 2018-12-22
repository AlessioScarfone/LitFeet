using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
	[Tooltip("If is true, it kill player with one shot")]
	public bool instantKill = true;

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			if (instantKill) {
				Managers.PlayerManager.SetLife (1);
				other.gameObject.GetComponent<Animator> ().SetTrigger ("Dying");
				Managers.PlayerManager.DamagePlayer ();
				//UI update of the hearth is done by GameOver method
			} else {
				Messenger.Broadcast (GameEvents.DAMAGE);
				other.gameObject.GetComponent<Animator> ().SetTrigger ("Dying");
				gameObject.GetComponent<BoxCollider>().enabled = false;
				//avoid multiple it durint Dying animation
			}

		}
	}

}