using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSounds : MonoBehaviour
{
    public void PlayCameraSound()
    {
        AkSoundEngine.PostEvent("Take_Photo", gameObject);
    }
    
    public void PlayCountdownSound()
    {
        AkSoundEngine.PostEvent("Countdown", gameObject);
    }
    
    public void PlayCollectStarSound()
    {
        AkSoundEngine.PostEvent("Collect_Star", gameObject);
    }
}
