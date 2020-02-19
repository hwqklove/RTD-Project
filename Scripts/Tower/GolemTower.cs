using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Golem is a tower that attacks all enemies in range
/// </summary>
public class GolemTower : MonoBehaviour
{
    public Animator ani;
    public float Range;
    public GameObject target;

    public float Damage;
    public GameObject Splash;

    public void Start()
    {
        ani = GetComponent<Animator>();
        InvokeRepeating("UpdateTarget", 0f, 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, Range);
    }

    // Find Target & Find new Target when monster is out of range
    public void UpdateTarget()
    {
        if (target == null)
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
                target = nearestMonster;
                Attack();
            }
            else
            {
                Idle();
            }
        }
        else
        {
            target = null;
        }
    }

    public void Attack()
    {
        ani.SetInteger("TowerAniState", 2);
    }
    public void Idle()
    {
        ani.SetInteger("TowerAniState", 1);
    }

    public void SplashDamage()
    {
        GameObject splashD = Instantiate(Splash, new Vector3(transform.position.x, transform.position.y - 0.4f, transform.position.z), Quaternion.identity);
        splashD.GetComponent<Splash>().Damage = Damage;
        Destroy(splashD, 1f);
    }
}
