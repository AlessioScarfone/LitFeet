using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour {

	public GameObject magicCandle;
	public Transform endPosition;

	private GameObject _camera;

	private void Start () {
		_camera = GameObject.FindGameObjectWithTag ("MainCamera");
	}

	private void OnTriggerEnter (Collider other) {
		Rigidbody r = magicCandle.GetComponent<Rigidbody> ();
		r.isKinematic = true;
		magicCandle.transform.position = endPosition.position;
		if (_camera != null) {
			_camera.GetComponent<ChaseCameraBehaviour> ().EndGame ();
		}
	}

}