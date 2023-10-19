using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSounds : MonoBehaviour
{
    public void PlayCameraSound()
    {
        AkSoundEngine.PostEvent("Take_Photo", gameObject);
    }
}
