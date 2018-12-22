using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour {


	public enum PermittedOperations {
        OnlyActive,
		OnlyDisable,
		Active_Disable
    };

	public PermittedOperations operation;
	public IActivableObject objectToActive;
	private bool actived = false;

	void OnTriggerEnter(Collider other){
		if(other.tag == "Player"){
			Debug.Log("You can active something!");
		}
		OnTriggerStay(other);
		
	}

	void OnTriggerStay(Collider other){
		if(other.tag == "Player"){
			// Debug.Log("You can active something!");
			if (!actived && Input.GetKeyDown("e")){
				if(operation == PermittedOperations.OnlyActive || operation == PermittedOperations.Active_Disable){
					objectToActive.ActiveObject();
					actived = true;
				}
			}
			else if((actived && Input.GetKeyDown("e"))){
				if(operation == PermittedOperations.OnlyDisable || operation == PermittedOperations.Active_Disable){
					objectToActive.DisableObject();
					actived = false;
				}
			}
		}
	}

}
