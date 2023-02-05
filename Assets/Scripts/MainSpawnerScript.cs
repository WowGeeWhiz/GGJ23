using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MainSpawnerScript : MonoBehaviour
{
    public GameObject [] spawner;
    //public KeyCode key1, key2, key3, key4;
    public float spawnTime;

    // Start is called before the first frame update
    void Start()
    { 
        //StartRound(1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown("space"))
        //{
        //    CancelInvoke("SpawnAll");
        //    CancelInvoke("SpawnExtra");
        //}
    }

    public void SpawnExtra()
    {
        int extra = UnityEngine.Random.Range(0, 4);
        Debug.Log("extra in: " + extra);
        spawner[extra].SendMessage("Spawn");
    }
    void SpawnAll()
    {
        for (int i = 0; i < spawner.Length; i++)
        {
            spawner[i].SendMessage("Spawn");
        }

}

    void StartRound(int roundNum)
    {
        InvokeRepeating("SpawnAll", spawnTime, spawnTime);
        for (int i = 0; i < roundNum; i++)
        {
            InvokeRepeating("SpawnExtra", spawnTime, spawnTime);
        }
    }
}
