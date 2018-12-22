using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PossibleMessageText {
	//0 -> white-default
	Null,

	//1 -> green
	Welcome,
	WelcomeRestoreFromSave,
	CheckPointReached,
	ActiveObject,
	MagicCandle,

	//2 -> yellow
	LightFireCandle,
	BurnObject,
	Key,
	Bomb,
	WorkInProgress,
	ClosedGate,

	//3 -> red
	PlayerDamaged
}
public class UIController : MonoBehaviour {

	[SerializeField] private PausePopup pausePopup;
	[SerializeField] private GameObject lifePanel;
	[SerializeField] private GameObject keyPanel;
	[SerializeField] private GameObject bombPanel;
	[SerializeField] private GameObject GameOverPanel;
	[SerializeField] private GameObject VictoryPanel;

	[SerializeField] private GameObject filledHeartPrefab;

	[SerializeField] private Sprite emptyHearthSprite;
	[SerializeField] private Sprite filledHearthSprite;

	private List<GameObject> _hearthlist;

	// -- TIPS --
	[SerializeField] GameObject tipsContainer;
	private TipsHandler _tipsHandler;
	private Dictionary<PossibleMessageText, string> _messageList = new Dictionary<PossibleMessageText, string> ();
	private Dictionary<PossibleMessageText, int> _messageListGravity = new Dictionary<PossibleMessageText, int> ();

	//-- SOUND --
	[SerializeField] private AudioClip soundButtonPress;
	[SerializeField] private AudioClip GameOverSound;
	[SerializeField] private AudioClip victory;

	// -- CURSOR --
	public Texture2D _cursorTexture;
	private CursorMode _cursorMode = CursorMode.Auto;
	private Vector2 _hotSpot = Vector2.zero;
	private bool _lockCursorOnShow = false;
	private bool _locksceen = false;

	void Awake () {

		Cursor.visible = false;
		Cursor.SetCursor (_cursorTexture, _hotSpot, _cursorMode);

		Messenger.AddListener (GameEvents.DAMAGE, DamagePlayer);
		Messenger<int>.AddListener (GameEvents.UPDATE_KEY_COUNT, UpdateKeyCount);
		Messenger<int>.AddListener (GameEvents.UPDATE_BOMB_COUNT, UpdateBombCount);
		Messenger<int, int>.AddListener (GameEvents.RESTORE_FROM_SAVE, RestoreUIFormSave);
		Messenger.AddListener (GameEvents.KILLED, GameOver);
		Messenger<PossibleMessageText>.AddListener (GameEvents.UPDATE_TIP, UpdateTipsText);
		Messenger<PossibleMessageText, float>.AddListener (GameEvents.UPDATE_TIP_TIMER, UpdateTipsTextWithTime);
		Messenger.AddListener (GameEvents.HIDE_AND_RESET_TIP, HideAndResetTips);
		Messenger.AddListener (GameEvents.WIN_GAME, ShowVictoryPanel);

		initTipsMessage ();
	}

	private void Start () {
		_tipsHandler = tipsContainer.GetComponent<TipsHandler> ();

		int life = Managers.PlayerManager.GetLife ();
		for (int i = 0; i < life; i++) {
			GameObject fill_hearth;
			fill_hearth = Instantiate (filledHeartPrefab) as GameObject;
			fill_hearth.transform.SetParent (lifePanel.transform);
		}
		_hearthlist = getAllChildrenHearth ();

		pausePopup.Close ();
		GameOverPanel.SetActive (false);
		VictoryPanel.SetActive (false);
	}

	private void initTipsMessage () {
		AddPossibleMessage (PossibleMessageText.Null, "", 0);
		AddPossibleMessage (PossibleMessageText.Welcome, "Welcome Player!\n\nUse the 'Arrow Keys' or 'W,A,S,D' for moving and hold 'Shift' for run. :)\n", 1);
		AddPossibleMessage (PossibleMessageText.WelcomeRestoreFromSave, "Welcome Player!\n\nUse the 'Arrow Keys' or 'W,A,S,D' for moving and hold 'Shift' for run :)\n\nSaved game restored!", 1);
		AddPossibleMessage (PossibleMessageText.CheckPointReached, "Great! You have reached a checkpoint.\nWhen you lose an heart you can RESTART FROM THIS POINT.\n\nWhen you lost all hearts, the game will RESTART FROM THE BEGINNING!", 1);
		AddPossibleMessage (PossibleMessageText.MagicCandle, "THE MAGIC LIGHT!!!.\n\nBut... something strange happened in the fall...\n\nThe light is off and has lit a fire!!! D:", 1);
		AddPossibleMessage (PossibleMessageText.LightFireCandle, "Press 'E' near a lamp for light on candle fire! \n\nDon't play with the fire....", 2);
		AddPossibleMessage (PossibleMessageText.ActiveObject, "Press 'E' near an object for active it! \n\nUnexpected things could happen....", 2);
		AddPossibleMessage (PossibleMessageText.Key, "Wow! You have just collected a golden key!! *-* ", 2);
		AddPossibleMessage (PossibleMessageText.BurnObject, "Sometimes, if you have the burning head, you can burn an object! \n\nDon't play with the fire........", 2);
		AddPossibleMessage (PossibleMessageText.PlayerDamaged, "Ops!\n\nAttention: if you fall too high you will lose a heart.\n\nAnd.....some traps can kill you in one shot! D: ", 3);
		AddPossibleMessage (PossibleMessageText.Bomb, "Wow! You have just collected a bomb!\nPress 'R' to drop it when you are at a right distance to the object!\n\nP.S. You need fire and....\nbe careful with the explosion!!", 2);
		AddPossibleMessage (PossibleMessageText.WorkInProgress, "OHOH! From this point the tower is full of traps!... May be danger! :(", 2);
		AddPossibleMessage (PossibleMessageText.ClosedGate, "This gate seems to be closed !! ", 2);
	}

	private void AddPossibleMessage (PossibleMessageText textEnum, String text, int gravity) {
		_messageList.Add (textEnum, text);
		_messageListGravity.Add (textEnum, gravity);
	}

	void OnDestroy () {
		Messenger.RemoveListener (GameEvents.DAMAGE, DamagePlayer);
		Messenger<int>.RemoveListener (GameEvents.UPDATE_KEY_COUNT, UpdateKeyCount);
		Messenger<int>.RemoveListener (GameEvents.UPDATE_BOMB_COUNT, UpdateBombCount);
		Messenger<int, int>.RemoveListener (GameEvents.RESTORE_FROM_SAVE, RestoreUIFormSave);
		Messenger.RemoveListener (GameEvents.KILLED, GameOver);
		Messenger<PossibleMessageText>.RemoveListener (GameEvents.UPDATE_TIP, UpdateTipsText);
		Messenger<PossibleMessageText, float>.RemoveListener (GameEvents.UPDATE_TIP_TIMER, UpdateTipsTextWithTime);
		Messenger.RemoveListener (GameEvents.HIDE_AND_RESET_TIP, HideAndResetTips);
		Messenger.RemoveListener (GameEvents.WIN_GAME, ShowVictoryPanel);
	}

	private void UpdateTipsTextWithTime (PossibleMessageText textEnum, float duration) {
		if (_tipsHandler.isClosed ()) { //if tips panel is closed, show it else change only the text
			_tipsHandler.SetTipText (_messageList[textEnum], _messageListGravity[textEnum]);
			_tipsHandler.ShowTipTextWithTimer (duration);
		} else {
			_tipsHandler.StopTipTextWithTimer ();
			_tipsHandler.SetTipText (_messageList[textEnum], _messageListGravity[textEnum]);
			_tipsHandler.ShowTipTextWithTimer (duration); //close	
		}

	}

	private void UpdateTipsText (PossibleMessageText textEnum) {
		if (_tipsHandler.isClosed ()) { //if tips panel is closed,  change the text and show it 
			_tipsHandler.SetTipText (_messageList[textEnum], _messageListGravity[textEnum]);
			_tipsHandler.OnShowHideTipText ();
		} else { //if tips panel is open, close, change the text and show it 
			_tipsHandler.StopTipTextWithTimer (); //stop tips with timer
			_tipsHandler.OnShowHideTipText (); //close	
			_tipsHandler.SetTipText (_messageList[textEnum], _messageListGravity[textEnum]);
			_tipsHandler.OnShowHideTipText (); //open
		}
	}

	private void HideAndResetTips () {
		PossibleMessageText msgnull = PossibleMessageText.Null;
		_tipsHandler.SetTipText (_messageList[msgnull], _messageListGravity[msgnull]);
		_tipsHandler.ForceHide ();
	}

	private void RestoreUIFormSave (int keyCount, int life) {
		UpdateKeyCount (keyCount);
		UpdateLife (life);
	}

	private void UpdateKeyCount (int keyCount) {
		Text t = keyPanel.GetComponentInChildren<Text> ();
		t.text = "X " + keyCount;
	}

	private void UpdateBombCount (int bombCount) {
		Text t = bombPanel.GetComponentInChildren<Text> ();
		t.text = "X " + bombCount;
	}

	private void DamagePlayer () {
		//Debug.Log ("DAMAGED");
		for (int i = _hearthlist.Count - 1; i >= 0; i--) {
			//Debug.Log (_hearthlist[i].name);
			GameObject child = _hearthlist[i];
			if (child.tag == "FilledHearth") {
				child.tag = "EmptyHearth";
				child.GetComponent<Image> ().sprite = emptyHearthSprite;
				break;
			}
		}
	}

	public void OnOpenPause () {
		_lockCursorOnShow = true;
		Cursor.visible = true;
		Managers.Audio.PlaySound (soundButtonPress);

		pausePopup.Open ();
	}

	public void OnClosePause () {
		_lockCursorOnShow = false;
		Cursor.visible = false;
		Managers.Audio.PlaySound (soundButtonPress);

		pausePopup.Close ();
	}

	public void OnRestartLevel () {
		Managers.PersistenceManager.RemoveAllSave ();
		RestartAfterGameOver ();
	}

	public void OnQuit () {
		SceneManager.LoadScene ("StartMenu", LoadSceneMode.Single);
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape) && !_locksceen) {
			if (pausePopup.isActive ())
				OnClosePause ();
			else
				OnOpenPause ();
		}
	}

	private List<GameObject> getAllChildrenHearth () {
		List<GameObject> children = new List<GameObject> ();
		for (int i = 0; i < lifePanel.transform.childCount; i++) {
			children.Add (lifePanel.transform.GetChild (i).gameObject);
		}
		return children;
	}

	private int CountFillHearth () {
		int count = 0;
		foreach (GameObject heart in _hearthlist) {
			if (heart.tag == "FilledHearth")
				count++;
		}
		return count;
	}

	private void UpdateLife (int l) {
		for (int i = 0; i <= _hearthlist.Count - 1; i++) {
			GameObject child = _hearthlist[i];
			if (i < l) {
				//Debug.Log(_hearthlist[i].name);
				child.GetComponent<Image> ().sprite = filledHearthSprite;
			} else {

				child.GetComponent<Image> ().sprite = emptyHearthSprite;
			}
		}

	}

	private void GameOver () {
		Time.timeScale = 0;
		UpdateLife (0);
		HideAllUIElement ();
		_locksceen = true;
		_lockCursorOnShow = true;
		Cursor.visible = true;
		GameOverPanel.SetActive (true);
		Managers.Audio.StopMusic ();
		Managers.Audio.PlaySound (GameOverSound);

	}

	public void RestartAfterGameOver () {
		// GameOverPanel.SetActive (false);
		// VictoryPanel.SetActive (false);
		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex, LoadSceneMode.Single);
		Time.timeScale = 1;
	}

	public void ShowCursor () {
		Cursor.visible = true;
	}

	public void HideCursor () {
		if (!_lockCursorOnShow)
			Cursor.visible = false;
	}

	private void ShowVictoryPanel () {
		Time.timeScale = 0;
		_locksceen = true;
		_lockCursorOnShow = true;
		ShowCursor ();
		HideAllUIElement ();
		if(VictoryPanel != null)
			VictoryPanel.SetActive (true);
		Managers.PersistenceManager.RemoveAllSave ();

		Managers.Audio.PlaySound (victory);
	}

	private void HideAllUIElement () {
		if (lifePanel != null)
			lifePanel.SetActive (false);
		if (bombPanel != null)
			bombPanel.SetActive (false);
		if (keyPanel != null)
			keyPanel.SetActive (false);
		if (tipsContainer != null)
			tipsContainer.SetActive (false);
	}

}