using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItemBehaviour : MonoBehaviour {

    public string pickedSound;

	private void OnTriggerEnter(Collider other) {
		if(other.tag == "Player"){
            Managers.Audio.PlaySound(pickedSound);
			switch(gameObject.tag){
				case "PickableKey":
					Managers.PlayerManager.AddKey();
					break;
				case "PickableBomb":
					Managers.PlayerManager.AddBomb();
					break;
			}
			Destroy(gameObject);
		}
	}
}
