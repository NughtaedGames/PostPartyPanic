using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class TrashObject : MonoBehaviour
{
    public float _animationTimePosition;
    public Vector3 _startPosition;
    public bool animationStarted;
    private AnimationCurve flyingCurve;
    private AnimationCurve flyingSpeed;
    private Transform flyingToPosition;
    private CarriableRigidbody cleaner;
    private Rigidbody rb;
    private float suckObjectForce;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (animationStarted)
        {
            UpdateFlyingPath();
        }
    }
    private void Awake()
    {
        GameEventManager.Raise(new SpawnCup());
    }

    public void SetupFlyingPath(AnimationCurve flyingCurve, AnimationCurve flyingSpeed, Transform flyingToPosition, CarriableRigidbody cleaner)
    {
        this.flyingCurve = flyingCurve;
        this.flyingSpeed = flyingSpeed;
        this.flyingToPosition = flyingToPosition;
        this.cleaner = cleaner;
        
        _startPosition = transform.position;
        animationStarted = true;
        _animationTimePosition = Random.Range(-0.4f, 0f);
        GetComponent<Rigidbody>().useGravity = false;
    }
    
    public void SetupFlyingPath(Transform flyingToPosition, CarriableRigidbody cleaner, float suckObjectForce)
    {
        this.flyingToPosition = flyingToPosition;
        this.cleaner = cleaner;
        this.suckObjectForce = suckObjectForce;
        
        //_startPosition = transform.position;
        animationStarted = true;
        //GetComponent<BoxCollider>().isTrigger = true;
        rb.useGravity = false;
        //_animationTimePosition = Random.Range(-0.4f, 0f);
        //rb.useGravity = false;
    }
    

    private void UpdateFlyingPath()
    {
        // _animationTimePosition += Time.deltaTime;
        // Vector3 modifiedDest = flyingToPosition.transform.position;
        // modifiedDest.y = flyingCurve.Evaluate(_animationTimePosition);
        //
        // transform.position = Vector3.Lerp(_startPosition, flyingToPosition.transform.position, flyingSpeed.Evaluate(_animationTimePosition));
        // transform.position = new Vector3(transform.position.x, transform.position.y + modifiedDest.y, transform.position.z);
        

        rb.AddForce((flyingToPosition.position - rb.position).normalized * suckObjectForce, ForceMode.Impulse);

        
        if (Vector3.Distance(flyingToPosition.position,transform.position) < 0.2f)
        {
            if (cleaner is VacuumCleaner vacuum)
            {
                vacuum.dustEffect.Play();
                AkSoundEngine.PostEvent("Tok", gameObject);
                //AkSoundEngine.PostEvent("GameMusic", gameObject);
            }
            GameEventManager.Raise(new DestroyCup());
            DestroyImmediate(this.gameObject);
        }

    }
    
    
}
