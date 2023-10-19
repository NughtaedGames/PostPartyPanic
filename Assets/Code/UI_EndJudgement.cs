using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;



public class UI_EndJudgement : MonoBehaviour
{
    public GameObject exitbutton;

    public IntInstance amountOfMaxCleanupObjects;
    public IntInstance amountOfCleanedUpObjects;

    public IntInstance amountOfMaxCleanedCups;
    public IntInstance amountOfCleanedCups;
    int percentOfCups;


    public GameObject TimesUp;
    public GameObject BG;
    public GameObject Camera;
    public GameObject pictures;
    public GameObject Score;
    public Sprite[] PolaroidEndPics; //0-3
    public GameObject DisplayEndPic;
    public Sprite BurnedHousePic;
    public GameObject[] ActiveStars;        //0-2
    public Sprite[] EndTextPics;     //0-3
    public GameObject DisplayText;

    public GameObject GoodEnd;
    public GameObject StandartEnd;
    public GameObject BadEnd;
    public GameObject FireEnd;

    private bool TimesUpWasActive = false;

    private bool didGameEnd;
  private void Update()
    {
        if (didGameEnd == true)
        {
            if(TimesUpWasActive == false)  //step1
            {
                StartCoroutine(endSceneAnimation());
            }
        }
    }

  
      public IEnumerator endSceneAnimation()
      {
          TimesUp.SetActive(true);
          BG.SetActive(true);
          Time.timeScale = 0;

          var vidlength = TimesUp.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
          yield return new WaitForSecondsRealtime(vidlength);
          
          Camera.SetActive(true);
          vidlength = Camera.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
          yield return new WaitForSecondsRealtime(vidlength);
          
          pictures.SetActive(true);
          EndOf(calculateSucess(), false); //calculates success and displays it
          vidlength = pictures.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
          yield return new WaitForSecondsRealtime(vidlength);
          
          Camera.SetActive(false);
          Score.SetActive(true);
          didGameEnd = false;
          EventSystem.current.SetSelectedGameObject(null);
          EventSystem.current.SetSelectedGameObject(exitbutton);

      }

      private bool DidAnimationFinnisch(GameObject gigi)
    {
        if (gigi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length < gigi.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime) //is it done?
        {
            return true;
        }

        else
        {
            return false;
        }
    }


    private void Awake()
    {
        GameEventManager.AddListener<TimeIsOver>(EndAnimations);
        GameEventManager.AddListener<FireIsGameover>(FireGameover);

    }

    public void FireGameover(FireIsGameover a)
    {
        FireEnd.SetActive(true);
        Time.timeScale = 0;
    }

    private void EndAnimations(TimeIsOver a)
    {
        didGameEnd = true; 
    }

    private void EndOf(int Endscore, bool fire)
    {
        DisplayEndPic.GetComponent<Image>().sprite = PolaroidEndPics[Endscore];
        DisplayText.GetComponent<Image>().sprite = EndTextPics[Endscore];

        for(int a=0; a< Endscore;)
        {
            ActiveStars[a].SetActive(true);
            a++;    
        }
    }

    private int calculateSucess()
    {
        percentOfCups = (amountOfCleanedCups.Integer * 100 / amountOfMaxCleanedCups.Integer); //percentofcups, 100% = best 0%=worst. Actually, let me say 96 % is the best.. and that /4 is about.. 24

        if (amountOfCleanedUpObjects.Integer+percentOfCups / 20 >= (amountOfMaxCleanupObjects.Integer+ 4)) 
        {
            Debug.Log("GREATTT");
            return 3;
        }
        else if (amountOfCleanedUpObjects.Integer+ percentOfCups / 20 >= (amountOfMaxCleanupObjects.Integer+ 4) / 1.5) //easier
        {
            Debug.Log("OKAYY");
            return 2;
        }
        else if (amountOfCleanedUpObjects.Integer+ percentOfCups/20 >= (amountOfMaxCleanupObjects.Integer+ 4) / 2.5) //easier
        {
            Debug.Log("CLOSE!");
            return 1;
        }
        else if (amountOfCleanedUpObjects.Integer+ percentOfCups / 20 >= 0) //always, lol
        {
            Debug.Log("BADDD");
            return 0;
        }
        Debug.Log(amountOfCleanedUpObjects.Integer);
        return 4;
    }

}
