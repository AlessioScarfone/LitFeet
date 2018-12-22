using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Play3DLoopSound : MonoBehaviour {
    public string loop3DSound;

    // Use this for initialization
    void Start () {

    }

    private void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            Managers.Audio.PlayLoopSound (loop3DSound);

        }
    }
    private void OnTriggerExit (Collider other) {
        if (other.tag == "Player") {
            Managers.Audio.StopLoopSounds ();
        }
    }
}