using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PuddleObject : MonoBehaviour
{
    public bool isShrinking;
    
    private void Start()
    {
        GameEventManager.Raise(new SpawnCup());
    }

    private void Update()
    {
        if (isShrinking)
        {
            UpdateShrinkingPuddle();
        }
    }

    public void StartShrinking()
    {
        isShrinking = true;
        AkSoundEngine.PostEvent("Slurp", gameObject);
    }

    public void StopShrinking()
    {
        isShrinking = false;
        AkSoundEngine.PostEvent("Stop_Slurp", gameObject);
    }
    
    private void UpdateShrinkingPuddle()
    {
        this.transform.localScale = new Vector3(transform.localScale.x * 0.99f, transform.localScale.y * 0.999f,transform.localScale.z * 0.99f);

        if (this.transform.localScale.x < 0.25f)
        {
            GameEventManager.Raise(new DestroyCup());
            AkSoundEngine.PostEvent("Stop_Slurp", gameObject);
            Destroy(this.gameObject);
        }
    }
    
}
