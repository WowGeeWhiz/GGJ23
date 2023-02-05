using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range;
    public bool broken;

    public bool targetInRange;
    public bool isSpinning;

    public float attackDelay, damage, currentDamage;
    float lastAttack;

    float innerRadius = 0.1f, outerRadius = 0.5f;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;

    void Start()
    {
        currentDurability = maxDurability;
        healthBar.SetHealth(currentDurability, maxDurability);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentDurability <= 0) broken = true;
        else broken = false;

        if (Time.fixedTime >= lastAttack + attackDelay)
        {
            currentDamage = damage;
            lastAttack = Time.fixedTime;
        }
        else currentDamage = 0;
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

        healthBar.SetHealth(currentDurability, maxDurability);
    }

}
