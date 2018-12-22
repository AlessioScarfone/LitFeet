using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HingeJoint))]
public class ActiveBridge : IActivableObject {

	private HingeJoint hingeJoint1;

    void Start () {
		hingeJoint1 = gameObject.GetComponent<HingeJoint>();
	}

    public override void ActiveObject() {
        Debug.Log("ACTIVE");
        hingeJoint1.useMotor = true;
    }


    public override void DisableObject() {
        Debug.Log("DISABLE");
        hingeJoint1.useMotor = false;
    }   
	
}
