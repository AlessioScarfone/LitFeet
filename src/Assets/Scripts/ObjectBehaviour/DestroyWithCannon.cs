using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWithCannon : MonoBehaviour {

	[SerializeField] private GameObject explosionPrefab;

	public void Explode (Vector3 explosionPoint) {
		GameObject[] explosions = new GameObject[6];

		for (int i = 0; i < explosions.Length; i++) {
			explosions[i] = Instantiate (explosionPrefab) as GameObject;
		}

		// explosion.transform.parent = transform;
		// explosion.transform.position = explosion.transform.parent.position;
		// explosion.transform.rotation = explosion.transform.parent.rotation;
		// explosion.transform.parent = transform.root;
		explosions[0].transform.position = explosionPoint;
		explosions[1].transform.position = explosionPoint + new Vector3 (0, 0, -7f);
		explosions[2].transform.position = explosionPoint + new Vector3 (-2, 2, 0f);
		explosions[3].transform.position = explosionPoint + new Vector3 (2, 2, 0f);
		explosions[4].transform.position = explosionPoint + new Vector3 (0, -2, 2f);
		explosions[5].transform.position = explosionPoint + new Vector3 (0, -2, -2f);
		Managers.Audio.PlaySound ("Explosion");

		StartCoroutine (DestroyExplosions (explosions));

		// explosion.GetComponent<ParticleSystem>().Play();
	}

	private IEnumerator DestroyExplosions (GameObject[] explosions) {
		yield return new WaitForSeconds (0.25f);
		gameObject.GetComponent<MeshRenderer>().enabled = false;
		gameObject.GetComponent<Collider>().enabled = false;
		// yield return new WaitForSeconds (2f);
		yield return new WaitForSeconds (10.0f);
		for (int t = 0; t < explosions.Length; t++) {
			Destroy (explosions[t].gameObject);
		}
		Destroy (gameObject);
	}
}