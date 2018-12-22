using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DyingStateBehaviour : StateMachineBehaviour {

    private GameObject _player;
    private CandleInputController _candleInputController;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _player = GameObject.FindGameObjectWithTag ("Player");
        if (_player != null) {
            _candleInputController = _player.GetComponent<CandleInputController> ();
            _candleInputController.SetBlockMovement (true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (_player != null) {
            _candleInputController.SetDamaged (false);
            _candleInputController.SetBlockMovement (false);
            Managers.PersistenceManager.Restart ();
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