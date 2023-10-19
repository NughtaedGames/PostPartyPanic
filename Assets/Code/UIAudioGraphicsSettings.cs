using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAudioGraphicsSettings : MonoBehaviour
{
    public GameObject G_Resolution;   //quallity gets lower, numbers get higher
    private int G_resNumber;
    private string G_res1 = "1280 x 720";
    private string G_res2 = "1420 x 960";
    private string G_res3 = "1920 x 1080";
    private string G_res4 = "2,048 x 1,080"; //standart
    public GameObject G_Quality;
    private int G_qualNumber;
    private string G_qual1 = "Low"; 
    private string G_qual2 = "Medium";
    private string G_qual3 = "High";//standart
    public GameObject G_Size;
    private int G_SizeNumber;
    private string G_Size1 = "Window";
    private string G_Size2 = "Fullscreen"; //standart
    bool isFullscreen = true;

    private void Start()
    {
        G_resNumber = 4;
        G_qualNumber = 2;
        G_SizeNumber = 2;
        isFullscreen = true;   //when saving, this is probably useless. 
    }
    public void g_resSettingsBigger()
    {


        if (G_resNumber < 4)
        {
            G_resNumber++;
        }
        else
        {
            G_resNumber = 1;
        } 


        if  (G_resNumber == 1) {
            G_Resolution.GetComponent<Text>().text = G_res1;

            Screen.SetResolution(1280 , 720 , isFullscreen);
        }
        else if (G_resNumber == 2){
            G_Resolution.GetComponent<Text>().text = G_res2;

            Screen.SetResolution(1420 , 960, isFullscreen);
        }
        else if (G_resNumber == 3){
            G_Resolution.GetComponent<Text>().text = G_res3;

            Screen.SetResolution(1920 , 1080, isFullscreen);
        }
        else if (G_resNumber == 4){
            G_Resolution.GetComponent<Text>().text = G_res4;

            Screen.SetResolution(2048 , 1080, isFullscreen);
        }
        else
        {
            Debug.Log("Excuse me, a resolution error happened. number too big");
        }

        Debug.Log(G_resNumber);

    }
    public void g_resSettingsSmaller()
    {
        if (G_resNumber > 1)
        {
            G_resNumber--;
        }
        else
        {
            G_resNumber = 4;
        }


        if (G_resNumber == 1)
        {
            G_Resolution.GetComponent<Text>().text = G_res1;

            Screen.SetResolution(1280, 720, isFullscreen);
        }
        else if (G_resNumber == 2)
        {
            G_Resolution.GetComponent<Text>().text = G_res2;

            Screen.SetResolution(1420, 960, isFullscreen);
        }
        else if (G_resNumber == 3)
        {
            G_Resolution.GetComponent<Text>().text = G_res3;

            Screen.SetResolution(1920, 1080, isFullscreen);
        }
        else if (G_resNumber == 4)
        {
            G_Resolution.GetComponent<Text>().text = G_res4;

            Screen.SetResolution(2048, 1080, isFullscreen);
        }


        else
        {
            Debug.Log("Excuse me, a resolution error happened. number too smol");
        }
    }

    // Resolution and Fullscreen/not fullscreen is nothing i can test without a build. You're the one who has to test it, Luca. Good luck.
    public void g_qualSettingsBigger()
    {
        if (G_qualNumber < 2)
        {
            G_qualNumber++;
        }
        else
        {
            G_qualNumber = 0;
        }


        if (G_qualNumber == 0)
        {
            G_Quality.GetComponent<Text>().text = G_qual1;
        }
        else if (G_qualNumber == 1)
        {
            G_Quality.GetComponent<Text>().text = G_qual2;
        }
        else if (G_qualNumber == 2)
        {
            G_Quality.GetComponent<Text>().text = G_qual3;
        }

        else
        {
            Debug.Log("Excuse me, a quality error happened. number too biig");
        }

        QualitySettings.SetQualityLevel(G_qualNumber);

    }
    public void g_qualSettingsSmaller()
    {
        if (G_qualNumber > 0)
        {
            G_qualNumber--;
        }
        else
        {
            G_qualNumber = 2;
        }


        if (G_qualNumber == 0)
        {
            G_Quality.GetComponent<Text>().text = G_qual1;
        }
        else if (G_qualNumber == 1)
        {
            G_Quality.GetComponent<Text>().text = G_qual2;
        }
        else if (G_qualNumber == 2)
        {
            G_Quality.GetComponent<Text>().text = G_qual3;
        }

        else
        {
            Debug.Log("Excuse me, a quality error happened. number too smol");
        }


        QualitySettings.SetQualityLevel(G_qualNumber);
    }


    public void g_sizeSettingsChanged()
    {
        if (G_SizeNumber == 1)
        {
            G_SizeNumber = 2;
        }
        else G_SizeNumber = 1;

        if (G_SizeNumber == 1)
        {
            G_Size.GetComponent<Text>().text = G_Size1;
        }
        else if (G_SizeNumber == 2)
        {
            G_Size.GetComponent<Text>().text = G_Size2;
        }
        else Debug.Log("Excuse me, your size number is out of range");

        if (G_SizeNumber == 1)
        {
            Screen.fullScreen = false;
            isFullscreen = false;
        } 
        if (G_SizeNumber == 2)
        {
            Screen.fullScreen = true;
            isFullscreen = true;
        }

    }

}
