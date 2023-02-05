using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.U2D.Animation;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, towerHealAmt, healCostInWood;
    public bool broken;

    public int repeatsPerCost;
    private int repeatsLeft;

    public PlayerController player;

    public bool targetInRange;
    public bool isSpinning;

    public float attackDelay, damage, currentDamage;
    float lastAttack;

    float lastCheckedDurability;
    float audioTimer, audioDelay, audioRate;
    float audioTimer2, audioDelay2, audioRate2;
    float audioTimer3, audioDelay3, audioRate3;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    public GameObject workingTower, brokenTower;

    float innerRadius = 0.1f, outerRadius = 0.5f;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;
    public TextMeshProUGUI sawCost;

    void Start()
    {
        //if (gameObject.CompareTag("permanentSaw")) sawCost.text = cost.ToString();
        currentDurability = maxDurability;
        healthBar.SetHealth(currentDurability, maxDurability);

        lastCheckedDurability = currentDurability;

        audioSource = GetComponent<AudioSource>();

        audioTimer = 0;
        audioRate = 2f;
        audioDelay = 1 / audioRate;

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
                PlayAudio(0, 4);
            }

            audioTimer2 -= Time.deltaTime;
            audioTimer3 -= Time.deltaTime;

            if (lastCheckedDurability > currentDurability)
            {
                if (audioTimer2 <= 0)
                {
                    audioTimer2 = audioDelay2;
                    PlayAudio(5, 12);
                }
            }
            else if (lastCheckedDurability < currentDurability)
            {
                if (audioTimer3 <= 0)
                {
                    audioTimer3 = audioDelay3;
                    PlayAudio(13, 13);
                }
            }
            lastCheckedDurability = currentDurability;

        }
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

    public void BreakThis()
    {
        changeDurability(-currentDurability);
    }
}
