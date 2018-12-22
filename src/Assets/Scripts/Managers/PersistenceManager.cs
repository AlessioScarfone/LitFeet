using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistenceManager : MonoBehaviour, IGameManager {
	[SerializeField] GameObject checkpointsParent;

	public ManagerStatus status {
		get;
		private set;
	}

	public void Startup () {
		// Debug.Log ("PlayerManager Starting ...");
		status = ManagerStatus.Started;
	}

	private void Awake () {
	}

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {

	}

	public void Restart () {
		// GameData gd;
		if (Managers.PlayerManager.GetLife () == 0) {
			GameOver ();
		} else {
			//start from last checkpoint
			// gd = SaveLoad.LoadFile ();
			// RestoreState (gd);
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
		}
	}

    public bool SaveFile(GameData gd)
    {
        return SaveLoad.Save(gd);
    }
	public void RestoreState (GameData gd) {
		try {
			Managers.PlayerManager.SetLife (gd.GetLife ());
			Managers.PlayerManager.SetKeyCount (gd.GetKeyCount ());
			Managers.PlayerManager.burning = gd.GetBurning ();
			if (gd.GetBurning () == true)
				Managers.PlayerManager.ShowCandleFire ();
			
			Managers.PlayerManager.SetCheckPointNumber (gd.GetCheckPointNumber ());
			HidePreviousCheckPoint(gd.GetCheckPointNumber());
			GameObject player = GameObject.FindGameObjectWithTag ("Player");
			player.transform.position = gd.getPosition ();
			Managers.LightManager.TurnOnLights (Managers.PlayerManager.GetCheckPointNumber () + 1);

			Managers.LightManager.TurnOnLightsUntilFloor (gd.GetCheckPointNumber () + 1);
			Messenger<int, int>.Broadcast (GameEvents.RESTORE_FROM_SAVE, Managers.PlayerManager.GetKeyCount (), Managers.PlayerManager.GetLife ());

		} catch (Exception e) {
			Debug.Log (e.Message);
		}
	}

	private void HidePreviousCheckPoint (int currentCheckPointNumber) {
		for (int i = 0; i < checkpointsParent.transform.childCount; i++) {
			GameObject currentCheckpoint = checkpointsParent.transform.GetChild (i).gameObject;
			int checkpointNumber = currentCheckpoint.GetComponent<SaveCheckPoint>().checkPointNumber;
			// Debug.Log("CN:"+checkpointNumber);
			if( checkpointNumber <= currentCheckPointNumber){
				// currentCheckpoint.SetActive(false);
				currentCheckpoint.GetComponent<SaveCheckPoint>().ChangeFlagColor();
				currentCheckpoint.GetComponent<BoxCollider> ().enabled = false;
			}
		}

	}

	public void GameOver () {
		Time.timeScale = 0;
		Messenger.Broadcast (GameEvents.KILLED);

		//delete save file
		SaveLoad.clearSavedFilesFolder ();
	}

	public void RemoveAllSave () {
		SaveLoad.clearSavedFilesFolder ();
	}

}