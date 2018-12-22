using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleInputController : MonoBehaviour {

    private const int dangerHeight = 18;

    // ------------
    // -- public --
    // ------------

    public float walkingSpeed;
    public float runningSpeed;

    // -------------
    // -- private --
    // -------------

    private Animator _animator;
    private CharacterController _characterController;

    private bool _blockMovement = false;
    private bool _damaged = false;

    private enum PlayerState { Idle, Landing, Moving, Falling }
    private PlayerState _playerState = PlayerState.Idle;

    private Vector3 _currentFacing;

    private float _pushingForce = 1;


    // -------------
    // -- audio ----
    // -------------
    [SerializeField] public AudioClip landingSound;
    [SerializeField] public AudioClip footStepSound;
    [SerializeField] private float _footStepSoundLength;
    [SerializeField] private bool _step;

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

    private bool RunningRequested () {
        return Input.GetKey (KeyCode.LeftShift);
    }

    private void Start () {
        _animator = GetComponent<Animator> ();
        _characterController = GetComponent<CharacterController> ();

        transform.rotation = Quaternion.LookRotation (CandleFacings.west);
        _footStepSoundLength = 0.40f;
        _step = true;
        
    }
    

    public void PlaySoundMoving(float _ft)
    {
        Managers.Audio.PlaySound(footStepSound);
        StartCoroutine(WaitForFootSteps(_ft));
    }


IEnumerator WaitForFootSteps(float stepsLength)
    {
        _step = false;
        yield return new WaitForSeconds(stepsLength);
        _step = true;
    }

    private void Update () {
       


        _currentFacing = InputToFacing ();
        float playerHeight = _characterController.height;
        float rayDistance = ShotRaycast ();

        checkDamage (rayDistance);

        if (rayDistance <= playerHeight && rayDistance > 0.4) {
            _animator.SetBool ("IsFalling", false);
            _playerState = PlayerState.Landing;
        } else if (rayDistance > playerHeight) {
            _playerState = PlayerState.Falling;
            _animator.SetBool ("IsFalling", true);
           
        } else {
            if (_blockMovement == false) {
                if (_currentFacing == Vector3.zero && _playerState == PlayerState.Moving) {
                    //STOP WALKING
                    _playerState = PlayerState.Idle;
                    _animator.SetInteger ("MovementType", 0);
                } else if (_currentFacing != Vector3.zero && _playerState != PlayerState.Moving) {
                    //START WALKING
                    _playerState = PlayerState.Moving;
                   
                }
                if (_playerState == PlayerState.Moving) {
                    LogicMovement ();
                   
                }
            } else {
                _animator.SetInteger ("MovementType", 0);
                _playerState = PlayerState.Idle;
            }
        }

        // apply gravity no matter what for now
        if (Time.timeScale != 0)
            _characterController.Move (new Vector3 (0, -0.5f, 0));

      /*  if (_characterController.velocity.magnitude > 1f && _step)
        {
            Managers.Audio.PlaySound(footStepSound);
            StartCoroutine(WaitForFootSteps(_footStepSoundLength));
        }
        */
    }

    private bool checkDamage (float rayDistance) {
        if (rayDistance > dangerHeight) {
          
            _damaged = true;
        }
        return _damaged;
    }

    private void LogicMovement () {
        float currentSpeed = RunningRequested () ? runningSpeed : walkingSpeed;
        int movementType = RunningRequested () ? 2 : 1;
        _footStepSoundLength = RunningRequested() ? 0.3f : 0.4f;
        _animator.SetInteger ("MovementType", movementType);
        if (Time.timeScale != 0)
            transform.rotation = Quaternion.LookRotation (_currentFacing);
        //characterController.Move (walkingSpeed * Time.deltaTime * currentFacing);
        _characterController.Move (currentSpeed * Time.deltaTime * _currentFacing);
      
        if(_step) {
            Managers.Audio.PlaySound(footStepSound);
            StartCoroutine(WaitForFootSteps(_footStepSoundLength));
        }
    }

    private void OnControllerColliderHit (ControllerColliderHit hit) {
        Rigidbody body = hit.collider.attachedRigidbody;
        if (body != null && !body.isKinematic && hit.gameObject.tag == "Pushable") {
            // hit.gameObject.GetComponent<ConstantForce>().enabled=true;
            float mass = body.mass;
            // if(RunningRequested())
            //     _pushingForce*=1.2f;
            Vector3 force = ((hit.moveDirection * _pushingForce) / mass);
            // body.velocity = force;
            // Debug.Log ("Hit a pushable obj and add a force of:" + force);
            body.AddForce (force, ForceMode.Impulse);
            // hit.transform.Translate(force * Time.deltaTime, Space.World );
        }

    }

    private float ShotRaycast () {
        if (_characterController.isGrounded == true) {
            return 0;
        }
        RaycastHit hit;
        Vector3 pos = transform.position;
        pos.y += _characterController.radius;
        if (Physics.SphereCast (pos, _characterController.radius, Vector3.down, out hit)) {
            // float check = (_characterController.height + _characterController.radius) / 0.6f;
            // if (hitGround) {
            // Debug.DrawRay( pos, Vector3.down, Color.red, 5);
            // Debug.Log ("hit dist:" + hit.distance);
            return hit.distance;
        }
        return -1.0f;
    }

    public void SetBlockMovement (bool val) {
        _blockMovement = val;
    }

    public bool GetBlockMovement () {
        return _blockMovement;
    }

    public void SetDamaged (bool value) {
        _damaged = value;
    }

    public bool IsDamaged () {
        return _damaged;
    }

   public void playLandingSound()
    {
        Managers.Audio.PlaySound(landingSound);
    }
}