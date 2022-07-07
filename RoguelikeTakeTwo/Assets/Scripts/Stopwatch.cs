using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Stopwatch : MonoBehaviour
{
    private bool stopWatch = true;
    private float currentTime;
    [SerializeField] private Text stopWatchText;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(stopWatch){
            currentTime = currentTime + Time.deltaTime;
        }
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        stopWatchText.text = time.Minutes.ToString() + ":" + time.Seconds.ToString();
    }
}
