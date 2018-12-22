using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombDropHandler : MonoBehaviour {

	[SerializeField] private GameObject bombPrefab;

	// Use this for initialization
	void Start () { }

	void Update () {
		if (Input.GetKeyDown ("r")) {
			if (Managers.PlayerManager.GetBombCount () > 0 && Managers.PlayerManager.burning) {
				// Debug.Log ("Press");
				Vector3 pos = new Vector3 (transform.position.x, transform.position.y + 1.2f, transform.position.z);
				RaycastHit hit;
				// Debug.DrawRay (pos, transform.forward * 4, Color.red, 5);

				if (!Physics.SphereCast (pos, 1f, transform.forward, out hit, 3f)) {
					GameObject bomb = GameObject.Instantiate (bombPrefab);
					Vector3 bombPosition = transform.position + transform.forward * 3;
					bombPosition.y += 3;
					// Debug.Log (bombPosition);
					bomb.transform.position = bombPosition;
					bomb.GetComponent<BombBehaviour> ().Explode ();
					Managers.PlayerManager.RemoveBomb ();
				} else {
					// Debug.Log ("Impossibile to Drop bomb - Hit distance:" + hit.distance);
				}
			} else if (Managers.PlayerManager.burning == false){
				Managers.Audio.PlaySound ("CantActive");
				Debug.Log ("For use a bomb you need the fire");
			}
		}
	}
}