using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;

public class FireObject : MonoBehaviour
{
    public ParticleSystem fire;
    public ParticleSystem smoke;
    public ParticleSystem sparks;
    public FloatInstance globalTime;
    private int fireTimer;
    
    public bool fireStarted;
    public bool isBeingExtinguished;
    private void Awake()
    {
        GameEventManager.AddListener<StartFire>(StartFire);
    }

    public void ExtinguishFire()
    {
        if (fireStarted)
        {
            isBeingExtinguished = true;
            fire.Stop();
            smoke.Stop();
            sparks.Stop();
            AkSoundEngine.PostEvent("Stop_Fire", gameObject);
        }
    }

    public void StartFire()
    {
        fireTimer = (int)globalTime.Float;
        isBeingExtinguished = false;
        fire.Play();
        smoke.Play();
        sparks.Play();
        fireStarted = true;
        AkSoundEngine.PostEvent("Fire", gameObject);
    }

    public void StartFire(StartFire e)
    {
        Debug.LogError("START FIRE");
        fireTimer = (int)globalTime.Float;
        isBeingExtinguished = false;
        fire.Play();
        smoke.Play();
        sparks.Play();
        fireStarted = true;
        AkSoundEngine.PostEvent("Fire", gameObject);
        StartCoroutine(delayFireAlarm());
    }

    public IEnumerator delayFireAlarm()
    {
        yield return new WaitForSeconds(5);
        AkSoundEngine.PostEvent("Fire_Alarm", gameObject);
        yield return null;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            StartFire();
            AkSoundEngine.PostEvent("Stop_Fire_Alarm", gameObject);
        }


        if ((int)globalTime.Float - fireTimer  == 90 && fireTimer>0)
        {
            if (isBeingExtinguished == false)
            {
                GameEventManager.Raise(new FireIsGameover());
                Debug.Log("FIRE WASN'T PUT OUT FAST ENOUGH; FLEEEE EVERYONE!!!");
            }
        }
    }
}   

