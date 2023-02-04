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

    void Awake()
    {
        cost = 20;
        maxDurability = 50;
    }

    void Start()
    {
        currentDurability = 50;

        damageTimer = 0;
        damageDelay = 1 / damageRate;
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
        if (collision.gameObject.tag == "Enemy")
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
        
    }

}
