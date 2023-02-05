using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, towerHealAmt, healCostInWood;
    public bool broken;

    public int repeatsPerCost;
    private int repeatsLeft;

    public PlayerController player;

    public bool targetInRange;
    public bool isSpinning;

    public float attackDelay, damage, currentDamage;
    float lastAttack;

    public GameObject workingTower, brokenTower;

    float innerRadius = 0.1f, outerRadius = 0.5f;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;
    public TextMeshProUGUI sawCost;

    void Start()
    {
        if (gameObject.CompareTag("permanentSaw")) sawCost.text = cost.ToString();
        currentDurability = maxDurability;
        healthBar.SetHealth(currentDurability, maxDurability);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentDurability <= 0)
        {
            brokenTower.gameObject.SetActive(true);
            workingTower.gameObject.SetActive(false);
            broken = true;
        }
        else broken = false;

        if (!broken)
        {
            brokenTower.gameObject.SetActive(false);
            workingTower.gameObject.SetActive(true);
            if (Time.fixedTime >= lastAttack + attackDelay)
            {
                lastAttack = Time.deltaTime;
                currentDamage = damage;
            }
            else currentDamage = 0;
        }
    }

    public void changeDurability(float amount = 0)
    {
        if (amount == 0) amount = towerHealAmt;

        currentDurability += amount;

        if (currentDurability > maxDurability) currentDurability = maxDurability;

        if (currentDurability <= 0)
        {
            currentDurability = 0;
            gameObject.tag = "BrokenTower";
        }
        else gameObject.tag = "Tower";

        healthBar.SetHealth(currentDurability, maxDurability);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood && currentDurability != maxDurability)
            {
                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                changeDurability();
                repeatsLeft--;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood && currentDurability != maxDurability)
            {
                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                changeDurability();
                repeatsLeft--;
            }
        }
    }

}
