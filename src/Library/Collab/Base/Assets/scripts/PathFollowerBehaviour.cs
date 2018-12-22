using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFollowerBehaviour : MonoBehaviour {

    // ------------
    // -- public --
    // ------------

    public List<Vector3> path;
    public float speed;
    public float landingDistance;
    public bool moveAtStart;

    public bool TogglePathFollowing() {
        return toggled = !toggled;
    }

    // -------------
    // -- private --
    // -------------

    private bool toggled;
    private int currentIndex;
    private Vector3 currentTarget;
    private Vector3 currentDirection;

    private void UpdateTarget() {
        currentIndex = (currentIndex + 1) % path.Count;
        currentTarget = path.ElementAt(currentIndex);
        currentDirection = Vector3.Normalize(currentTarget - transform.position);
    }

    private void Start() {
        toggled = moveAtStart;
        currentIndex = 0;
        currentTarget = (path.Any() ? path.ElementAt(currentIndex) : Vector3.zero);
        currentDirection = Vector3.Normalize(currentTarget - transform.position);
    }

    private void Update() {
        if (toggled) {
            if (Vector3.Distance(currentTarget, transform.position) < landingDistance) {
                transform.position = currentTarget;
                UpdateTarget();
            } else {
                transform.Translate(speed * Time.deltaTime * currentDirection);
            }
        }
    }

}
