using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BalloonType { Action, Fire, NOACTION }
public class BalloonManager : MonoBehaviour, IGameManager {

	[SerializeField] private GameObject balloonParent;
	private Dictionary<BalloonType, GameObject> ballons = new Dictionary<BalloonType, GameObject> ();
	private Dictionary<BalloonType, int> ballonsActiveCount = new Dictionary<BalloonType, int> ();
	private Vector3 originalPos;
	private Quaternion originalRot;

	public ManagerStatus status {
		get;
		private set;
	}

	public void Startup () {
		status = ManagerStatus.Initializing;
		foreach (Transform t in balloonParent.transform) {
			GameObject currentObj = t.gameObject;
			if (currentObj.tag == "ActionBalloon") {
				ballons.Add (BalloonType.Action, currentObj);
				ballonsActiveCount.Add (BalloonType.Action, 0);
			} else if (currentObj.tag == "FireBalloon") {
				ballons.Add (BalloonType.Fire, currentObj);
				ballonsActiveCount.Add (BalloonType.Fire, 0);
			}
		}
		originalPos = ballons[BalloonType.Action].transform.localPosition;
		originalRot = ballons[BalloonType.Action].transform.localRotation;
		status = ManagerStatus.Started;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	public void ShowBalloon (BalloonType type) {
		if (type != BalloonType.NOACTION) {
			// Debug.Log ("SHOW:" + type);
			GameObject b = ballons[type];
			b.SetActive (true);
			ballonsActiveCount[type]++;
			// Debug.Log ("SHOW:" + type + " - " + ballonsActiveCount[type]);
			if (CountBalloonActive ().Count > 1) {
				AdjustBalloonPosition ();
			}
		}
		// Debug.Log("Showed:"+type.ToString()+" ballon actived:"+ballonsActiveCount[type]);

	}

	public void HideBalloon (BalloonType type) {
		if (type != BalloonType.NOACTION && ballonsActiveCount[type] > 0) {
			// Debug.Log ("HIDE:" + type);
			GameObject b = ballons[type];
			ballonsActiveCount[type]--;
			// Debug.Log("TRY TO HIDE:"+type.ToString()+" count:"+ ballonsActiveCount[type]);
			if (ballonsActiveCount[type] == 0) {
				b.SetActive (false);
				ResetPosition (b);
				List<GameObject> list = CountBalloonActive ();
				if (list.Count == 1)
					ResetPosition (list[0]);
			}
		} else if (type != BalloonType.NOACTION && ballonsActiveCount[type] == 0)
			throw new Exception ("Try to hide ad ballon with count already equal to 0");
		// Debug.Log("HIDE:"+type.ToString()+"-"+ballonsActiveCount[type]);
	}

	public void HideAll () {
		foreach (var key in ballons.Keys) {
			ballons[key].SetActive (false);
			ResetPosition (ballons[key]);
			ballonsActiveCount[key]--;
		}

		// foreach (var item in ballons) {
		// 	item.Value.SetActive (false);
		// 	// ResetPosition(item.Value);
		// }
	}

	public List<GameObject> CountBalloonActive () {
		List<GameObject> activeBalloons = new List<GameObject> ();
		foreach (var key in ballons.Keys) {
			if (ballons[key].activeSelf) {
				activeBalloons.Add (ballons[key]);
			}
		}
		// Debug.Log ("Count Active Balloons:" + activeBalloons.Count);
		return activeBalloons;
	}

	private void AdjustBalloonPosition () {
		List<GameObject> activeBallon = new List<GameObject> ();
		foreach (var key in ballons.Keys) {
			if (ballons[key].activeSelf)
				activeBallon.Add ((ballons[key]));
		}
		int dir = -1;

		foreach (GameObject b in activeBallon) {
			Transform objTransf = b.transform;
			float traslateValue = dir * 4f;
			objTransf.localPosition = new Vector3 (objTransf.localPosition.x + traslateValue, objTransf.localPosition.y, objTransf.localPosition.z);
			objTransf.Rotate (Vector3.forward, -35 * dir, Space.Self);
			dir *= -1;
		}
	}

	private void ResetPosition (GameObject obj) {
		obj.transform.localPosition = originalPos;
		obj.transform.localRotation = originalRot;
	}

}