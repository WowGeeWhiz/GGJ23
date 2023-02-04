using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float cost, maxDurability, currentDurability;
    public bool broken;

    public bool targetInRange;

    public float damage, damageRate, damageTimer, damageDelay;

    float innerRadius, outerRadius;

    public GameObject pivot, buildingRadius;
    public Collider2D fireArea;

    private GameObject target;

    void Awake()
    {
        cost = 20;
        maxDurability = 50;
    }

    void Start()
    {
        damage = 2;

        currentDurability = 50;

        damageTimer = 0;
        damageDelay = 1 / damageRate;

        innerRadius = 0.1f;
        outerRadius = 3f;
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

        if (target != null && !broken)
        {
            Debug.Log("1");
            damageTimer -= Time.deltaTime;

            if (targetInRange)
            {
                Debug.Log("2");
                //pivot rotate to look at target

                if (damageTimer <= 0)
                {
                    damageTimer = damageDelay;

                }
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

    private void OnTriggerEnter(Collider c)
    {
        if (gameObject != null)
        {
            if (c.gameObject.name.Contains("Enemy"))
            {
                targetInRange = true;
                target = c.gameObject;
            }
        }

    }

   

}
