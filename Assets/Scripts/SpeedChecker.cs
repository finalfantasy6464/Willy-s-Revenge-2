using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedChecker : StateMachineBehaviour
{
    BigOrange orangescript;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        orangescript = animator.GetComponent<BigOrange>();

        if (stateInfo.IsName("Stomp") && orangescript.previousstate == "Stomp")
        {
            orangescript.stompspeedindex = Mathf.Min(2, orangescript.stompspeedindex + 1);
        }

        else
        {
            orangescript.stompspeedindex = 0;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName("Stomp"))
        {
            orangescript.previousstate = "Stomp";
        }
          
        else if (stateInfo.IsName("Jump"))
        {
            orangescript.previousstate = "Jump";
        }
           
        else if (stateInfo.IsName("LeftFistSlam"))
        {
            orangescript.previousstate = "LeftFistSlam";
        }
       
        else if (stateInfo.IsName("RightFistSlam"))
        {
            orangescript.previousstate = "RightFistSlam";
        }

        else if (stateInfo.IsName("Damage"))
        {
            orangescript.previousstate = "Damage";
        }

    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
