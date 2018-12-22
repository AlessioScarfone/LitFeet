using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour {

	[SerializeField] private Button continueBtn;

	private void Start () {
		GameData gameData = SaveLoad.LoadFile ();
		if (gameData == null) {
			continueBtn.interactable = false;
			for (int i = 0; i < continueBtn.transform.childCount; i++) {
				//active strikethrough text
				(continueBtn.transform.GetChild (i).gameObject).SetActive (true);
			}
		}
	}

	public void OnNewGame () {
		SaveLoad.clearSavedFilesFolder ();
		SceneManager.LoadScene ("Level1", LoadSceneMode.Single);
	}

	public void OnContinue () {
		SceneManager.LoadScene ("Level1", LoadSceneMode.Single);
	}

	public void OnExit () {
		Application.Quit ();
	}

}