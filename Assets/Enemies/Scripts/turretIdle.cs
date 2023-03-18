using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretIdle : StateMachineBehaviour
{
    Transform player;

    [Header("Parameters")]
    public float range;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        if (distance <= range)
        {
            animator.SetBool("inRange", true);
        }
    }
}

