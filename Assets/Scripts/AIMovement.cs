using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    private GameObject player, house;
    private GameObject[] towers; 
    public float speed, attackDistance;

    private float playerDistance, towerDistance, distaceToClosestTower;
    // Start is called before the first frame update
    void Start()
    {
 
        player = GameObject.FindGameObjectWithTag("Player");
        house = GameObject.FindGameObjectWithTag("House");
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
                Debug.Log("found closest tower");
            }
        }
        //layerDistance = Vector2.Distance(transform.position, player.transform.position);
        towerDistance = Vector2.Distance(transform.position, closestTower.transform.position);
        Vector2 direction = player.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //different movements



        //tower
        if (towerDistance < attackDistance)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, closestTower.transform.position, speed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        }

        //move towards house
        else {
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
}
