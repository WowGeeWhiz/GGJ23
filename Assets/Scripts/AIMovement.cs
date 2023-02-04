using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject  house, canvas;
    private GameObject[] towers; 
    public float speed, attackDistance, health, stopDistanceForHouse;
    public int towerKillScore, playerKillScore;
    public bool ignoreTowers;

    //floats for taking damage
    public float damageInterval;
    float lastDamage;
    PlayerController player;

    private float playerDistance, towerDistance, distaceToClosestTower, currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<PlayerController>();
        house = GameObject.FindGameObjectWithTag("House");
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        distaceToClosestTower = Mathf.Infinity;
        GameObject closestTower = null;
        towers = GameObject.FindGameObjectsWithTag("Tower");

        foreach (GameObject currentTower in towers)
        {
            float distanceToTower = (currentTower.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToTower < distaceToClosestTower)
            {
                distaceToClosestTower = distanceToTower;
                closestTower = currentTower;
                //Debug.Log("found closest tower");
            }
        }
        //playerDistance = Vector2.Distance(transform.position, player.transform.position);
        towerDistance = Vector2.Distance(transform.position, closestTower.transform.position);
        Vector2 direction = house.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //different movements



        //tower
        if (towerDistance < attackDistance && !ignoreTowers)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, closestTower.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        //move towards house
        else if (Vector3.Distance(this.transform.position, house.transform.position) > stopDistanceForHouse)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, house.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        //player
        //else if (playerDistance < attackDistance)
        //{ 
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //    transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        //}
    }
    void TakeDamage(float damage, bool isPlayer = false)
    {
        currentHealth = health - damage;
        Debug.Log("enemy health: " + currentHealth);
        lastDamage = Time.fixedTime;

        if (currentHealth <= 0)
        {

            if (isPlayer) player.score += towerKillScore;
            else player.score += playerKillScore;
            Destroy(gameObject);

        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            Debug.Log("Player hit enemy");
            TakeDamage(player.damage, true);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (Time.fixedTime - damageInterval >= lastDamage)
            {
                Debug.Log("Enemy stayed in attack");
                TakeDamage(player.damage, true);
            }
        }
           
    }
}
