using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCheckPoint : MonoBehaviour {
    public int checkPoint;
   // public int numLamps;
    public List<GameObject> lamps;
    // private string name = "Katia";
    // private int score = 22;
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {

    }
    public void OnTriggerEnter (Collider other) {
        if (other.tag == "Player") {
                GameData gd = new GameData (other.gameObject, checkPoint);  
                if(SaveLoad.SaveFile (gd))
                    Destroy(gameObject);
            openLights(checkPoint+1);
        }
    }

    public void openLights(int floorNumber)
    {
      foreach (GameObject lamp in lamps)
        {
            lamp.GetComponent<Light>().intensity = 3;
        }
    }
}