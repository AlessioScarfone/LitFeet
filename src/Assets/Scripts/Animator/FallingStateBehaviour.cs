using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingStateBehaviour : StateMachineBehaviour {

    private GameObject _player;
    private CandleInputController _candleInputController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _player = GameObject.FindGameObjectWithTag ("Player");
        _candleInputController = _player.GetComponent<CandleInputController> ();
       
        _candleInputController.SetBlockMovement (true);
        _candleInputController.playLandingSound();
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (_candleInputController.IsDamaged ()) {
            animator.SetTrigger ("Dying");
            Messenger.Broadcast(GameEvents.DAMAGE);
        }
        else{
            //if start dying animation, not unblock the movement
             _candleInputController.SetBlockMovement (false);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}