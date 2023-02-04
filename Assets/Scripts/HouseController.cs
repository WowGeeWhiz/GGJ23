using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public float maxHealth, currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeDurability(float damage)
    {
        currentHealth += damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
}
