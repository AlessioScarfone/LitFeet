using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithExplosion : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void addRigidBody () {
		if (gameObject.GetComponent<Rigidbody> () == null) {
			Rigidbody body = gameObject.AddComponent (typeof (Rigidbody)) as Rigidbody;
			body.isKinematic = false;
			body.useGravity = true;
		}
	}
}