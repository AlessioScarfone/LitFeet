using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Rigidbody))]
public class ShakingBehaviour : MonoBehaviour {
	[Tooltip ("Vector that set the 'shake direction'")]
	public Vector3 shakeVector;
	[Tooltip ("Time before the fall of the object")]
	public float duration = 5;

	private bool _actived = false;
	private float _shake_angle = 1f;
	private int _revertdir = -1;

    public AudioClip droppingObjectSound;

	// Use this for initialization
	void Start () {
		shakeVector = shakeVector.normalized;
	}

	private void Update () {
		if (_actived) {
			_revertdir *= -1;
			transform.Rotate (shakeVector * _shake_angle * _revertdir * Time.timeScale);
			// transform.Rotate(new Vector3(0,0,_revertdir*_shake_angle)); 
		}
	}

	private void OnTriggerEnter (Collider other) {
		if(_actived == false && (other.tag == "Player" || other.tag == "Pushable"))
				StartCoroutine (Flickering ());
	}

	private IEnumerator Flickering () {
		Rigidbody body = gameObject.GetComponent<Rigidbody> ();
		float timesplit = duration / 4;

		yield return new WaitForSeconds (timesplit);
		_actived = true; //start flickerint

		yield return new WaitForSeconds (timesplit);
		_shake_angle += 3f; //flickering angle = 4

		yield return new WaitForSeconds (timesplit);
		_shake_angle += 2f; //flickering angle = 6

		yield return new WaitForSeconds (timesplit);
        Managers.Audio.PlaySound(droppingObjectSound);
		body.isKinematic = false;
        body.AddForce(new Vector3(0,-40,0),ForceMode.VelocityChange);
        _actived = false;
        body.detectCollisions = false;

        yield return new WaitForSeconds(duration * 2);
        Destroy(this.gameObject);
        // yield return new WaitForSeconds (0.1f);
        // float fallRotation = 60f;
        // transform.Rotate (new Vector3 (1, 0, 0) * fallRotation, Space.Self);

    }
}