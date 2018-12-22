using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObjectWithPression : MonoBehaviour {
	[Space (10)]
	public IActivableObject[] objectToActive;

	private int objCount = 0;

	private void OnTriggerEnter (Collider other) {
		objCount++; //count object on the button
		foreach (var obj in objectToActive) {
			if (obj.sound_active != null)
				Managers.Audio.PlaySound (obj.sound_active);
		}
		// Debug.Log("Enter -> objCount:"+objCount);
	}

	void OnTriggerStay (Collider other) {
		foreach (var obj in objectToActive) {
			obj.ActiveObject ();
		}
	}

	void OnTriggerExit (Collider other) {
		objCount--;
		// Debug.Log("Exit -> objCount:"+objCount);
		if (objCount == 0)
			foreach (var obj in objectToActive) {
				obj.DeactivateObject ();
			}

	}

}