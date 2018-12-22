using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PermittedOperations { OnlyActive, OnlyDisable, Active_Disable }

public abstract class IActivableObject : MonoBehaviour {
 	[Header("IActivableObject")]
	[Tooltip("Set the possible actions on the object")]
	[SerializeField] private PermittedOperations operation;
 	
	[Tooltip("Set the state of 'Active' of the object")]
	public bool actived = false;
	
	[Tooltip("At the time of activation the player is rotated towards the object (not significant for some components)")]
	public bool needLookAt = false;

	[Tooltip("Sound to play when you active object")]
	public string sound_active;

	public PermittedOperations getOperation() { return operation; }

	public abstract bool ActiveObject ();

	public abstract bool DeactivateObject ();

	public abstract BalloonType BalloonTypeNeeded();

}