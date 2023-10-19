using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiSounds : MonoBehaviour
{
    public void PlayButtonClicked()
    {
        AkSoundEngine.PostEvent("Click", gameObject);
    }
}
