using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public PlayerController player;
    public float healthAmt, healCostInWood;
    private int repeatsLeft;
    public int repeatsPerCost;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        repeatsLeft = repeatsPerCost;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) player.lockMovement = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void HealHouse()
    {
        currentHealth += healthAmt;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood)
            {
                //Debug.Log("House in player attackBox and has enough to repair");
                
                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                HealHouse();
                repeatsLeft--;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood)
            {

                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                HealHouse();
                repeatsLeft--;
            }
        }
    }
}
