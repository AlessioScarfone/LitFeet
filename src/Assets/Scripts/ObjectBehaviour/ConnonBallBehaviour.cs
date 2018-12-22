using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnonBallBehaviour : MonoBehaviour {

	private void OnCollisionEnter(Collision coll) {
        DestroyWithCannon destroyWithCannon = coll.gameObject.GetComponent<DestroyWithCannon>();
        if (destroyWithCannon != null){
			ContactPoint[] c = coll.contacts;
			destroyWithCannon.Explode(c[0].point);

			Destroy(gameObject);
		}
	}


}
