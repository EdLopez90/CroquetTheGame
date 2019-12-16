using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingAnimator : StateMachineBehaviour {
    
    private int triggerHash;
    private int backSwingHash;
    private int followThroughHash;
    private int newMinHash;
    private int newMaxHash;

    private HingeJoint hinge;
    private bool isActive;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        this.triggerHash = Animator.StringToHash("Switch State");

        this.backSwingHash = Animator.StringToHash("Base Layer.Back Swing");
        this.followThroughHash = Animator.StringToHash("Base Layer.Follow Through");

        this.newMinHash = Animator.StringToHash("NewMinAngle");
        this.newMaxHash = Animator.StringToHash("NewMaxAngle");

        this.hinge = animator.GetComponentInParent<HingeJoint>();
        
	}

	// OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        
        if (Input.GetButton("Fire1"))
        {
            if (stateInfo.fullPathHash == backSwingHash)
            {
                //Ensures the Min angle is negative
                animator.SetFloat(newMinHash, (0 - Mathf.Abs(hinge.angle)));
            }
            else if (stateInfo.fullPathHash == followThroughHash)
            {
                //Ensures the Max angle is positive
                animator.SetFloat(newMaxHash, Mathf.Abs(hinge.angle));
            }
            animator.SetTrigger(triggerHash);
            
        }
	}

	// OnStateExit is called before OnStateExit is called on any state inside this state machine
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called before OnStateMove is called on any state inside this state machine
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called before OnStateIK is called on any state inside this state machine
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMachineEnter is called when entering a statemachine via its Entry Node
	//override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash){
	//
	//}

	// OnStateMachineExit is called when exiting a statemachine via its Exit Node
	//override public void OnStateMachineExit(Animator animator, int stateMachinePathHash) {
	//
	//}
}
