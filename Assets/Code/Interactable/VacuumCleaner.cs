using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using PathCreation;
using UnityEngine.PlayerLoop;
using GameEvents;

public class VacuumCleaner : CarriableRigidbody
{
    private List<TrashObject> trashObjects;
    private List<PuddleObject> puddleObjects;
    public Transform flyingToPosition;
    [SerializeField] private AnimationCurve flyingCurve;
    [SerializeField] private AnimationCurve flyingSpeed;
    public VisualEffect dustEffect;
    //public GameObject vacuumCleanerHoldingObject;

    Vector3 normalHandleLocalPos;
    Quaternion normalHandleLocalRot;
    public Transform liftedHandlePos;
    public Transform hoseConnection;

    public float suckObjectForce;
    
    private float liftedModeTransitionT;
    private bool isBeingUsed;
    private bool isBeingCarried;

    [Header("Sucking Line")]
    public SuckLine suckLinePrefab;
    public float maxTimeBetweenLines;
    float nextLineSpawnTime;
    Pool<SuckLine> pool;
    
    public float LineDst;
    public float LineAngle;
    public float suckRandomObjectForce;

    private new void Awake()
    {
        GameEventManager.Raise(new DecreaseMaxAmountOfCleanupObjects());
        trashObjects = new List<TrashObject>();
        puddleObjects = new List<PuddleObject>();
        base.Awake();
    }

    public override IEnumerator Use()
    {
        isBeingUsed = true;

        AkSoundEngine.PostEvent("Vacuum_Cleaner", gameObject);
        yield break;
    }

    public override void IsBeingCarried(PlayerMovement playerMovement)
    {
        isBeingCarried = true;
        isBeingHeld = true;
        this.transform.SetParent(playerMovement.vacuumCleanerCarryHandle);
        this.transform.position = playerMovement.transform.TransformPoint(playerMovement.vacuumCleanerCarryHandle.localPosition);
        //this.transform.localRotation = playerMovement.vacuumCleanerCarryHandle.localRotation;
        
        var rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        Debug.LogError("Stop");
        rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        savedMass = rigidbody.mass;
        rigidbody.mass = 0;
        this.transform.localEulerAngles = new Vector3(0, 0, 0);
        
        normalHandleLocalPos = transform.localPosition;
        normalHandleLocalRot = transform.localRotation;
    }

    public override void StoppedBeingCarried()
    {
        isBeingCarried = false;
        isBeingHeld = false;
        
        for (int i = 0; i < puddleObjects.Count; i++)
        {
            puddleObjects[i].StopShrinking();
        }
        base.StoppedBeingCarried();
    }
    
    public override IEnumerator StopUsing()
    {
        isBeingUsed = false;
        AkSoundEngine.PostEvent("Stop_Vacuum_Cleaner", gameObject);
        yield break;
    }

    public override void UpdateObject(PlayerMovement playerMovement)
    {
        //this.transform.localEulerAngles = new Vector3(0, 0, 0);
        
        liftedModeTransitionT = Mathf.Clamp01(liftedModeTransitionT);
        
        float t = Ease.Circular.InOut(liftedModeTransitionT);
        liftedHandlePos = playerMovement.liftedVacuumCleanerCarryHandle;
        transform.localPosition = Vector3.Lerp(normalHandleLocalPos, liftedHandlePos.localPosition, t);
        transform.localRotation = Quaternion.Slerp(normalHandleLocalRot, liftedHandlePos.localRotation, t);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TrashObject>() is TrashObject trash)
        {
            trashObjects.Add(trash);
        }
        else if (other.gameObject.GetComponent<PuddleObject>() is PuddleObject puddle)
        {
            puddleObjects.Add(puddle);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trashObjects.Contains(other.gameObject.GetComponent<TrashObject>()))
        {
            trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
            //other.gameObject.GetComponent<TrashObject>().DeactivateHighlight();
        }
        else if (puddleObjects.Contains(other.gameObject.GetComponent<PuddleObject>()))
        {
            other.gameObject.GetComponent<PuddleObject>().StopShrinking();
            puddleObjects.Remove(other.gameObject.GetComponent<PuddleObject>());
        }
    }

    private void Start()
    {
        pool = new Pool<SuckLine>(suckLinePrefab, 20);
        isUsable = true;
    }
    
    private void Update()
    {
        if (isBeingUsed)
        {
            liftedModeTransitionT += Time.deltaTime * 5;
            
            if (trashObjects.Count > 0)
            {
                for (int i = trashObjects.Count -1; i > 0; i--) 
                {
                    //trashObjects.rigidbody.AddForceAtPosition((Vector3.up) * collisionForce, collision.contacts[0].point, ForceMode.VelocityChange);
                    if (!trashObjects[i]) 
                    { 
                        trashObjects.RemoveAt(i);
                        return;
                    }
                    if (trashObjects[i].transform.position != flyingToPosition.position && !trashObjects[i].animationStarted)
                    {
                        //trashObjects[i].SetupFlyingPath(flyingCurve, flyingSpeed, flyingToPosition, this);
                        trashObjects[i].SetupFlyingPath(flyingToPosition, this, suckObjectForce); 
                    }
                }
            }
            //Sucking Line
            if (Time.time > nextLineSpawnTime)
            {
                nextLineSpawnTime = Time.time + UnityEngine.Random.value * maxTimeBetweenLines;
                SuckLine line;
                if (pool.TryInstantiate(out line))
                {
                    float randomAngle = (UnityEngine.Random.value - 0.5f) * LineAngle * Mathf.Deg2Rad;
                    float randomDst = Mathf.Lerp(LineDst * 0.25f, LineDst, UnityEngine.Random.value);
                    float currentAngle = Mathf.Atan2(flyingToPosition.forward.z, flyingToPosition.forward.x);

                    Vector3 randomDir = new Vector3(Mathf.Cos(currentAngle + randomAngle), 0, Mathf.Sin(currentAngle + randomAngle));
                    line.transform.position = flyingToPosition.position + randomDir * randomDst;
                    line.Init(this);
                }
            }

            var cols = Physics.OverlapSphere(flyingToPosition.position + flyingToPosition.forward * LineDst/2,5);

            foreach (var c in cols) {
                Rigidbody r = null;
                if (c.TryGetComponent<Rigidbody>(out r)) {
                    r.AddForce((flyingToPosition.position - r.position).normalized * suckRandomObjectForce, ForceMode.Impulse);
                }
            }
        }
        else
        {
            liftedModeTransitionT -= Time.deltaTime * 5;
            if (puddleObjects.Count > 0 && isBeingCarried)
            {

                for (int i = 0; i < puddleObjects.Count; i++)
                {
                    if (!puddleObjects[i]) 
                    {
                        puddleObjects.RemoveAt(i);
                        return;
                    }
                    if (!puddleObjects[i].isShrinking)
                    {
                        puddleObjects[i].StartShrinking();
                    }
                }
                
                //
                // for (int i = puddleObjects.Count - 1; i > 0 ; i--)
                // {
                //
                // }
            }
        }
    }

    public override void CheckForObjectSnapping()
    {
        return;
    }
}
