using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public static Monster instance;

    [SerializeReference]
    public float Health;

    private void Start ()
    {
        instance = this;
        if ((int)Spawner.instance.timeSet == 0)
        {
            Health += Spawner.instance.tempHealth;
        }
    }

    private void Update ()
    {
    }

    public void GetDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Splash")
        {
            GetDamage(other.GetComponent<Splash>().Damage);
        }
    }
}
