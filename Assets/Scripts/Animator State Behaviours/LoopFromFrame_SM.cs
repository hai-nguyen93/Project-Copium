using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopFromFrame_SM : StateMachineBehaviour
{
    [Tooltip("Where to go back and loop from there (t = desired_frame/ num_of_frames)")] 
    [Range(0f, 1f)] public float startFrameTimeNormalized = 0;
    [Range(0f, 1f)] public float endFrameTimeNormalized = 0.98f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= endFrameTimeNormalized)
        {
            animator.Play(stateInfo.fullPathHash, 0, startFrameTimeNormalized);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
