using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnTime;
    // Start is called before the first frame update
    public GameObject maximum, minimum, enemy;
    void Start()
    {
        InvokeRepeating("Spawn", spawnTime, spawnTime);
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

        Instantiate(enemy, random_position, Quaternion.identity);
    }
}
