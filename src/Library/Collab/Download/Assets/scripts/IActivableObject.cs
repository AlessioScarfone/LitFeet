using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PermittedOperations { OnlyActive, OnlyDisable, Active_Disable }
public abstract class IActivableObject : MonoBehaviour {

	[Tooltip("Set the possible actions on the object")]
	[SerializeField] private PermittedOperations operation;
 	
	[Tooltip("Set the state of 'Active' of the object")]
	public bool actived = false;
	
	[Tooltip("At the time of activation the player is rotated towards the object (not significant for some components)")]
	public bool needLookAt = false;

	public PermittedOperations getOperation() { return operation; }

	public abstract void ActiveObject ();

	public abstract void DeactivateObject ();

}