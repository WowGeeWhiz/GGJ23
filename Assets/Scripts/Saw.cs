using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, towerHealAmt, healDelay;
    public bool broken;

    public bool targetInRange;
    public bool isSpinning;

    public float attackDelay, damage, currentDamage;
    float lastAttack, lastHeal;

    float audioTimer, audioDelay, audioRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;


    // health for enemies with slider object
    public HealthBarBehavior healthBar;

    void Start()
    {
        currentDurability = maxDurability;
        healthBar.SetHealth(currentDurability, maxDurability);

        audioSource = GetComponent<AudioSource>();

        audioTimer = 0;
        audioRate = 2f;
        audioDelay = 1 / audioRate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentDurability <= 0) broken = true;
        else broken = false;

        if (!broken)
        {
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
                PlayAudio(0,4);
            }
        }
    }

    public void changeDurability(float amount = 0)
    {
        if (amount == 0) amount = towerHealAmt;

        if (amount > 0 && Time.deltaTime >= lastHeal + healDelay)
        {
            currentDurability += amount;
            lastHeal = Time.deltaTime;
        }

        if (amount < 0) currentDurability += amount;

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
            Debug.Log("Saw in player attackBox");
            changeDurability();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            Debug.Log("Saw in player attackBox");
            changeDurability();
        }
    }

    public void PlayAudio(int startIndex, int endIndex)
    {
        int index = Random.Range(startIndex, endIndex);
        audioSource.clip = sounds[index];
        audioSource.PlayOneShot(audioSource.clip, 0.2f);
    }

}
