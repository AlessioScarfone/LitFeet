using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour {

	[SerializeField] private GameObject explosionParticleSystem;
	[SerializeField] private GameObject firefuse;
    public String _soundTimer;
    public String _soundBomb;

    private float timer = 2f;
	private int _explosionSphereRadius = 5;
	private int _explosionForce = 50;
   
    private List<string> destructibleTagList = new List<string> { "Destructible" };

	// Use this for initialization
	void Start () {
        //ignore collision with player
        Physics.IgnoreCollision (GameObject.FindGameObjectWithTag ("Player").gameObject.GetComponent<Collider> (), GetComponent<Collider> ());
	}

	// Update is called once per frame
	void Update () { }

	public void Explode () {
		StartCoroutine (ExplodeCoroutine ());
	}

	private IEnumerator ExplodeCoroutine () {
		firefuse.SetActive (true);
        Managers.Audio.PlaySound(_soundTimer);
        yield return new WaitForSeconds (timer);
        Managers.Audio.PlaySound(_soundBomb);

        Collider[] hitColliders = Physics.OverlapSphere (transform.position, _explosionSphereRadius);

		// -- Draw Sphere for Debug --
		// GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// sphere.transform.position = gameObject.transform.position;
		// sphere.transform.localScale = new Vector3(10,10,10);

		Destroy (firefuse);
		explosionParticleSystem.SetActive (true);
		yield return new WaitForSeconds (0.3f);
		foreach (var item in hitColliders) {
			if (item.transform.root != transform) { //ignore collision with bomb itself
				// Debug.Log (item.name);
				// if is possible apply an explosive force to the rigidbody, else create add a rigidbody or destroy it
				Rigidbody rb = item.GetComponent<Rigidbody> ();
				MoveWithExplosion mve = item.gameObject.GetComponent<MoveWithExplosion> ();

				if (destructibleTagList.Contains (item.gameObject.tag)) {
					// Debug.Log(item.tag+"-"+destructibleTagList.Contains(item.gameObject.tag));
					Destroy (item.gameObject);
				} else if (rb != null)
					AddExpolosionForce (rb);
				else if (mve != null) {
					mve.addRigidBody ();
					AddExpolosionForce (mve.gameObject.GetComponent<Rigidbody> ());
				}
				if (item.tag == "Player") {
					// Debug.Log ("Player Damaged from explosion!");
					Messenger.Broadcast (GameEvents.DAMAGE);
					Managers.BalloonManager.HideAll();
					item.gameObject.GetComponent<Animator> ().SetTrigger ("Dying");
				}
			}
		}
		gameObject.GetComponent<MeshRenderer> ().enabled = false;
		gameObject.GetComponent<BoxCollider> ().enabled = false;
		yield return new WaitForSeconds (5f);
		Destroy (gameObject);

	}

	private void AddExpolosionForce (Rigidbody rb) {
		rb.AddExplosionForce (_explosionForce / rb.mass, transform.position, _explosionSphereRadius, 0.0F, ForceMode.Impulse);
	}
}