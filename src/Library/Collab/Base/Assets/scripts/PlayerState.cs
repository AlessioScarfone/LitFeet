using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent (typeof (CharacterController))]
public class PlayerState : MonoBehaviour {

    private int _checkpointNumber = 0;
    private int keyCount = 0;
    private int bombCount = 0;

    [SerializeField] private int life = 3;
    public bool burning = false;

    void Awake () {
        Messenger.AddListener (GameEvent.DAMAGE, DamagePlayer);
    }

    void OnDestroy () {
        Messenger.RemoveListener (GameEvent.DAMAGE, DamagePlayer);
    }

    private void Update () {
        // Debug.Log("Life " + life);
    }

    private void Start () {
        GameData gd = SaveLoad.LoadFile();
        if (gd != null)
        { //significa che non è la prima partita quindi carico la scena dall'ultimo checkpoint e decremento la vita salvata nel checkpoint
            RestoreState(gd);
        }
        else
        {
            //siamo allo start iniziale quindi abbiamo le impostazioni di default

            Vector3 initialPosition = new Vector3(0.1f, 0.7f, 38.7f);
            gameObject.transform.position = initialPosition;
            //salvataggio di default
            gd = new GameData(gameObject, 0);
            SaveLoad.SaveFile(gd);
        }
    }

    public void DamagePlayer () {
        Debug.Log("Damaged!");
        life--;

        //se esiste un salvataggio andare a decrementare la vita in questo salvataggio
        SaveLoad.UpdateSavedFile(life);
        // GetComponent<CandleInputController> ().setDamaged (true);
    }

    public void SetCheckPointNumber (int p) {
        if (p > _checkpointNumber)
            _checkpointNumber = p;
    }
    public int GetCheckPointNumber () {
        return _checkpointNumber;
    }

    // ---------------
    //      Keys
    // ---------------

    public int GetKeyCount () {
        return keyCount;
    }
    public void SetKeyCount (int k) {
        keyCount = k;
    }

    public void RemoveKey () {
        keyCount--;
        Debug.Log ("Use a key. Total Key:" + keyCount);
        Messenger<int>.Broadcast (GameEvent.UPDATE_KEY_COUNT, keyCount);
    }

    public void AddKey () {
        keyCount++;
        Debug.Log ("Added a key. Total Key:" + keyCount);
        Messenger<int>.Broadcast (GameEvent.UPDATE_KEY_COUNT, keyCount);
    }

    // ---------------
    //      Bombs
    // ---------------

    public int GetBombCount () {
        return bombCount;
    }

    public void SetBombCount (int k) {
        bombCount = k;
    }

    public void RemoveBomb () {
        bombCount--;
    }

    public int GetLife () {
        return life;
    }

    public void Restart () {
        // GameData gd;
        if (life == 0) {
            GameOver ();
        } else {
            //start from last checkpoint
            // gd = SaveLoad.LoadFile ();
            // RestoreState (gd);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    //TODO eliminare i savecheckpoint che si trovano prima dell'ultimo checkpoint salvato
    public void RestoreState (GameData gd) {
        try
        {
            life = gd.GetLife();
            keyCount = gd.GetKeyCount();
            burning = gd.GetBurning();
            _checkpointNumber = gd.GetCheckPointNumber();
            gameObject.transform.position = gd.getPosition();
            Managers.LightManager.TurnOnLights(_checkpointNumber + 1);
            // Vector3 loadedRot = new Vector3 (gd.GetXRotation (), gd.GetYRotation(), gd.GetZRotation());
            // Debug.Log(loadedRot);
            // gameObject.transform.Rotate(loadedRot,Space.World);
            // Debug.Log(gd.getRotation());
            // gameObject.transform.localRotation = gd.getRotation();

            // gameObject.transform.eulerAngles = new Vector3(0,180,0);
            // gameObject.transform.Rotate(new Vector3(0,180,0));
            // Debug.Log("--"+gameObject.transform.eulerAngles.y);

            Messenger<int, int>.Broadcast(GameEvent.RESTORE_FROM_SAVE, keyCount, life);

        }
        catch (Exception e)
        {
            Debug.Log(e.StackTrace);
        }
    }

    public void GameOver () {
        Time.timeScale = 0;
        Messenger.Broadcast (GameEvent.KILLED);

        //delete save file
        SaveLoad.clearSavedFilesFolder ();

    }

}