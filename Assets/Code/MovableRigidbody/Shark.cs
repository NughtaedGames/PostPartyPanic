using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.PlayerLoop;

public class Shark : CarriableRigidbody
{
    
    [SerializeField] private Outline snappingOutline;
    private bool canBeSnapped;
    [SerializeField] private GameObject visualSound;
    private bool isplaced;
    [SerializeField] private Transform[] routes;
    public int routeStart = 0;
    private float tParam;
    [SerializeField]
    private float speedModifier = 0.5f;
    private Vector3 sharkPosition;

    private bool coroutineAllowed;
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
            AkSoundEngine.PostEvent("Splash", gameObject);
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
        AkSoundEngine.PostEvent("Splash", gameObject);
        StopCoroutine(RandomSounds());
        this.transform.position = snappingTransform.position;
        this.transform.rotation = snappingTransform.rotation;
        
        var randomIndex = UnityEngine.Random.Range(0, snappingEffectTransform.childCount - 1);
        snappingEffectTransform.GetChild(randomIndex).GetComponent<ParticleSystem>().Play();
        snapObject = false;
        isSnapped = true;

        GameEventManager.Raise(new IncreaseAmountOfCleanedObjects());

        coroutineAllowed = true;

    }
    
    private IEnumerator FollowRoute(int routeNumber)
    {
        this.GetComponent<Animator>().Play("swimming");
        var radius = 25;
        tParam = 0f;
        
        // var r1 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        // var r2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        //
        // while (Vector2.Distance(r1, r2) > 30 || Vector2.Distance(r1, r2) < 20)
        // {
        //     r2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        // }

        
        // routes[routeNumber].GetChild(0).position = new Vector3(r1.x, 1 , r1.y);
        // routes[routeNumber].GetChild(3).position = new Vector3(-r1.x, 1 , -r1.y);
        //
        // routes[routeNumber].GetChild(1).position = new Vector3(r2.x, 1 , r2.y);
        // routes[routeNumber].GetChild(2).position = new Vector3(-r2.x, 1 , -r2.y);
        
        var p0 = routes[routeNumber].GetChild(0).position;
        var p1 = routes[routeNumber].GetChild(1).position;
        var p2 = routes[routeNumber].GetChild(2).position;
        var p3 = routes[routeNumber].GetChild(3).position;
        
        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            
            sharkPosition = Mathf.Pow(1- tParam, 3) * p0 + 
                            3*Mathf.Pow(1 - tParam, 2)*tParam* p1 + 
                            3*(1-tParam) * Mathf.Pow(tParam,2) * p2 + 
                            Mathf.Pow(tParam,3)* p3;

            transform.LookAt(sharkPosition);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, -90, 0));
            this.transform.position = sharkPosition;
            yield return new WaitForFixedUpdate();

        }
        tParam = 0f;
        coroutineAllowed = true;
    }

    private void LateUpdate()
    {
        if (coroutineAllowed)
        {
            coroutineAllowed = false;
            StartCoroutine(FollowRoute(routeStart));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == snappingTransform.gameObject)
        {
            canBeSnapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == snappingTransform.gameObject)
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
