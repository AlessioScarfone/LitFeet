using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (HingeJoint))]
public class ActiveBridge : IActivableObject {

    private HingeJoint _hingeJoint;

    private bool reachLimitActive = false;
    private bool reachLimitDeactive = false;

    void Start () {
        _hingeJoint = gameObject.GetComponent<HingeJoint> ();
    }

    public override bool ActiveObject () {
        // Debug.Log("ACTIVE");
        _hingeJoint.useMotor = true;
        actived = true;
        reachLimitDeactive = false;
        return true;
    }

    private void Update () {
        if (_hingeJoint.useMotor == true && !reachLimitActive) {
            checkAngleActive ();
        }
        else if (_hingeJoint.useMotor == false && !reachLimitDeactive) {
            checkAngleDeactive ();
        }
    }

    void checkAngleActive () {
        if (_hingeJoint.angle >= _hingeJoint.limits.max-3) {
            // Debug.Log ("Stop");
            Managers.Audio.StopSound ();
            reachLimitActive = true;
            
        }
    }

     void checkAngleDeactive () {
        if (_hingeJoint.angle <= _hingeJoint.limits.min+3) {
            // Debug.Log ("Stop");
            Managers.Audio.StopSound ();
            reachLimitDeactive = true;
        }
    }

    public override bool DeactivateObject () {
        // Debug.Log("DISABLE");
        _hingeJoint.useMotor = false;
        actived = false;
        reachLimitActive = false;
        return true;
    }

    public override BalloonType BalloonTypeNeeded () {
        return BalloonType.Action;
    }
}