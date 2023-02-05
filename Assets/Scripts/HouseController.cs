using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class HouseController : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public PlayerController player;
    public float healthAmt, healCostInWood;
    private int repeatsLeft;
    public int repeatsPerCost;
    public bool hasPlayedDeath;

    public Sprite house, damagedHouse;
    SpriteRenderer renderer;

    float lastCheckedHealth;

    float audioTimer, audioDelay, audioRate;
    float audioTimer2, audioDelay2, audioRate2;
    private AudioSource audioSource;
    public AudioClip[] sounds;
    public bool HasPlayedRespawn = false;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        renderer.sprite = house;
        currentHealth = maxHealth;
        repeatsLeft = repeatsPerCost;

        lastCheckedHealth = currentHealth;

        audioSource = GetComponent<AudioSource>();

        audioTimer = 0;
        audioRate = 4f;
        audioDelay = 1 / audioRate;

        audioTimer2 = 0;
        audioRate2 = 2f;
        audioDelay2 = 1 / audioRate2;
    }

    void FixedUpdate()
    {
        audioTimer -= Time.deltaTime;
        audioTimer2 -= Time.deltaTime;

        if (lastCheckedHealth > currentHealth)
        {
            if (audioTimer <= 0)
            {
                audioTimer = audioDelay;
                PlayAudio(0, 5);
            }
        }
        else if (lastCheckedHealth < currentHealth)
        {
            if (audioTimer2 <= 0)
            {
                audioTimer2 = audioDelay2;
                PlayAudio(6, 6);
            }
        }
        lastCheckedHealth = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.resetHouse)
        {
            currentHealth = maxHealth / 2;
        }
        if (currentHealth <= 0)
        {
            player.lockMovement = true;
            renderer.sprite = damagedHouse;
            if (player.score > PlayerPrefs.GetFloat("highscore")) PlayerPrefs.SetFloat("highscore", player.score);
            if (!hasPlayedDeath)
            {
                hasPlayedDeath = true;
                player.PlayDeath();
            }
            else if (!HasPlayedRespawn)
            {
                player.PlayRespawn();
                HasPlayedRespawn = true;
            }
            else
            {
                currentHealth = maxHealth / 2;
            }


        }
        else
        {
            player.lockMovement = false;
            renderer.sprite = house;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }

    public void HealHouse(float replaceValue = 0)
    {
        if (replaceValue != 0) currentHealth += replaceValue;
        else currentHealth += healthAmt;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood && currentHealth != maxHealth)
            {
                //Debug.Log("House in player attackBox and has enough to repair");
                
                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                HealHouse();
                repeatsLeft--;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood)
            {

                if (repeatsLeft <= 0)
                {
                    repeatsLeft = repeatsPerCost;
                    player.wood -= (int)healCostInWood;
                }
                HealHouse();
                repeatsLeft--;
            }
        }
    }

    public void PlayAudio(int startIndex, int endIndex)
    {
        int index = Random.Range(startIndex, endIndex);
        audioSource.clip = sounds[index];
        audioSource.PlayOneShot(audioSource.clip, 0.7f);
    }
}
