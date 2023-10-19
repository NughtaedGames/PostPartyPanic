using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameEvents;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class UiSceneCode : MonoBehaviour
{
    public GameObject Countdown;

    public IntInstance amountOfMaxCleanedCups;
    public IntInstance amountOfCleanedCups;

    public GameObject PlayButton, PauseResumeButton,
        CreditsQuitButton, ExitNoButton, RestartNoButton,
        SettingsBackButtonV, SettingsBackButtonA, RestartGameButton;



    public GameObject[] AllUi;  // The videoplayer and Gameinterface should not be a part of this array cause they're in the BG!
    public GameObject pauseMenu;
    public GameObject MainMenu;
    public GameObject credits;
    public GameObject settingsA;
    public GameObject settingsV;
    public GameObject gameinterface; //shall not be a part of the ui array
    public GameObject LoadingScreen;
    public GameObject CharacterSelecton;

    public GameObject videoPlayer; //shall not be a part of the ui array
    public GameObject VideoDisplay;
    public VideoClip MenuToCharacters;
    public VideoClip StandartPartyVideo;
    public VideoClip CharacterBG;
    public VideoClip GameIntroVideo;
    public VideoClip GameIntroLoop;
    private bool comesfromMainmenu=false;

    public GameObject ProgressBar;

    public GameObject Exit_Dialogue;
    public GameObject restart_dialogue1;

    private void Awake()
    {
        GameEventManager.AddListener<StartGame>(startGameEvent);
        GameEventManager.AddListener<SwitchPauseMenu>(SwitchPauseMenu);
        amountOfMaxCleanedCups.Integer = 0;
        amountOfCleanedCups.Integer = 0;
        
        
        //NOT SURE IF NEEDED, Unity doesnt kick out disconnected deviced over several sessions so they ares tacking up
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        Debug.Log("Device added: " + device);
                        InputSystem.AddDevice(device);
                        break;
                    case InputDeviceChange.Removed:
                        Debug.Log("Device removed: " + device);
                        InputSystem.RemoveDevice(device);
                        break;
                    case InputDeviceChange.ConfigurationChanged:
                        Debug.Log("Device configuration changed: " + device);
                        break;
                }
            };
    }

    private void OnDisable()
    {
        GameEventManager.RemoveListener<StartGame>(startGameEvent);
    }
    
    
    void Start()
    {
        AkSoundEngine.PostEvent("MenuMusic", gameObject);

        videoPlayer.GetComponent<VideoPlayer>().isLooping = false;
        videoPlayer.GetComponentInChildren<VideoPlayer>().clip = GameIntroVideo; //start introvideo
    }   

    void Update()
    {

        if (videoPlayer.GetComponent<VideoPlayer>().frame > 0 &&
            videoPlayer.GetComponentInChildren<VideoPlayer>().isPlaying == false &&
            videoPlayer.GetComponentInChildren<VideoPlayer>().clip== GameIntroVideo) //from Intro Video to introloop
        {
            videoPlayer.GetComponent<VideoPlayer>().isLooping = true;
            videoPlayer.GetComponentInChildren<VideoPlayer>().clip = GameIntroLoop;
        }

        if (videoPlayer.GetComponentInChildren<VideoPlayer>().clip == GameIntroLoop&& Input.anyKey)  //from introloop to main menu
        {
            MainMenu.SetActive(true);
            videoPlayer.GetComponent<VideoPlayer>().isLooping = false;  //by an instant
            
        }

            if (videoPlayer.GetComponent<VideoPlayer>().frame > 0 && 
            videoPlayer.GetComponentInChildren<VideoPlayer>().isPlaying == false&&
            comesfromMainmenu==true)
        {
            CharacterSelecton.SetActive(true);
            videoPlayer.GetComponentInChildren<VideoPlayer>().clip = CharacterBG;
            videoPlayer.GetComponent<VideoPlayer>().isLooping = true;
            comesfromMainmenu = false;

            //videoscript ends here
        }

        // if (Countdown.activeSelf == true)
        // {
        //     if(Countdown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length < Countdown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime)
        //     {
        //         Time.timeScale = 1;
        //         Countdown.SetActive(false);
        //     }
        // }
    }
    public void SwitchPauseMenu(GameEvent e)
    {
        pauseMenu.SetActive(!pauseMenu.activeSelf);
            
        Time.timeScale = Convert.ToInt32(!pauseMenu.activeSelf);
        if (pauseMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(PauseResumeButton);
        }
    }
    
    public IEnumerator getSceneProgress()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("GameTests", LoadSceneMode.Additive);
        gameinterface.SetActive(true);
        while (operation.isDone==false)
        {

            Debug.Log(operation.progress);
            ProgressBar.GetComponent<Slider>().value = operation.progress;
            yield return null;
        }
        if(operation.isDone == true)
        {
            Debug.Log("Scene loaded successfully");
            LoadingScreen.SetActive(false);
            sceneloadingDone();
            
        }
    }

    private void sceneloadingDone()
    {
        StartCoroutine(startCountdown());
        
        AkSoundEngine.PostEvent("StopMenuMusic", gameObject);
        AkSoundEngine.PostEvent("GameMusic", gameObject);

    }

    //closing, exit, etc
    public void closeUI() 
    {
        for (int i = 0; i < AllUi.Length ; i++)
        {
            // frick this, doesn't work.   if (AllUi[i].gameObject.active==true) 
            Time.timeScale = 1;
            AllUi[i].SetActive(false);      
        }
    }
    public void startgame() // and sctivate LoadingScreen
    {
        Time.timeScale = 0;
        Debug.Log("loading...");
        LoadingScreen.SetActive(true);
        CharacterSelecton.SetActive(false);
        videoPlayer.SetActive(false);
        VideoDisplay.SetActive(false);
        videoPlayer.GetComponent<VideoPlayer>().isLooping = false;

        StartCoroutine(getSceneProgress());

    }

    public IEnumerator startCountdown()
    {
        yield return new WaitForSecondsRealtime(1f);
        LoadingScreen.SetActive(false);
        Countdown.SetActive(true);
        yield return new WaitForSecondsRealtime(Countdown.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        gameinterface.GetComponent<GameMechanics>().ActivateUI();
        Time.timeScale = 1;
        Countdown.SetActive(false);
    }
    
    public void startGameEvent(StartGame e) // and sctivate LoadingScreen
    {
        startgame();
    }
    
    public void ChangeToCharacterSelection()
    {
        MainMenu.SetActive(false);
        videoPlayer.GetComponentInChildren<VideoPlayer>().clip = MenuToCharacters;
        videoPlayer.GetComponentInChildren<VideoPlayer>().Play();
        videoPlayer.GetComponentInChildren<VideoPlayer>().isLooping= false;
        comesfromMainmenu = true;
        videoPlayer.SetActive(true);
        VideoDisplay.SetActive(true);


        //continue reading in update.
    }

    public void activateExit_Dialogue()
    {
        Debug.Log("you wanna quit?");
        pauseMenu.SetActive(false);
        Exit_Dialogue.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(ExitNoButton);
    }
    public void activateRestart_Dialogue1() //second, do not confuse with upper
    {
        Debug.Log("you wanna Restart?");
        pauseMenu.SetActive(false);
        restart_dialogue1.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(RestartNoButton);
    }
    public void CloseGame()
    {
        Debug.Log("trying to quit");
        Application.Quit();
        Debug.Log("did not quit but at least code doesn't crash");
    }
    public void restartGame()
    {
        SceneManager.UnloadSceneAsync("GameTests");
        gameinterface.SetActive(false);
        //here change to loading screen.


        // remove following when loading function exists-->
        GameEventManager.Raise(new RestartGame());
        SceneManager.LoadSceneAsync("GameTests", LoadSceneMode.Additive);
        restart_dialogue1.SetActive(false);
        gameinterface.SetActive(true);
    }




    //activating other ui 
    public void ChangeToMainMenu() //unloads gamescene
    {
        if (SceneManager.GetSceneByName("GameTests").isLoaded == true)
        {
            SceneManager.UnloadSceneAsync("GameTests");
            SceneManager.LoadScene("Ui station");
           
        }
        else
            Debug.Log("BUG! THERE IS A BUG in the changeTomainmenu code");

        AkSoundEngine.PostEvent("Stop_GameMusic", gameObject);
        //AkSoundEngine.PostEvent("MenuMusic", gameObject);
        AkSoundEngine.PostEvent("Stop_Fire", gameObject);
        AkSoundEngine.PostEvent("Stop_Fire_Alarm", gameObject);
        callMainMenu(); //literally just opens the menu and closes the uld ui. Also resets values.

    }
    public void Credits()
    {
        credits.SetActive(true);
        MainMenu.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(CreditsQuitButton);
    }

    public void callMainMenu()
    {
        closeUI();
        MainMenu.SetActive(true);
        gameinterface.SetActive(false); //not a part of the ui array

        //videoPlayer.GetComponentInChildren<VideoPlayer>().isLooping = true;

        videoPlayer.SetActive(true);
        VideoDisplay.SetActive(true);
        videoPlayer.GetComponentInChildren<VideoPlayer>().clip= StandartPartyVideo;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(PlayButton);


    }

    public void callMenuSimple()
    {
        closeUI();

        if(SceneManager.GetSceneByName("GameTests").isLoaded == false)
        {
            MainMenu.SetActive(true);
        }

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(PlayButton);

    }

    public void V_ChangeToSettingsV()
    {
       closeUI();
        Time.timeScale = 0;
        settingsV.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsBackButtonV);
    }

    public void A_ChangeToSettingsA()
    {
        closeUI();
        Time.timeScale = 0;
        settingsA.SetActive(true);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(SettingsBackButtonA);
    }
}
