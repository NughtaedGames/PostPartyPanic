using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using Random = System.Random;

public class Dog : CarriableRigidbody
{

    [SerializeField] private Outline snappingOutline;
    private bool canBeSnapped;
    [SerializeField] private GameObject visualSound;
    private bool isplaced;
    
    private void Awake()
    {
        this.GetComponent<Rigidbody>().isKinematic = true;
        base.Awake();
        StartCoroutine(RandomSounds());
    }

    public override void ActivateSnappingObject()
    {
        snappingOutline.enabled = true;
    }

    public override void DeactivateSnappingObject()
    {
        snappingOutline.enabled = false;
    }

    public override void ActivateOutline()
    {
        
    }

    public override void DeactivateOutline()
    {
        
    }
    
    public override void CheckForObjectSnapping()
    {
        if (canBeSnapped)
        {
            snapObject = true;
        }
    }

    private IEnumerator RandomSounds()
    {
        while (!isplaced)
        {
            var time = UnityEngine.Random.Range(15, 30);
            yield return new WaitForSecondsRealtime(time);
            if (isplaced)
            {
                yield break;
            }
            visualSound.SetActive(true);
            AkSoundEngine.PostEvent("Dog_Bark", gameObject);
            yield return new WaitForSecondsRealtime(6);
            if (isplaced)
            {
                yield break;
            }
            visualSound.SetActive(false);
        }
    }
    
    protected override void SnapObject()
    {
        isplaced = true;
        AkSoundEngine.PostEvent("Dog_Bark", gameObject);
        StopCoroutine(RandomSounds());
        this.transform.position = snappingTransform.position;
        this.transform.rotation = snappingTransform.rotation;
        
        var randomIndex = UnityEngine.Random.Range(0, snappingEffectTransform.childCount - 1);
        snappingEffectTransform.GetChild(randomIndex).GetComponent<ParticleSystem>().Play();
        snapObject = false;
        isSnapped = true;
        GameEventManager.Raise(new IncreaseAmountOfCleanedObjects());
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == snappingOutline.gameObject)
        {
            canBeSnapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == snappingOutline.gameObject)
        {
            canBeSnapped = false;
        }
    }
    
    public override void IsBeingCarried(PlayerMovement playerMovement)
    {
        isplaced = true;
        StopCoroutine(RandomSounds());
        visualSound.SetActive(false);
        base.IsBeingCarried(playerMovement);
    }
}
