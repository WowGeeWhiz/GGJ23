using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, attackDelay, damage, currentDamage;
    public bool broken;
    float lastAttack;

    public GameObject pivot, buildingRadius;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;

    void Start()
    {
        currentDurability = maxDurability;

        healthBar.SetHealth(currentDurability, maxDurability);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDurability <= 0) broken = true;
        else broken = false;

        if (!broken)
        {
            if (Time.fixedTime >= lastAttack + attackDelay)
            {
                lastAttack = Time.deltaTime;
                currentDamage = damage;
            }
            else currentDamage = 0;
        }
        else this.gameObject.SetActive(false);
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
