using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using GameEvents;
using TMPro;
using UnityEngine.SceneManagement;

public class TimerSlider : MonoBehaviour
{
    public FloatInstance globalTime;
    Slider timerBar;
    public float maxTime = 300f;
    float timePassed;

    public bool timerIsActive = false;
    public float timeLeft = 300;
    public TextMeshProUGUI timeText;
    private bool fireStarted;
    

    void Start()
    {
        globalTime.Float = 0;
        timerBar = GetComponent<Slider>();
        timePassed = 0;
        timerIsActive = true;    
    }

    private void Awake()
    {
        GameEventManager.AddListener<RestartGame>(Gamestart);
    }

    private void Gamestart(RestartGame e)//every time, game starts new, do dis
    {
        timeLeft = maxTime;
        timePassed = 0f;
        globalTime.Float = timePassed;
    }

    void Update()
    {

            if (Input.GetKeyDown(KeyCode.T))
            {
                timePassed = 297;
                timeLeft = 3;
            }

            if (Input.GetKey(KeyCode.Z))
            {
                GameEventManager.Raise(new DestroyCup());
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                GameEventManager.Raise(new IncreaseAmountOfCleanedObjects());
            }
        
            if (timerIsActive)
            {
                if (timePassed < maxTime)
                {
                    timePassed += Time.deltaTime;
                    timerBar.value = timePassed / maxTime;
                }

                if (timeLeft > 0)
                {
                    timeLeft -= Time.deltaTime;
                    DisplayTime(timeLeft);
                }

                else if( timeLeft <=0)
                {
                    timeLeft = 0;
                    timerIsActive = false;
                    GameEventManager.Raise(new TimeIsOver()); //Hi, Carla
                }
            }

            if ((int)timeLeft == 180 && !fireStarted)
            {
                GameEventManager.Raise(new StartFire());
                fireStarted = true;
            }
            else if ((int)timeLeft == 150)
            {
                GameEventManager.Raise(new StartUFO());
            }
            globalTime.Float = timePassed;
    }


    void DisplayTime(float timeDisplayed)
    {
        //timeDisplayed += 1;



        if (timeDisplayed <= 0)
        {
            timeText.text = "0:00";
        }
        else
        {
            float minutes = Mathf.FloorToInt(timeDisplayed / 60);
            float seconds = Mathf.FloorToInt(timeDisplayed % 60);
            
            timeText.text = string.Format("{0:0}:{1:00}", minutes, seconds);
        }
        

    }
}
