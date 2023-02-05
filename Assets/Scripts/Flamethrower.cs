using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, attackDelay, damage, currentDamage, towerHealAmt, healCostInWood;
    public bool broken;
    float lastAttack;

    public int repeatsPerCost;
    private int repeatsLeft;

    public GameObject pivot, buildingRadius;
    public PlayerController player;
    public GameObject workingTower, brokenTower;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;

    float audioTimer, audioDelay, audioRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;
    public TextMeshProUGUI flameCost;

    void Start()
    {
        if (gameObject.CompareTag("permanentFlame")) flameCost.text = cost.ToString();
        currentDurability = maxDurability;

        healthBar.SetHealth(currentDurability, maxDurability);

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[0];

        audioTimer = 0;
        audioRate = audioSource.clip.length;
        audioDelay = audioSource.clip.length;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentDurability <= 0)
        {
            brokenTower.gameObject.SetActive(true);
            workingTower.gameObject.SetActive(false);
            broken = true;
        }
        else broken = false;

        if (!broken)
        {
            brokenTower.gameObject.SetActive(false);
            workingTower.gameObject.SetActive(true);
            if (Time.fixedTime >= lastAttack + attackDelay)
            {
                lastAttack = Time.deltaTime;
                currentDamage = damage;
            }
            else currentDamage = 0;

            audioTimer -= Time.deltaTime;
            if (audioTimer <= 0)
            {
                audioTimer = audioDelay;
                PlayAudio(0, 0);
            }
        }
        else if (broken)
        {
            audioSource.Stop();
        }
        //else this.gameObject.SetActive(false);
    }

    public void changeDurability(float amount = 0)
    {
        if (amount == 0) amount = towerHealAmt;

        currentDurability += amount;

        if (currentDurability > maxDurability) currentDurability = maxDurability;

        if (currentDurability <= 0)
        {
            currentDurability = 0;
            gameObject.tag = "BrokenTower";
        }
        else gameObject.tag = "Tower";

        healthBar.SetHealth(currentDurability, maxDurability);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood && currentDurability != maxDurability)
            {

                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                changeDurability();
                repeatsLeft--;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood && currentDurability != maxDurability)
            {

                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                changeDurability();
                repeatsLeft--;
            }
        }
    }


    public void PlayAudio(int startIndex, int endIndex)
    {
        int index = Random.Range(startIndex, endIndex);
        audioSource.clip = sounds[index];
        audioSource.PlayOneShot(audioSource.clip, 0.2f);
    }
}

