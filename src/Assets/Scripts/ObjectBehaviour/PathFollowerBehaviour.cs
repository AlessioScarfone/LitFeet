using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFollowerBehaviour : IActivableObject {

    // ------------
    // -- public --
    // ------------
    [Space (10)]
    [Tooltip ("Loop behaviour:\nif is true, when the object reach the end of the path start to move toward the first position else stop its movement")]
    public bool loop = true;
    // public float checkReachTargetDistance = 0.15f;
    public float speed = 2;
    [Tooltip ("Reset the object when is deactivate")]
    public bool resetAtDeactivation = false;
    [Tooltip ("Reset the object and block action when is deactivate")]
    public bool resetAndBlockAtDeactivation = false;
    public List<Vector3> path;

    // -------------
    // -- private --
    // -------------

    private int _currentIndex = 0;
    private Vector3 _currentTarget;
    // private Vector3 _currentDirection;

    //used for block the movement at the end of the path if 'loop' is set to FALSE
    private bool _stop = false;
    private bool _startReset = false;

    private void Start () {
        //block useless parameter behaviour
        needLookAt = false;

        _currentTarget = (path.Any () ? path.ElementAt (_currentIndex) : Vector3.zero);
        // _currentDirection = Vector3.Normalize (_currentTarget - transform.position);
    }

    private void UpdateTarget () {
        _currentIndex = (_currentIndex + 1) % path.Count;
        _currentTarget = path.ElementAt (_currentIndex);
        // _currentDirection = Vector3.Normalize (_currentTarget - transform.position);

        if (_currentIndex == 0 && !loop) {
            _stop = true;
        }
    }

    private void Update () {
        //OLD VERSION
        // if (actived && !_stop) {
        //     if (Vector3.Distance(_currentTarget, transform.position) < checkReachTargetDistance) {
        //         transform.position = _currentTarget;
        //         if(_startReset == true){
        //             _startReset = false;
        //             actived = false;
        //             if(resetAndBlockAtDeactivation)
        //                 _stop = true;
        //         }
        //         UpdateTarget();
        //     } else {
        //         transform.Translate(speed * Time.deltaTime * _currentDirection);
        //     }
        // }

        if (actived && !_stop) { 
            transform.position = Vector3.MoveTowards (gameObject.transform.position, _currentTarget, speed * Time.deltaTime);
            if (transform.position == _currentTarget) {
                transform.position = _currentTarget;
                if (_startReset == true) {
                    _startReset = false;
                    actived = false;
                    if (resetAndBlockAtDeactivation)
                        _stop = true;
                }
                UpdateTarget ();
            }
        }

    }

    public override bool ActiveObject () {
        actived = true;
        return true;
    }

    public override bool DeactivateObject () {
        if (resetAtDeactivation || resetAndBlockAtDeactivation) {
            Reset ();
            // Debug.Log("DIS");
        } else
            actived = false;

        return true;

    }

    public void Unblock () {
        _stop = false;
    }

    public void Reset () {
        _stop = false;
        _currentIndex = 0;
        _currentTarget = (path.Any () ? path.ElementAt (_currentIndex) : Vector3.zero);
        // _currentDirection = Vector3.Normalize (_currentTarget - transform.position);
        _startReset = true;
    }

    public override BalloonType BalloonTypeNeeded () {
        return BalloonType.Action;
    }
}