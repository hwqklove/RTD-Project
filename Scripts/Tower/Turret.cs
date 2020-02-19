using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Turret is tower that attacks single target
/// Makes the head part look at the specified target
/// </summary>
public class Turret : MonoBehaviour
{
    public int Damage;
    public float Range;
    public GameObject Target;
    public Animator ani;
    public Transform PartToRotate;

    public void Start ()
    {
        ani = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    public void Update ()
    {
        LookAtTarget();
    }

    private void OnDrawGizmosSelected ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    public void UpdateTarget ()
    {
        if (Target == null)
        {
            GameObject[] Monsters = GameObject.FindGameObjectsWithTag("Monster");
            float shortestDistance = Mathf.Infinity;
            GameObject nearestMonster = null;

            foreach (GameObject Monster in Monsters)
            {
                float DistanceToMonster = Vector3.Distance(transform.position, Monster.transform.position);

                if (DistanceToMonster < shortestDistance)
                {
                    shortestDistance = DistanceToMonster;
                    nearestMonster = Monster;
                }
            }

            if (nearestMonster != null && shortestDistance <= Range)
            {
                Target = nearestMonster;
                Attack();
            }
            else
            {
                ResetTower();
                Idle();
            }
        }
        else if (Target != null)
        {
            float DistanceToMonster = Vector3.Distance(transform.position, Target.transform.position);
            if (DistanceToMonster > Range) Target = null;
        }
    }

    public void Attack ()
    {
        ani.SetInteger("TowerAniState", 2);
    }

    public void Idle ()
    {
        ani.SetInteger("TowerAniState", 1);
    }

    public void AttackTarget ()
    {
        Target.GetComponent<Monster>().GetDamage(Damage);
    }

    void LookAtTarget ()
    {
        if (Target == null) return;
        Vector3 dir = Target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = lookRotation.eulerAngles;
        PartToRotate.rotation = Quaternion.Euler(0f, rotation.y-90, 0f);
    }

    void ResetTower ()
    {
        PartToRotate.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}
