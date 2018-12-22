using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveObject : MonoBehaviour {
	[Space (10)]
	[Tooltip ("Player need a key to active all the object connected")]
	public bool requireKey;
	[Tooltip ("List of object to active/deactive")]
	public IActivableObject[] objectToActive;
	[Tooltip ("Sound to play when you can't active object")]
	[SerializeField] private String sound_cant_active;

	[Tooltip ("Animator of activator object")]
	public Animator animator;

	private GameObject _player;

	private void Start () { }

	void OnTriggerEnter (Collider other) {
		if (other.tag == "Player") {
			// Debug.Log ("You can active something!");
			_player = other.gameObject;
			foreach (var obj in objectToActive) {
				Managers.BalloonManager.ShowBalloon (obj.BalloonTypeNeeded ());
			}
		}
		OnTriggerStay (other);
	}

	void OnTriggerStay (Collider other) {
		if (other.tag == "Player") {
			if (Input.GetKeyDown ("e")) {
				runAnimationActivator ();
				//check if a key is needed
				if (requireKey == true) {
					if (Managers.PlayerManager.GetKeyCount () > 0) {
						Managers.PlayerManager.RemoveKey ();
					} else { //player have 0 keys
						if (sound_cant_active != "")
							Managers.Audio.PlaySound (sound_cant_active);
						return;
					}
				}

				foreach (var obj in objectToActive) {
					PermittedOperations operation = obj.getOperation ();
					//object that need a key can be only actived and only one time
					if (requireKey && operation != PermittedOperations.OnlyActive) {
						operation = PermittedOperations.OnlyActive;
					}

					if ((operation == PermittedOperations.OnlyActive || operation == PermittedOperations.Active_Disable) && !obj.actived) {
						AutoRotate (obj, _player);
						// Managers.Audio.PlaySoundActived();
						bool actived = obj.ActiveObject ();
						if (actived && obj.sound_active != "") {
							Managers.Audio.PlaySound (obj.sound_active);
						}
						if (!actived && sound_cant_active != "")
							Managers.Audio.PlaySound (sound_cant_active);

						//if key is required: hide trigger of activator (so permit only one activation)
						if (requireKey && actived) {
							gameObject.GetComponent<BoxCollider> ().enabled = false;
							Managers.BalloonManager.HideBalloon (obj.BalloonTypeNeeded ());
						}

					} else if ((operation == PermittedOperations.OnlyDisable || operation == PermittedOperations.Active_Disable) && obj.actived) {
						AutoRotate (obj, _player);
						bool actived = obj.DeactivateObject ();
						if (actived && obj.sound_active != "") {
							Managers.Audio.PlaySound (obj.sound_active);
						}
						if (!actived && sound_cant_active != "")
							Managers.Audio.PlaySound (sound_cant_active);
					}
				}
			}
		}
	}

	void OnTriggerExit (Collider other) {
		if (other.tag == "Player") {
			foreach (var obj in objectToActive) {
				Managers.BalloonManager.HideBalloon (obj.BalloonTypeNeeded ());
			}
		}
	}

	private bool CheckLookAt (IActivableObject obj, GameObject player) {
		if (obj.needLookAt) {
			Vector3 _playerPos = player.transform.position;
			Vector3 _targetPos = transform.position;
			_targetPos.y = 0;
			_playerPos.y = 0;
			Vector3 _direction = (_targetPos - _playerPos).normalized;
			float dot = Vector3.Dot (player.transform.forward, _direction);
			// Debug.Log ("T:" + _targetPos + "-" + "P:" + _playerPos + "=" + _direction + " dot:" + dot);
			if (dot > 0.5f) {
				return true;
			} else {
				Debug.Log ("You need to look at the object");
				return false;
			}
		}
		return true;
	}

	private void AutoRotate (IActivableObject target, GameObject player) {
		if (target.needLookAt) {
			Vector3 _direction = transform.position - player.transform.position;
			_direction.y = 0;
			//create the rotation we need to be in to look at the target
			Quaternion _lookRotation = Quaternion.LookRotation (_direction);
			player.transform.rotation = _lookRotation;

			//FIXME : NOT WORK
			// player.transform.rotation = Quaternion.Slerp (player.transform.rotation, _lookRotation, Time.deltaTime);
		}
	}

	private void runAnimationActivator () {
		if (animator != null)
			animator.SetTrigger ("Active");
	}

}