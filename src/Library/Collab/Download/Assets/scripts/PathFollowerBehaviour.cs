using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFollowerBehaviour : IActivableObject {

    // ------------
    // -- public --
    // ------------

    public List<Vector3> path;
    public float speed = 2;
    public float checkReachTargetDistance = 0.1f;
    [Tooltip("Loop behaviour:\nif is true, when the object reach the end of the path start to move toward the first position else stop its movement")]
    public bool loop = true;

    // -------------
    // -- private --
    // -------------

    private int _currentIndex = 0;
    private Vector3 _currentTarget;
    private Vector3 _currentDirection;
    
    //used for block the movement at the end of the path if 'loop' is set to FALSE
    private bool _stop = false;


    private void Start() {
        //block useless parameter behaviour
        needLookAt = false;

        _currentTarget = (path.Any() ? path.ElementAt(_currentIndex) : Vector3.zero);
        _currentDirection = Vector3.Normalize(_currentTarget - transform.position);
    }

    private void UpdateTarget() {
        _currentIndex = (_currentIndex + 1) % path.Count;
        _currentTarget = path.ElementAt(_currentIndex);
        _currentDirection = Vector3.Normalize(_currentTarget - transform.position);

        if(_currentIndex == 0 && !loop) {
            _stop = true;
        }
    }

    private void Update() {
        if (actived && !_stop) {
            if (Vector3.Distance(_currentTarget, transform.position) < checkReachTargetDistance) {
                transform.position = _currentTarget;
                UpdateTarget();
            } else {
                transform.Translate(speed * Time.deltaTime * _currentDirection);
            }
        }
    }

    public override void ActiveObject()
    {
        actived = true;
    }

    public override void DeactivateObject()
    {
        actived = false;
    }

    public void Reset(){
        _stop = false;
    }
}
