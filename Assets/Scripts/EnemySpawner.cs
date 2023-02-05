using System.Collections;
using System.Collections.Generic;
using UnityEditor.AssetImporters;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    //public float spawnTime;
    public GameObject maximum, minimum, enemy1, enemy2, house, player, permaSaw;
    public float ratio;
    //public KeyCode key;

    // Start is called before the first frame update

    void Start()
    {
        //InvokeRepeating("Spawn", spawnTime, spawnTime);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Spawn()
    {
        float x = Random.Range(minimum.transform.position.x, maximum.transform.position.x);
        float y = Random.Range(minimum.transform.position.y, maximum.transform.position.y);

        Vector2 random_position = new Vector2(x, y);

        float rand = Random.Range(0, 10);

        if (rand < ratio)
        {
            GameObject temp = Instantiate(enemy2, random_position, Quaternion.identity);
            AIMovement tempAI = temp.GetComponent<AIMovement>();
            tempAI.house = house;
            tempAI.permaSaw = permaSaw.GetComponent<Saw>();
            
        }
        else
        { 
            GameObject temp = Instantiate(enemy1, random_position, Quaternion.identity);
            AIMovement tempAI = temp.GetComponent<AIMovement>();
            tempAI.house = house;
            tempAI.permaSaw = permaSaw.GetComponent<Saw>();
        }
    }
}
