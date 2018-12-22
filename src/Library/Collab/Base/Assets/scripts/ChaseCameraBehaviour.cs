using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseCameraBehaviour : MonoBehaviour {

    // ------------
    // -- public --
    // ------------

    public GameObject candle;

    // -------------
    // -- private --
    // -------------

    private Vector3 distance;

    private void Start() {
        distance = candle.transform.position - transform.position;
    }

    private void Update() {
        transform.position = candle.transform.position - distance;
    }

}
