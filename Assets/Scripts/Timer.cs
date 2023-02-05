using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public bool stopTimer;

    //bool stopwatchActive = false;
    bool dayTimeTimer = false;
    float currentDayTime;
    public float dayMinutes;
    public GameObject MainSpawner;

    private int roundNum;

    public bool nightTimeTimer = false;
    float currentNightTime;
    public float nightMinutes;

    public TextMeshProUGUI currentTimeText;
    public TextMeshProUGUI dayOrNightText;

    public GameObject nightTrees, dayTrees;
    public GameObject nightShader;

    // Start is called before the first frame update
    void Start()
    {
        stopTimer = true;
        roundNum = 1;
        
        currentDayTime = dayMinutes * 60;
        StartDayTimer();

        nightTimeTimer = false;
        currentNightTime = nightMinutes * 60;

        //dayOrNightText = FindObjectOfType<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time;

        if (dayTimeTimer == true)
        {
            dayOrNightText.text = "Day";
            currentDayTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentDayTime);
            if (currentDayTime <= 0)
            {
                StopDayTimer();
                StartNightTimer();
            }
            dayTrees.SetActive(true); 
            nightTrees.SetActive(false);
            nightShader.SetActive(false);
        }
        else 
        {
            if (nightTimeTimer == false && stopTimer == true) return;
            dayOrNightText.text = "Night";
            restoreDayTime(dayMinutes);
            currentNightTime -= Time.deltaTime;
            time = TimeSpan.FromSeconds(currentNightTime);
            //currentTimeText.text = time.ToString("Night Time: " + @"mm\:ss\:fff");
            if (currentNightTime <= 0)
            {
                StopNightTimer();
                StartDayTimer();
            }
            dayTrees.SetActive(false);
            nightTrees.SetActive(true);
            nightShader.SetActive(true);
        }
        currentTimeText.text = time.ToString(@"mm\:ss");
        //currentTimeText.text = currentTime.ToString() + ":" + time.Milliseconds.ToString();
    }

    public void FixedUpdate()
    {
        if (dayTimeTimer == true)
        {
            restoreNightTime(nightMinutes);

        }
        else
        {

            restoreDayTime(dayMinutes);
        }
    }
    public void restoreDayTime(float dayMinutes)
    {
        currentDayTime = dayMinutes * 60;
    }
    public void StartDayTimer()
    {
        MainSpawner.SendMessage("StartRound", roundNum);
        roundNum++;
        //stopwatchActive = true;
        dayTimeTimer = true;
    }
    public void StopDayTimer()
    {
        MainSpawner.SendMessage("CancelInvoke");

        //stopwatchActive = false;
        dayTimeTimer = false;
    }


    public void restoreNightTime(float dayMinutes) 
    {
        currentNightTime = nightMinutes * 60;
    }
    public void StartNightTimer()
    {
        nightTimeTimer = true;
    }
    public void StopNightTimer()
    {
        nightTimeTimer = false;
    }
}
