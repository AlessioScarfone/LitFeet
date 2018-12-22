using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowTips : MonoBehaviour {

	public PossibleMessageText tip = PossibleMessageText.Null;
	
	public bool hideOnExit = false;
	[HideInInspector]
	public bool useTimer = false;
	[HideInInspector]
	public float timer = 5;

	private void OnTriggerEnter (Collider other) {
		if (other.tag == "Player" && tip != PossibleMessageText.Null && !useTimer) {
			Messenger<PossibleMessageText>.Broadcast (GameEvents.UPDATE_TIP, tip);
		} else if (other.tag == "Player" && tip != PossibleMessageText.Null && useTimer) {
			Messenger<PossibleMessageText,float>.Broadcast (GameEvents.UPDATE_TIP_TIMER, tip, timer);
		}
	}

	private void OnTriggerExit (Collider other) {
		//if the timer is used, this function is ignored
		if (other.tag == "Player" && tip != PossibleMessageText.Null && hideOnExit == true && !useTimer) {
			Messenger.Broadcast (GameEvents.HIDE_AND_RESET_TIP);
		}
	}

}