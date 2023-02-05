using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI highScore;

    // Start is called before the first frame update
    void Start()
    {
        highScore.text = "High Score:  " + PlayerPrefs.GetFloat("highscore");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
