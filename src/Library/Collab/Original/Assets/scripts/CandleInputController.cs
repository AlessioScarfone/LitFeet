using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleInputController : MonoBehaviour {

    // ------------
    // -- public --
    // ------------

    public float walkingSpeed;
    public float runningSpeed;

    // -------------
    // -- private --
    // -------------

    private Animator animator;
    private CharacterController characterController;

    private bool grounded = true;
    private bool isWalking = false;
    private bool startLanding = false;
    private bool blockMovement = false;
    private Vector3 currentFacing;

    private float pushingForce = 1;

    private class CandleFacings {
        public static Vector3 north = new Vector3 (1, 0, 0);
        public static Vector3 west = new Vector3 (0, 0, 1);
        public static Vector3 south = new Vector3 (-1, 0, 0);
        public static Vector3 east = new Vector3 (0, 0, -1);

        private CandleFacings () { }
    }

    private Vector3 InputToFacing () {
        Vector3 movement = new Vector3 ();

        if (Input.GetKey (KeyCode.A) || (Input.GetKey (KeyCode.LeftArrow))) { movement += CandleFacings.north; }
        if (Input.GetKey (KeyCode.S) || (Input.GetKey (KeyCode.DownArrow))) { movement += CandleFacings.west; }
        if (Input.GetKey (KeyCode.D) || (Input.GetKey (KeyCode.RightArrow))) { movement += CandleFacings.south; }
        if (Input.GetKey (KeyCode.W) || (Input.GetKey (KeyCode.UpArrow))) { movement += CandleFacings.east; }

        return Vector3.ClampMagnitude (movement, 1);
    }

    private bool RunningRequested() {
        return Input.GetKey(KeyCode.LeftShift);
    }

    private void Start () {
        animator = GetComponent<Animator> ();
        characterController = GetComponent<CharacterController> ();

        transform.rotation = Quaternion.LookRotation (CandleFacings.west);
    }

    private void Update () {
        currentFacing = InputToFacing ();

        if (!characterController.isGrounded && grounded) { //start falling
            grounded = false;
            animator.SetBool ("IsFalling", true);
            // Debug.Log ("START FALLING");
        } else if (characterController.isGrounded) {
            startLanding = false;
            if (!grounded) {
                grounded = true;
                // animator.SetTrigger ("Stop Falling");
            } else {
                // update candle's state according to the current input
                if (currentFacing == Vector3.zero && isWalking) {
                    //STOP WALKING
                    isWalking = false;
                    animator.SetInteger("MovementType", 0);
                } else if (currentFacing != Vector3.zero && !isWalking) {
                    //START WALKING
                    isWalking = true;
                }
                if (!blockMovement) {
                    //animator.SetBool ("IsWalking", isWalking);
                    // orient candle and translate player if candle is still walking
                    if (isWalking) {
                        float currentSpeed = RunningRequested() ? runningSpeed : walkingSpeed;
                        int movementType = RunningRequested() ? 2 : 1;

                        animator.SetInteger("MovementType", movementType);
                        transform.rotation = Quaternion.LookRotation (currentFacing);
                        //characterController.Move (walkingSpeed * Time.deltaTime * currentFacing);
                        characterController.Move (currentSpeed * Time.deltaTime * currentFacing);
                    }
                }
            }

        } else if (!characterController.isGrounded) {
            if (startLanding == false && ShotRaycast ()) {
                startLanding = true;
                // Debug.Log ("STOP FALLING");
                animator.SetBool ("IsFalling", false);
            }
        }
        // apply gravity no matter what for now
        characterController.Move (new Vector3 (0, -.5f, 0));
    }

    private void OnControllerColliderHit (ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic && hit.gameObject.tag == "Pushable") {
            float mass = body.mass;
            if(RunningRequested())
                pushingForce*=1.2f;
            Vector3 force = ((hit.moveDirection * pushingForce) / mass);
            Debug.Log ("Hit a pushable obj and add a force of:" + currentFacing);
            body.velocity = force;
            // body.AddForce(force/mass,ForceMode.Force);
        }
    }

    private bool ShotRaycast () {
        RaycastHit hit;
        bool hitGround = false;
        if (Physics.Raycast (transform.position, Vector3.down, out hit)) {
            float check = (characterController.height + characterController.radius) / 0.4f;
            hitGround = (hit.distance <= check);

            // if (hitGround){
                // Debug.DrawRay(transform.position,  Vector3.forward, Color.green,5);
                // Debug.Log ("OK - hit dist:" + hit.distance + "  check:" + check);
            // }
        }
        return hitGround;
    }

    public void SetBlockMovement (bool val) {
        blockMovement = val;
    }

}
