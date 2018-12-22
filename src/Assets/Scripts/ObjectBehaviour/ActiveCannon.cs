using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (ParticleSystem))]
public class ActiveCannon : IActivableObject {
	[SerializeField] private GameObject shotfire;
	[SerializeField] private GameObject cannonBallPrefab;

	private GameObject cannonball = null;
	private Vector3 alignBallToCannon = new Vector3 (0, 0.5f, 0); //align ball with cannon

	public Vector3 shotDirection;
	public int force = 80;
	public string soundExplosion;

	//limit shot to one
	private int numberOfShot = 1;

	void Start () {
		shotDirection = shotDirection.normalized;
	}

	public override bool ActiveObject () {
		if (Managers.PlayerManager.burning && numberOfShot > 0) {
			// fire.SetActive(true);
			gameObject.GetComponent<ParticleSystem> ().Play ();
			StartCoroutine (ShootCannonBall ());
			return true;
		}
		return false;
	}

	private IEnumerator ShootCannonBall () {
		numberOfShot--;
		yield return new WaitForSeconds (1.2f);
		shotfire.SetActive (true);
		// Debug.Log ("SHOT CANNONBALL");
		yield return new WaitForSeconds (0.2f);
		//shoot ONLY ONE BALL AT TIME (IF THE FIRST BALL IS DESTROY YOU CAN FIRE AGAIN)
		if (cannonball == null) {
			cannonball = Instantiate (cannonBallPrefab) as GameObject;
			Managers.Audio.StopSound (); //stop fire sound
			cannonball.transform.parent = transform;
			cannonball.transform.position = cannonball.transform.parent.position + shotDirection * 5 + alignBallToCannon;
			cannonball.transform.rotation = cannonball.transform.parent.rotation;
			cannonball.GetComponent<Rigidbody> ().AddForce (shotDirection * force, ForceMode.Impulse);
			Managers.Audio.PlaySound (soundExplosion);

		}
		shotfire.SetActive (false);

	}

	public override bool DeactivateObject () {
		return false;
	}

	public override BalloonType BalloonTypeNeeded () {
		return BalloonType.Action;
	}
}