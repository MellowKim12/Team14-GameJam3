using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedAttackRadius : AttackRadius
{
    public NavMeshAgent agent;
    public Bullet bulletPrefab;
    public Vector3 BulletSpawnOffset = new Vector3(0, 1, 0);
    public LayerMask mask;
    private ObjectPool bulletPool;
    [SerializeField] private float spherecastRadius = 0.1f;
    private RaycastHit hit;
    private IDamageable targetDamageable;
    private Bullet bullet;

    protected override void Awake()
    {
        base.Awake();

        bulletPool = ObjectPool.CreateInstance(bulletPrefab, Mathf.CeilToInt((1 / attackDelay) * bulletPrefab.autoDestroyTime));
    }

    protected override IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(attackDelay);

        yield return wait;

        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++)
            {
                if (HasLineOfSightTo(Damageables[i].GetTransform()))
                {
                    targetDamageable = Damageables[i];
                    OnAttack?.Invoke(Damageables[i]);
                    agent.enabled = false;
                    break;
                }
            }

            if (targetDamageable != null)
            {
                PoolableObject poolableObject = bulletPool.GetObject();
                if (poolableObject != null)
                {
                    bullet = poolableObject.GetComponent<Bullet>();

                    bullet.damage = damage;
                    bullet.transform.position = transform.position + BulletSpawnOffset;
                    bullet.transform.rotation = agent.transform.rotation;
                    bullet.rb.AddForce(agent.transform.forward * bulletPrefab.moveSpeed, ForceMode.VelocityChange);
                }
            }
            else
            {
                agent.enabled = true; // no target in line of sight, try to get closer
            }

            yield return wait;

            if (targetDamageable == null || !HasLineOfSightTo(targetDamageable.GetTransform()))
            {
                agent.enabled = true;
            }

            Damageables.RemoveAll(DisabledDamageables);
        }

        agent.enabled = true;
        AttackCoroutine = null;
    }

    private bool HasLineOfSightTo(Transform target)
    {
        if (Physics.SphereCast(transform.position + BulletSpawnOffset, spherecastRadius, ((target.position + BulletSpawnOffset) - (transform.position + BulletSpawnOffset)).normalized, out hit, Collider.radius, mask))
        {
            IDamageable damageable;
            if (hit.collider.TryGetComponent<IDamageable>(out damageable))
            {
                return damageable.GetTransform() == target;
            }   
        }

        return false;
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (AttackCoroutine == null)
        {
            agent.enabled = true;
        }
    }
}
