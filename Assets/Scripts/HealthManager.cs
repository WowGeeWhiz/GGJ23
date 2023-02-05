using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount;
    private float maxHealth;
    public HouseController house;
    // Start is called before the first frame update
    void Start()
    {
        maxHealth = house.maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        healthAmount = house.currentHealth;
        healthBar.fillAmount = healthAmount / maxHealth;
    }

    public void heal(float healingAmount)
    {
        healthAmount += healingAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 100);

        healthBar.fillAmount = healthAmount / 100f;
    }
}
