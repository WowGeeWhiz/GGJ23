using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    bool stopwatchActive = false;
    public float currentTime;
    // public int startMinutes;
    public TextMeshProUGUI currentTimeText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        stopwatchActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (stopwatchActive == true)
        {
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        //currentTimeText.text = currentTime.ToString() + ":" + time.Milliseconds.ToString();
        currentTimeText.text = time.ToString(@"mm\:ss\:fff");
    }

    public void StartTimer()
    {
        stopwatchActive = true;
    }

    public void StopTimer()
    {
        stopwatchActive = false;
    }
}
