using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour 
{
    public WeaponController wc;
    //public GameObject HitParticle;
    private List<IDamageable> Damageables = new List<IDamageable>();
    public delegate void AttackEvent(IDamageable target);
    public AttackEvent OnAttack;
    private Coroutine AttackCoroutine;

    public int damage = 1;

    void OnTriggerEnter(Collider other)
    {
        if(wc.IsAttacking)
        {
            //other.GetComponent<Animator>().SetTrigger("Hit");
            /*Instantiate(HitParticle, new Vector3(other.transform.position.x,
                transform.position.y, other.transform.position.z), 
                other.transform.rotation);*/

            IDamageable damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                Damageables.Add(damageable);

                if (AttackCoroutine == null)
                {
                    AttackCoroutine = StartCoroutine(Attack());
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            Damageables.Remove(damageable);
            if (Damageables.Count == 0)
            {
                StopCoroutine(AttackCoroutine);
                AttackCoroutine = null;
            }
        }
    }

    private IEnumerator Attack()
    {
        WaitForSeconds wait = new WaitForSeconds(0f);

        yield return wait;

        IDamageable closestDamageable = null;
        float closestDistance = float.MaxValue;

        while (Damageables.Count > 0)
        {
            for (int i = 0; i < Damageables.Count; i++) 
            {
                Transform damageableTransform = Damageables[i].GetTransform();
                float distance = Vector3.Distance(transform.position, damageableTransform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestDamageable = Damageables[i];
                }
            }

            if (closestDamageable != null)
            {
                OnAttack?.Invoke(closestDamageable);
                closestDamageable.TakeDamage(damage);
            }

            closestDamageable = null;
            closestDistance = float.MaxValue;

            yield return wait;

            Damageables.RemoveAll(DisabledDamageables);
        }

        AttackCoroutine = null;
    }

    private bool DisabledDamageables(IDamageable damageable)
    {
        return damageable != null && !damageable.GetTransform().gameObject.activeSelf;
    }

}
