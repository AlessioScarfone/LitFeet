using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IGameManager {
    private int _checkpointNumber = 0;
    private int _keyCount = 0;
    private int _bombCount = 0;
    public String damagedSound;
    public ManagerStatus status {
        get;
        private set;
    }

    [SerializeField] private int life = 3;
    [SerializeField] private GameObject candleFire;

    public bool burning = false;

    public void Startup () {
        // Debug.Log ("PlayerManager Starting ...");
        status = ManagerStatus.Started;
    }

    void Awake () {
        Messenger.AddListener (GameEvents.DAMAGE, DamagePlayer);
    }

    void OnDestroy () {
        Messenger.RemoveListener (GameEvents.DAMAGE, DamagePlayer);
    }

    // Use this for initialization
    void Start () {
        Debug.Log ("Try to restore a previous state:");
        GameData gd = SaveLoad.LoadFile ();
        if (gd != null) {
            Debug.Log ("Save file finded!");
            Managers.PersistenceManager.RestoreState (gd);
            Messenger<PossibleMessageText>.Broadcast (GameEvents.UPDATE_TIP, PossibleMessageText.WelcomeRestoreFromSave);
        } else {
            Debug.Log ("No Save File");
            GameObject player = GameObject.FindGameObjectWithTag ("Player");
            // Vector3 initialPosition = new Vector3 (0.1f, 0.7f, 38.7f);
            // player.transform.position = initialPosition;
            //create a default save
            gd = new GameData (player, 0);
            Managers.PersistenceManager.SaveFile(gd);
            Messenger<PossibleMessageText>.Broadcast (GameEvents.UPDATE_TIP, PossibleMessageText.Welcome);
        }
    }

    // Update is called once per frame
    void Update () { }

    public void DamagePlayer () {
        Managers.Audio.PlaySound(damagedSound);
        // Debug.Log ("Damaged!");
        life--;
        Messenger<PossibleMessageText>.Broadcast (GameEvents.UPDATE_TIP, PossibleMessageText.PlayerDamaged);

        //se esiste un salvataggio andare a decrementare la vita in questo salvataggio
        SaveLoad.UpdateSavedFile (life);
        // GetComponent<CandleInputController> ().setDamaged (true);
    }

    // ---------------
    //      CheckPoint
    // ---------------

    public void SetCheckPointNumber (int p) {
        if (p > _checkpointNumber)
            _checkpointNumber = p;
    }
    public int GetCheckPointNumber () {
        return _checkpointNumber;
    }

    public int GetLife () {
        return life;
    }

    public void SetLife (int newValue) {
        life = newValue;
    }

    // ---------------
    //      Keys
    // ---------------

    public int GetKeyCount () {
        return _keyCount;
    }
    public void SetKeyCount (int k) {
        _keyCount = k;
    }

    public void RemoveKey () {
        _keyCount--;
        // Debug.Log ("Use a key. Total Key:" + _keyCount);
        Messenger<int>.Broadcast (GameEvents.UPDATE_KEY_COUNT, _keyCount);
    }

    public void AddKey () {
        _keyCount++;
        // Debug.Log ("Added a key. Total Key:" + _keyCount);
        Messenger<int>.Broadcast (GameEvents.UPDATE_KEY_COUNT, _keyCount);
    }

    // ---------------
    //      Bombs
    // ---------------

    public int GetBombCount () {
        return _bombCount;
    }

    public void SetBombCount (int k) {
        _bombCount = k;
    }

    public void RemoveBomb () {
        _bombCount--;
        Messenger<int>.Broadcast (GameEvents.UPDATE_BOMB_COUNT, _bombCount);
    }

    public void AddBomb () {
        _bombCount++;
        Messenger<int>.Broadcast (GameEvents.UPDATE_BOMB_COUNT, _bombCount);
    }

    public void ShowCandleFire () {
        candleFire.SetActive (true);
    }

    public void HideCandleFire () {
        candleFire.SetActive (false);
    }

}