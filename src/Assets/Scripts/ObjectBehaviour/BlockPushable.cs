using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPushable : MonoBehaviour {

	[SerializeField] private string sound;

	private void OnTriggerEnter(Collider other) {
		if(other.tag =="Pushable"){
			other.tag = "Untagged";
			Destroy(other.gameObject.GetComponent<ConstantForce>());
			Destroy(other.gameObject.GetComponent<Rigidbody>());
			Managers.Audio.PlaySound(sound);
		}
	}

}
