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

    float lastCheckedDurability;
    float audioTimer, audioDelay, audioRate;
    float audioTimer2, audioDelay2, audioRate2;
    float audioTimer3, audioDelay3, audioRate3;
    private AudioSource audioSource;
    public AudioClip[] sounds;
    public TextMeshProUGUI flameCost;

    void Start()
    {
        //if (gameObject.CompareTag("permanentFlame")) flameCost.text = cost.ToString();
        currentDurability = maxDurability;

        healthBar.SetHealth(currentDurability, maxDurability);

        lastCheckedDurability = currentDurability;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[0];

        audioTimer = 0;
        audioRate = audioSource.clip.length;
        audioDelay = audioSource.clip.length;

        audioTimer2 = 0;
        audioRate2 = 4f;
        audioDelay2 = 1 / audioRate2;

        audioTimer3 = 0;
        audioRate3 = 2f;
        audioDelay3 = 1 / audioRate3;

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
            //audioSource.Stop();
        }
        //else this.gameObject.SetActive(false);

        audioTimer2 -= Time.deltaTime;
        audioTimer3 -= Time.deltaTime;

        if (lastCheckedDurability > currentDurability)
        {
            if (audioTimer2 <= 0)
            {
                audioTimer2 = audioDelay2;
                PlayAudio(1, 7);
            }
        }
        else if (lastCheckedDurability < currentDurability)
        {
            if (audioTimer3 <= 0)
            {
                audioTimer3 = audioDelay3;
                PlayAudio(8, 8);
            }
        }
        lastCheckedDurability = currentDurability;
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

