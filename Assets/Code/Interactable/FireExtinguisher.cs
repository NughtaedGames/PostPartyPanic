using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using GameEvents;
using UnityEngine.PlayerLoop;

public class FireExtinguisher : CarriableRigidbody
{
    public VisualEffect fireExtuinguisherEffect;
    private bool isBeingUsed;
    public List<FireObject> _fireObjects;

    private new void Awake()
    {
        GameEventManager.Raise(new DecreaseMaxAmountOfCleanupObjects());
        base.Awake();
    }

    private new void Update()
    {
        if (isBeingUsed && _fireObjects.Count > 0)
        {
            foreach (var fire in _fireObjects)
            {
                if (!fire.isBeingExtinguished)
                {
                    fire.ExtinguishFire();
                }
            }
        }
        base.Update();
    }
    
    public override IEnumerator Use()
    {
        if (isUsable)
        {
            fireExtuinguisherEffect.Play();
            isBeingUsed = true;
            AkSoundEngine.PostEvent("Fire_Extinguisher", gameObject);
        }
        yield break;
    }

    public override IEnumerator StopUsing()
    {
        if (isUsable)
        {
            fireExtuinguisherEffect.Stop();
            isBeingUsed = false;
            //fireExtuinguisherEffect.transform.parent = this.transform;
            AkSoundEngine.PostEvent("Stop_Fire_Extinguisher", gameObject);
        }
        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<FireObject>() is FireObject fire)
        {
            _fireObjects.Add(fire);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (_fireObjects.Contains(other.gameObject.GetComponent<FireObject>()))
        {
            _fireObjects.Remove(other.gameObject.GetComponent<FireObject>());
            //other.gameObject.GetComponent<TrashObject>().DeactivateHighlight();
        }
    }
    
    public override void CheckForObjectSnapping()
    {
        return;
    }
}
