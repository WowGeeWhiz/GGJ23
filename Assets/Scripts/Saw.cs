using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float cost, maxDurability, currentDurability;
    public bool broken;

    public bool targetInRange;
    public bool isSpinning;

    public float damage, damageRate, damageTimer, damageDelay;

    float innerRadius, outerRadius;

    void Awake()
    {
        cost = 20;
        maxDurability = 50;
    }

    void Start()
    {
        damage = 1;

        currentDurability = 50;

        damageTimer = 0;
        damageDelay = 1 / damageRate;

        innerRadius = 0.1f;
        outerRadius = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDurability <= 0)
        {
            broken = true;
        }
        else if (currentDurability > 0)
        {
            broken = false;
        }

        if (targetInRange && !broken)
        {
            damageTimer -= Time.deltaTime;
            if (damageTimer <= 0)
            {
                damageTimer = damageDelay;

                DealDamage();
            }
        }

    }

    public void changeDurability(float amount)
    {
        currentDurability += amount;

        if (currentDurability > maxDurability)
        {
            currentDurability = maxDurability;
        }

        if (currentDurability < 0)
        {
            currentDurability = 0;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Enemy"))
        {
            changeDurability(0 - Random.Range(5, 10));
            if (!broken)
            {
                targetInRange = true;
            }
        }
    }

    public void DealDamage()
    {
        //if any enemies in outer radius, enemy health = enemy health -5;
        Collider[] enemiesInRange = Physics.OverlapSphere(this.gameObject.transform.position, outerRadius);
        foreach (Collider col in enemiesInRange)
        {
            if (col.gameObject.name.Contains("Enemy"))
            {
                Debug.Log(damage + " Damage dealt to enemy");
            }
        }


    }

}
