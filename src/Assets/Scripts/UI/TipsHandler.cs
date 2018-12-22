using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TipsHandler : MonoBehaviour{

	[SerializeField] private GameObject tipsTitle;
	[SerializeField] private GameObject tipsText;
	[SerializeField] private Button collapseButton;

	[SerializeField] private Sprite collapse_up;
	[SerializeField] private Sprite collapse_down;

	private bool _closed = true;

	private Color whiteColor;
	private Color greenColor;
	private Color yellowColor;
	private Color redColor;

	private IEnumerator runningCoroutine = null;

	// Use this for initialization
	void Start () {
		whiteColor = createNormalizedColor (255, 255, 255, 33);
		greenColor = createNormalizedColor (0, 255, 55, 33);
		yellowColor = createNormalizedColor (255, 216, 0, 155);
		redColor = createNormalizedColor (100, 0, 0, 100);
	}

	public void OnShowHideTipText () {
		if (_closed) {
			tipsText.SetActive (true);
			collapseButton.GetComponentInChildren<Image> ().sprite = collapse_up;
			_closed = false;
		} else {
			tipsText.SetActive (false);
			collapseButton.GetComponentInChildren<Image> ().sprite = collapse_down;
			ResetTitleColor ();
			_closed = true;
		}
	}

	public void ShowTipTextWithTimer (float second) {
		runningCoroutine = ShowTipsWithTimer (second);
		StartCoroutine (runningCoroutine);
	}

	public void StopTipTextWithTimer () {
		if (runningCoroutine != null) {
			// Debug.Log("STOPPED");
			StopCoroutine (runningCoroutine);
		}
	}

	public bool isClosed () {
		return _closed;
	}

	public void ForceHide () {
		tipsText.SetActive (false);
		collapseButton.GetComponentInChildren<Image> ().sprite = collapse_down;
		ResetTitleColor ();
		_closed = true;
	}

	public void SetTipText (string tipText, int gravity) {
		switch (gravity) {
			case 0:
				tipsTitle.GetComponent<Image> ().color = whiteColor;
				break;
			case 1:
				tipsTitle.GetComponent<Image> ().color = greenColor;
				break;
			case 2:
				tipsTitle.GetComponent<Image> ().color = yellowColor;
				break;
			case 3:
				tipsTitle.GetComponent<Image> ().color = redColor;
				break;
		}
		tipsText.GetComponentInChildren<Text> ().text = tipText;
		if (_closed == false)
			OnShowHideTipText ();
	}

	private IEnumerator ShowTipsWithTimer (float second) {
		if (_closed) {
			OnShowHideTipText ();
		}
		// else{
		// 	tipsText.SetActive (false);
		// }
		yield return new WaitForSeconds (second);
		if (_closed == false)
			OnShowHideTipText ();

		runningCoroutine = null;
	}

	private void ResetTitleColor () {
		tipsTitle.GetComponent<Image> ().color = whiteColor;
	}

	private Color createNormalizedColor (float r, float g, float b, float a) {
		return new Color (r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
	}

	

}