using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpearDamagePlayer : MonoBehaviour {
	private void OnCollisionEnter(Collision other) {
		if(other.gameObject.tag == "Player"){

			Managers.PlayerManager.DamagePlayer();
			other.gameObject.GetComponent<Animator> ().SetTrigger ("Dying");

			//destroy spear
			Destroy(gameObject);
		}
	}
}
