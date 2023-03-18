using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(AgentLinkMover))]
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    public float updateSpeed = 0.1f;

    [SerializeField] private Animator animator;

    private NavMeshAgent agent;
    private AgentLinkMover linkMover;

    private const string isWalking = "IsWalking";
    private const string jump = "Jump";
    private const string landed = "Landed";

    private Coroutine followCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        linkMover = GetComponent<AgentLinkMover>();

        linkMover.onLinkEnd += HandleLinkEnd;
        linkMover.onLinkStart += HandleLinkStart;
   
    }
    
    public void StartChasing()
    {
        if (followCoroutine == null)
        {
            followCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on an Enemy that is already chasing");
        }
    }

    private void HandleLinkStart()
    {
        animator.SetTrigger(jump);
    }

    private void HandleLinkEnd()
    {
        animator.SetTrigger(landed);
    }

    private void Update()
    {
        animator.SetBool(isWalking, agent.velocity.magnitude > 0.01f);
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (enabled)
        {
            agent.SetDestination(target.transform.position);

            yield return wait;
        }
    }
}
