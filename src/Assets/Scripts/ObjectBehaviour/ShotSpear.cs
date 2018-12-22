using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpear : MonoBehaviour {

	[SerializeField]private GameObject spearprefab;
	public float shotForce = 75f;

	private GameObject spear;
	private bool _stop = false;
	public string spearSound;
	public bool soundOn = false;

	// Update is called once per frame
	void Update () {
		if (spear == null && !_stop) {
			spear = Instantiate (spearprefab) as GameObject;
			if (soundOn)
				Managers.Audio.PlaySound (spearSound);
			spear.GetComponent<SpearDamagePlayer> ().setShooter (this);
			// spear.transform.parent = gameObject.transform;
			spear.transform.position = transform.TransformPoint (Vector3.up);
			Rigidbody rb = spear.AddComponent (typeof (Rigidbody)) as Rigidbody;
			rb.isKinematic = false;
			rb.useGravity = true;

			rb.AddForce (Vector3.forward * shotForce, ForceMode.Impulse);

			// spear.transform.rotation = transform.rotation;
		}
	}

	public void StopShoting (bool value) {
		_stop = value;
	}

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player")
			soundOn = true;
	}
	private void OnTriggerExit (Collider other) {
		if (other.tag == "Player")
			soundOn = false;
	}
}