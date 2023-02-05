using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class AIMovement : MonoBehaviour
{
    public GameObject house;
    private GameObject[] towers; 
    public float speed, attackDistance, health, stopDistanceForHouse;
    public int towerKillScore, playerKillScore;
    public bool ignoreTowers;

    //variables for attacking towers/house
    public float damageOutput, attackDelay;
    float lastAttack;

    //floats for taking damage
    public float damageInterval;
    float lastDamage;
    PlayerController player;
    public Flamethrower flamethrowerPrefab;
    public Saw sawPrefab;

    // health for enemies with slider object
    //public float hitpoints;
    //public float maxHitpoints;
    public EnemyHealthBarBehavior healthBar;

    private float towerDistance, distaceToClosestTower, currentHealth, houseDistance;
    // Start is called before the first frame update
    void Start()
    {
        GameObject GetSaw = GameObject.FindGameObjectWithTag("permanentSaw");
        sawPrefab = GetSaw.GetComponent<Saw>();
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        player = temp.GetComponent<PlayerController>();
        currentHealth = health;

        //hitpoints = maxHitpoints;
        healthBar.SetHealth(currentHealth, health);

    }

    // Update is called once per frame
    void FixedUpdate()
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
            if (distanceToTower <= sawPrefab.range)
            {
                var Saw = currentTower.GetComponent<Saw>();
                if (Saw != null) TakeDamage(sawPrefab.damage);
            }
        }
        houseDistance = Vector2.Distance(this.gameObject.transform.position, house.transform.position);
        towerDistance = Vector2.Distance(this.transform.position, closestTower.transform.position);

        //direction.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //different movements


        //tower
        if (towerDistance < attackDistance && !ignoreTowers)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, closestTower.transform.position, speed * Time.deltaTime);
            Attack(closestTower);
        }

        //move towards house
        else if (houseDistance > stopDistanceForHouse)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, house.transform.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);

            if (houseDistance < attackDistance) Attack(house);
        }
        else if (houseDistance < attackDistance) Attack(house);


        //player
        //else if (playerDistance < attackDistance)
        //{ 
        //    transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, speed * Time.deltaTime);
        //    transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        //}
    }
    public void TakeDamage(float damage, bool isPlayer = false)
    {
        if (!isPlayer) Debug.Log($"Taking {damage} from tower");
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth, health);
        Debug.Log("enemy health: " + currentHealth);
        lastDamage = Time.fixedTime;

        if (currentHealth <= 0)
        {
            if (isPlayer) player.score += playerKillScore;
            else player.score += towerKillScore;
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
        if (collision.gameObject.CompareTag("fireAttackBox"))
        {
            Debug.Log("Fire hit enemy");
            TakeDamage(flamethrowerPrefab.damage);

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
        if (collision.gameObject.CompareTag("fireAttackBox"))
        {
            if (Time.fixedTime - damageInterval >= lastDamage)
            {
                Debug.Log("Enemy stayed in fire");
                TakeDamage(flamethrowerPrefab.damage);
            }
        }

    }

    private void Attack(GameObject Tower)
    {
        if (Time.fixedTime < lastAttack + attackDelay) return;

        bool targetIsSaw = false, targetIsFlamethrower = false, targetIsHouse = false;
        var tempSaw = Tower.GetComponent<Saw>();
        var tempFlame = Tower.GetComponent<Flamethrower>();
        var tempHouse = house.GetComponent<HouseController>();
        if (tempSaw != null) targetIsSaw = true;
        else if (tempFlame != null) targetIsFlamethrower = true;
        else if (tempHouse != null) targetIsHouse = true;

        if (targetIsSaw)
        {
            //Debug.Log("Enemy attacked saw");
            Tower.GetComponent<Saw>().changeDurability(damageOutput);
            //saw.changeDurability(damageOutput);
            lastAttack = Time.fixedTime;
            return;
        }
        if (targetIsFlamethrower)
        {
            Flamethrower flame = Tower.GetComponent<Flamethrower>();
            flame.changeDurability(damageOutput);
            lastAttack = Time.fixedTime;
            return;
        }
        if (targetIsHouse)
        {
            HouseController house = Tower.GetComponent<HouseController>();
            house.changeDurability(damageOutput);
            lastAttack = Time.fixedTime;
            return;
        }
    }

    
}
