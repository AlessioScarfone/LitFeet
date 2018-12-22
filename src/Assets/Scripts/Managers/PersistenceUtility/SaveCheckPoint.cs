using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheckPoint : MonoBehaviour {
    public string _sound;
    bool playAudio = false;
    bool saved = false;
    public int checkPointNumber;
    [SerializeField] private Material greenFlag;
    // Use this for initialization
    void Start () { }

    void Update () {
        if (playAudio) {
            StartCoroutine ("SoundOn");
            playAudio = false;
        }
    }
    public IEnumerator SoundOn () {

        Managers.Audio.PlaySound (_sound);
        yield return new WaitForSeconds (1f);
        if (saved)
            ChangeFlagColor();
        // Destroy (gameObject);

    }

    public void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
            gameObject.GetComponent<BoxCollider> ().enabled = false;
            // gameObject.GetComponent<MeshRenderer>().enabled = false;
            playAudio = true;
            Debug.Log ("Entered in a checkpoint");
            GameData gd = new GameData (other.gameObject, checkPointNumber);
            if (Managers.PersistenceManager.SaveFile (gd)) { saved = true; }
            Managers.LightManager.TurnOnLights (checkPointNumber + 1);
        }
    }

    public void ChangeFlagColor () {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            GameObject child = gameObject.transform.GetChild (i).gameObject;
            if (child.tag == "CheckPointFlag") {
                child.GetComponent<Renderer> ().material = greenFlag;
            }
        }
    }

}