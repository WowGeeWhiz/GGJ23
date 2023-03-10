using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class AIMovement : MonoBehaviour
{
    Animator animator;
    public GameObject house;
    private GameObject[] towers;
    private GameObject[] brokenTowers;
    public float speed, attackDistance, health, stopDistanceForHouse;
    public int towerKillScore, playerKillScore;
    public bool ignoreTowers;
    public int woodAwarded;

    //variables for attacking towers/house
    public float damageOutput, attackDelay;
    float lastAttack;

    //floats for taking damage
    public float damageInterval;
    float lastDamage;
    PlayerController player;
    public Flamethrower permaFlame;
    public Saw permaSaw;

    // health for enemies with slider object
    //public float hitpoints;
    //public float maxHitpoints;
    public HealthBarBehavior healthBar;

    float audioTimer, audioDelay, audioRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    private bool hasAwardred;
    private float towerDistance, distaceToClosestTower, currentHealth, houseDistance;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
        brokenTowers = GameObject.FindGameObjectsWithTag("BrokenTower");

        if (player.autoKillEnemies)
        {
            foreach (GameObject bTower in brokenTowers)
            {
                var tempSaw = bTower.GetComponent<Saw>();
                var tempFlame = bTower.GetComponent<Flamethrower>();

                if (tempSaw != null)
                {
                    tempSaw.BreakThis();
                    player.cinemEnables.Add(bTower);
                    bTower.SetActive(false);
                }
                if (tempFlame != null)
                {
                    tempFlame.BreakThis();
                    player.cinemEnables.Add(bTower);
                    bTower.SetActive(false);
                }
            }
        }

        foreach (GameObject currentTower in towers)
        {
            if (player.autoKillEnemies)
            {
                var tempSaw = currentTower.GetComponent<Saw>();
                var tempFlame = currentTower.GetComponent<Flamethrower>();

                if (tempSaw != null)
                {
                    tempSaw.BreakThis();
                    player.cinemEnables.Add(currentTower);
                    currentTower.SetActive(false);
                }
                if (tempFlame != null)
                {
                    tempFlame.BreakThis();
                    player.cinemEnables.Add(currentTower);
                    currentTower.SetActive(false);
                }
            }
            else
            {
                float distanceToTower = (currentTower.transform.position - this.transform.position).sqrMagnitude;
                if (distanceToTower < distaceToClosestTower)
                {
                    distaceToClosestTower = distanceToTower;
                    closestTower = currentTower;
                    //Debug.Log("found closest tower");
                }
                if (distanceToTower <= permaSaw.range)
                {
                    var Saw = currentTower.GetComponent<Saw>();
                    if (Saw != null) TakeDamage(permaSaw.damage);
                }
                if (distanceToTower <= permaFlame.range)
                {
                    var Flame = currentTower.GetComponent<Flamethrower>();
                    if (Flame != null) TakeDamage(permaFlame.damage);
                }
            }
        }

        if (player.autoKillEnemies) Destroy(this.gameObject);
        houseDistance = Vector2.Distance(this.gameObject.transform.position, house.transform.position);
        towerDistance = Vector2.Distance(this.transform.position, closestTower.transform.position);

        //direction.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //different movements


        //tower
        if (towerDistance < attackDistance && !ignoreTowers)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, closestTower.transform.position, speed * Time.deltaTime);
            var flame = closestTower.GetComponent<Flamethrower>();
            var saw = closestTower.GetComponent<Saw>();
            if (flame != null || saw != null) closestTower.SendMessage("changeDurability", -damageOutput);
        }

        //move towards house
        else if (houseDistance > stopDistanceForHouse)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, house.transform.position, speed * Time.deltaTime);
            //transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        }
        else if (houseDistance < stopDistanceForHouse)
        {
            animator.SetBool("IsMoving", false);
            house.SendMessage("TakeDamage", damageOutput);
        }


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
            if (!hasAwardred)
            {
                hasAwardred = true;
                player.wood += woodAwarded;
            }
            animator.SetBool("isDying", true);
            Invoke("KillEnemy",1);
        }
    }

    void KillEnemy()
    {
        Destroy(gameObject);
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
