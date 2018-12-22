using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildAdopterBehaviour : MonoBehaviour {

    // -------------
    // -- private --
    // -------------

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Adoptable" || other.tag == "Player" || other.tag == "Pushable") {
            other.transform.parent = transform.parent;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Adoptable" || other.tag == "Player" || other.tag == "Pushable") {
            other.transform.parent = null;
        }
    }

}
