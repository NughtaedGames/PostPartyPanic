using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private Slider master_volume_bar;
    [SerializeField] private Slider music_volume_bar;
    [SerializeField] private Slider sfx_volume_bar;
    
    private void Awake()
    {
        AkSoundEngine.SetRTPCValue("master_volume", PlayerPrefs.GetFloat("master_volume", 75));
        AkSoundEngine.SetRTPCValue("music_volume", PlayerPrefs.GetFloat("music_volume", 75));
        AkSoundEngine.SetRTPCValue("sfx_volume", PlayerPrefs.GetFloat("sfx_volume", 75));
        master_volume_bar.value = PlayerPrefs.GetFloat("master_volume", 75);
        music_volume_bar.value = PlayerPrefs.GetFloat("music_volume", 75);
        sfx_volume_bar.value = PlayerPrefs.GetFloat("sfx_volume", 75);
        this.gameObject.SetActive(false);
    }

    public void OnMasterChanged(float volume)
    {
        AkSoundEngine.SetRTPCValue("master_volume", volume);
        PlayerPrefs.SetFloat("master_volume", volume);
        PlayerPrefs.Save();
    }
    
    public void OnMusicChanged(float volume)
    {
        AkSoundEngine.SetRTPCValue("music_volume", volume);
        PlayerPrefs.SetFloat("music_volume", volume);
        PlayerPrefs.Save();
    }
    
    public void OnSfxChanged(float volume)
    {
        AkSoundEngine.SetRTPCValue("sfx_volume", volume);
        PlayerPrefs.SetFloat("sfx_volume", volume);
        PlayerPrefs.Save();
    }
}
