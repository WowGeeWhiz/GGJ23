using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//create a gameobject called MusicSource in the startscreen scene
//give the gameobject this script and an audisource
//under "Sounds" give this gameobject the DaytimeMusic, CombatMusic, and DeathMusic in that order.
public class Music : MonoBehaviour
{
    public static Music Instance = null;

    float audioTimer, audioDelay, audioRate;
    private AudioSource audioSource;
    public AudioClip[] sounds;

    string lastCheckedScene, lastCheckedTime;
    Timer timer;

    private void Awake()
    {
        // If there is not already an instance of SoundManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        //If an instance already exists, destroy whatever this object is to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        lastCheckedScene = SceneManager.GetActiveScene().name;
        lastCheckedTime = "null";

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sounds[0];

        audioTimer = 0;
        audioDelay = audioSource.clip.length;
    }

    void FixedUpdate()
    {
        if (lastCheckedScene != SceneManager.GetActiveScene().name)
        {
            audioSource.Stop();
            audioTimer = 0;
        }

        if (SceneManager.GetActiveScene().name == "StartScreen" || SceneManager.GetActiveScene().name == "TutorialScene")
        {
            audioSource.clip = sounds[0];
            audioDelay = audioSource.clip.length;

            audioTimer -= Time.deltaTime;
            if (audioTimer <= 0)
            {
                audioTimer = audioDelay;
                audioSource.Play();
            }
        }
        else if (SceneManager.GetActiveScene().name == "EthanMain")
        {
            var tempCan = GameObject.FindGameObjectWithTag("Canvas");
            if (tempCan != null)
            {
                var tempTimer = tempCan.GetComponent<Timer>();
                if (tempTimer != null)
                {
                    timer = tempTimer;

                    if (lastCheckedTime != timer.dayOrNightText.text && timer.dayOrNightText.text == "Day") //if day/night changes and its now day, play day music
                    {

                        audioSource.Stop();
                        audioTimer = 0;

                        audioSource.clip = sounds[1];
                        audioDelay = audioSource.clip.length;

                        audioTimer -= Time.deltaTime;
                        if (audioTimer <= 0)
                        {
                            audioTimer = audioDelay;
                            audioSource.Play();
                        }
                    }
                    else if (lastCheckedTime != timer.dayOrNightText.text && timer.dayOrNightText.text == "Night")//if day/night changes and its now night, play night music
                    {
                        audioSource.Stop();
                        audioTimer = 0;

                        audioSource.clip = sounds[0];
                        audioDelay = audioSource.clip.length;

                        audioTimer -= Time.deltaTime;
                        if (audioTimer <= 0)
                        {
                            audioTimer = audioDelay;
                            audioSource.Play();
                        }
                    }
                    lastCheckedTime = timer.dayOrNightText.text;
                }
            }
        }


        lastCheckedScene = SceneManager.GetActiveScene().name;
    }


    public void PlayAudio(int startIndex, int endIndex)
    {
        int index = Random.Range(startIndex, endIndex);
        audioSource.clip = sounds[index];
        audioSource.PlayOneShot(audioSource.clip, 0.3f);
    }
}
