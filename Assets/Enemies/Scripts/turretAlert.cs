using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class turretAlert : StateMachineBehaviour
{
    Transform player, barrel, head;

    [Header("Parameters")]
    public float range;
    public GameObject projectile;
    public float fireRate, nextFire, power;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        head = animator.gameObject.transform;
        GameObject temp = animator.gameObject;
       // barrel = temp.FindGameObjectWithTag("Barrel").transform;

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(player.position, animator.transform.position);

        head.LookAt(player);
        if (Time.time >= nextFire)
        {
            nextFire = Time.time + 1f / fireRate;
            shoot(animator);
        }


        if (distance >= range)
        {
            animator.SetBool("inRange", false);
        }
    }
        
    void shoot(Animator animator)
    {
        GameObject clone = Instantiate(projectile, barrel.position, animator.transform.rotation);
        clone.GetComponent<Rigidbody>().AddForce(animator.transform.forward * power);
        Destroy(clone, 10);
    }
}


