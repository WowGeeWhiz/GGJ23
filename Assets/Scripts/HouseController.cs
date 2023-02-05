using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseController : MonoBehaviour
{
    public float maxHealth, currentHealth;
    public PlayerController player;
    public float healthAmt, healCostInWood;
    private int repeatsLeft;
    public int repeatsPerCost;
    float lastCheckedHealth;

    float audioTimer, audioDelay, audioRate;
    float audioTimer2, audioDelay2, audioRate2;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    // Start is called before the first frame update
    void Start()
    {
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

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0) player.lockMovement = true;
    }

    void FixedUpdate()
    {
        audioTimer -= Time.deltaTime;

        if (lastCheckedHealth > currentHealth)
        {
            if (audioTimer <= 0)
            {
                audioTimer = audioDelay;
                PlayAudio(0, 5);
            }
        }
        else if(lastCheckedHealth < currentHealth)
        {
            if (audioTimer2 <= 0)
            {
                audioTimer2 = audioDelay2;
                PlayAudio(6, 6);
            }
        }
        lastCheckedHealth = currentHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

    }

    public void HealHouse()
    {
        currentHealth += healthAmt;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("attackBox"))
        {
            if (player.wood >= healCostInWood)
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
