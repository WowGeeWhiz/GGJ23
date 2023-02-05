using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscene : MonoBehaviour
{
    public VideoPlayer video;
    public bool hasStopped;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.position = new Vector3(0, 0, -25);   
    }

    // Update is called once per frame
    void Update()
    {
        hasStopped = !video.isPlaying;
    }
}
