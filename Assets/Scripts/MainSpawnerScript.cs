using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSpawnerScript : MonoBehaviour
{
    public GameObject spawner1, spawner2, spawner3, spawner4;
    public KeyCode key1, key2, key3, key4;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key1))
        {
            spawner1.SendMessage("Spawn");
        }
        if (Input.GetKeyDown(key2))
        {
            spawner2.SendMessage("Spawn");
        }
        if (Input.GetKeyDown(key3))
        {
            spawner3.SendMessage("Spawn");
        }
        if (Input.GetKeyDown(key4))
        {
            spawner4.SendMessage("Spawn");
        }
    }
}
