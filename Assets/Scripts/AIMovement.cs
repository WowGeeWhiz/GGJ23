using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject  house;
    private GameObject[] towers; 
    public float speed, attackDistance,health, damage;
    public bool ignoreTowers;

    //floats for taking damage
    public float damageInterval;
    float lastDamage;
    PlayerController player;

    private float playerDistance, towerDistance, distaceToClosestTower, currentHealth;
    // Start is called before the first frame update
    void Start()
    {

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
        else
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
    void TakeDamage(float damage)
    {
        currentHealth = health - damage;
        Debug.Log("enemy health: " + currentHealth);
        lastDamage = Time.fixedTime;

        if (currentHealth <= 0) Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            Debug.Log("Player hit enemy");
            TakeDamage(player.damage);

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (Time.fixedTime - damageInterval >= lastDamage)
            {
                Debug.Log("Enemy stayed in attack");
                TakeDamage(player.damage);
            }
        }
           
    }
}
