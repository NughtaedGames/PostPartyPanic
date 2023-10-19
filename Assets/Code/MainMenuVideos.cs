using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class MainMenuVideos : MonoBehaviour
{

    public VideoClip[] MenuBG;
    public GameObject videoPlayer;

    private int i =0;

    private void Awake()
    {

        videoPlayer.GetComponent<VideoPlayer>().Stop();
        videoPlayer.GetComponentInChildren<VideoPlayer>().clip = MenuBG[2];
    }

    private void Update()
    {


        if (videoPlayer.GetComponent<VideoPlayer>().frame > 0 && videoPlayer.GetComponentInChildren<VideoPlayer>().isPlaying == false) 
        {
            
            videoPlayer.GetComponentInChildren<VideoPlayer>().clip = MenuBG[i];
            videoPlayer.GetComponentInChildren<VideoPlayer>().Play();
            Debug.Log(("starting to play video")+(i.ToString()));
                i++;

            if (i > MenuBG.Length-1)
            {
                i = 0;
                Debug.Log("starting videos new");
            }
        }

    }
}
