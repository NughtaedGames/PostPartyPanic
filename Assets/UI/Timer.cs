using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 5f;
    float timePassed;

    void Start()
    {
        timerBar = GetComponent<Image>();
        timePassed = 0;
    }

    void Update()
    {
        if (timePassed < maxTime)
        {
            timePassed += Time.deltaTime;
            timerBar.fillAmount = timePassed / maxTime;
        }
        //else
        //{
        //    Time.timeScale = 0;
        //}
    }
}
