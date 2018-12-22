using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PermittedOperations { OnlyActive, OnlyDisable, Active_Disable }
public abstract class IActivableObject : MonoBehaviour {


	[SerializeField] private PermittedOperations operation;

	public bool actived = false;
	public bool needLookAt = false;

	public PermittedOperations getOperation() { return operation; }

	public abstract void ActiveObject ();

	public abstract void DeactivateObject ();

}