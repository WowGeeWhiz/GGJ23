using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamethrower : MonoBehaviour
{
    public float cost, maxDurability, currentDurability, range, attackDelay, damage, currentDamage, towerHealAmt, healDelay;
    public bool broken;
    float lastAttack, lastHeal;

    public GameObject pivot, buildingRadius;

    // health for enemies with slider object
    public HealthBarBehavior healthBar;

    float audioTimer, audioDelay, audioRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    void Start()
    {
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
            Debug.Log("Flame in player attackBox");
            changeDurability();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            Debug.Log("Flame in player attackBox");
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
