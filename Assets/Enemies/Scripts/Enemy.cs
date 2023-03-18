using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// this class contains references to the GroundEnemy behaviour and is to be called by
// the ObjectPool
public class Enemy : PoolableObject, IDamageable

{
    public AttackRadius attackRadius;
    public Animator animator;
    public EnemyMovement movement;
    public NavMeshAgent agent;
    public EnemyScriptableObject enemyScriptableObject;
    public int health = 1;

    private Coroutine LookCoroutine;
    private const string ATTACK_TRIGGER = "Attack";
    private void Awake()
    {
        attackRadius.OnAttack += OnAttack;
    }

    private void OnAttack(IDamageable target)
    {
        animator.SetTrigger(ATTACK_TRIGGER);

        if (LookCoroutine != null)
        {
            StopCoroutine(LookCoroutine);
        }

        LookCoroutine = StartCoroutine(LookAt(target.GetTransform()));
    }

    private IEnumerator LookAt(Transform target)
    {
        Quaternion lookRotation = Quaternion.LookRotation(target.position - transform.position);
        float time = 0;

        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, time);

            time += Time.deltaTime * 2;
            yield return null;
        }
        transform.rotation = lookRotation;
    }
    public virtual void OnEnable()
    {
        SetUpAgentFromConfiguration();
    }

    public override void OnDisable()
    {
        base.OnDisable();

        agent.enabled = false;
    }

    // maybe switch to object compsition in future with more enemies and more behaviors
    public virtual void SetUpAgentFromConfiguration()
    {
        agent.acceleration = enemyScriptableObject.acceleration;
        agent.angularSpeed = enemyScriptableObject.angularSpeed;
        agent.areaMask = enemyScriptableObject.areaMask;
        agent.avoidancePriority = enemyScriptableObject.avoidancePriority;
        agent.baseOffset = enemyScriptableObject.baseOffset;
        agent.height = enemyScriptableObject.height;
        agent.obstacleAvoidanceType = enemyScriptableObject.obstacleAvoidanceType;
        agent.radius = enemyScriptableObject.radius;
        agent.speed = enemyScriptableObject.speed;
        agent.stoppingDistance = enemyScriptableObject.stoppingDistance;

        movement.updateSpeed = enemyScriptableObject.AIUpdateInterval;

        // reset health when enemy dies
        health = enemyScriptableObject.health;

        (attackRadius.Collider == null ? attackRadius.GetComponent<SphereCollider>() : attackRadius.Collider).radius = enemyScriptableObject.attackRadius;
        attackRadius.attackDelay = enemyScriptableObject.attackDelay;
        attackRadius.damage = enemyScriptableObject.damage;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
