using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.transform.rotation = Quaternion.Euler(0, 0.5f, 0);
       
        Debug.Log("Rotation done y:" + other.gameObject.transform.rotation.y);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
