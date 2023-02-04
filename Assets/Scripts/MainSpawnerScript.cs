using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainSpawnerScript : MonoBehaviour
{
    public GameObject [] spawner;
    public KeyCode key1, key2, key3, key4;
    public float spawnTime;
    int extra;

    // Start is called before the first frame update
    void Start()
    {
        StartRound(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            CancelInvoke();
        }
    }

    void SpawnExtra()
    {
        spawner[extra].SendMessage("Spawn");
    }
    void SpawnAll()
    {
        spawner[0].SendMessage("Spawn");
        spawner[1].SendMessage("Spawn");
        spawner[2].SendMessage("Spawn");
        spawner[3].SendMessage("Spawn");
}

    void StartRound(int roundNum)
    {
        InvokeRepeating("SpawnAll", spawnTime, spawnTime);
        extra = Random.RandomRange(0, spawner.Length-1);
        Debug.Log("extra in " + extra);
        InvokeRepeating("SpawnExtra", spawnTime, spawnTime);

    }
}
