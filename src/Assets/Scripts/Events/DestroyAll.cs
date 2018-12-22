using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAll : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player" && other.tag != "Indestructible")
        {
            Destroy(other.gameObject);
            // Debug.Log("DESTROY PLANE:" + other.name);

        }
    }

}