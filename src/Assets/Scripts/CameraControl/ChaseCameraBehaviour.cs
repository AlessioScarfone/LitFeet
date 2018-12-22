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

    private Vector3 _distance;

    private Vector3 startingPos;
    private bool _endGame = false;

    private void Start () {
        startingPos = gameObject.transform.position;
        // _distance = candle.transform.position - transform.position;
        _distance = new Vector3 (83.3f, -61.7f, -43.6f);
        // Debug.Log("Start camera distance:"+_distance);
    }

    private void Update () {
        if (_endGame) {
            transform.position = Vector3.MoveTowards (gameObject.transform.position, startingPos, 25f * Time.deltaTime);
            if (Vector3.Distance (transform.position, startingPos) < 1.0f) {
                GameObject.FindGameObjectWithTag ("MagicCandle").GetComponent<MagicCandleBehaviour> ().LightMagicFire ();
            }
        } else
            transform.position = candle.transform.position - _distance;
    }

    public void SetDistance (Vector3 dist) {
        _distance = dist;
    }

    public Vector3 getStartingPosition () {
        return startingPos;
    }

    public void EndGame () {
        _endGame = true;
    }

}