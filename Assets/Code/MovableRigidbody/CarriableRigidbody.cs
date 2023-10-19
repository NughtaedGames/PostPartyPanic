using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarriableRigidbody : MovableRigidbody
{
    protected float savedMass;

    public float throwForce = 4000;
    private float distance;
    public bool canBeThrown;
    
    public bool isUsable;
    private PlayerMovement pM;

    private void Start()
    {
        snappinDistance = 0.6f;
    }

    public virtual void UpdateObject(PlayerMovement playerMovement)
    {
        this.transform.eulerAngles = playerMovement.transform.eulerAngles;
        pM = playerMovement;
    }
    
    public virtual void IsBeingCarried(PlayerMovement playerMovement)
    {
        //ActivateOutline();

        isBeingHeld = true;
        ActivateSnappingObject();
        
        this.transform.SetParent(playerMovement.transform);
        this.transform.position = playerMovement.transform.TransformPoint(playerMovement.carrySpot);
        
        var rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        //rigidbody.velocity = Vector3.zero;
        //rigidbody.angularVelocity = Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
        savedMass = rigidbody.mass;
        rigidbody.mass = 0;
        this.transform.eulerAngles = new Vector3(0, 0, 0);
        
    }
    
    public virtual void StoppedBeingCarried()
    {
        //DeactivateOutline();
        
        isBeingHeld = false;
        DeactivateSnappingObject();
        
        var rigidbody = this.GetComponent<Rigidbody>();
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.mass = savedMass;
        
        this.transform.SetParent(null);
    }

    private void FixedUpdate()
    {
        // if (isThrowing)
        // {
        //     rb = this.GetComponent<Rigidbody>();
        //     rb.velocity = Vector3.zero;
        //     rb.angularVelocity = Vector3.zero;
        // }
    }

    public virtual IEnumerator Use()
    {
        //isThrowing = true;
        yield break;
    }

    public virtual IEnumerator StopUsing()
    {
        //StoppedBeingCarried();
        //Debug.LogError("Throw");
        StartCoroutine(pM.GetState().EInteract());
        snapObject = false;
        this.GetComponent<Rigidbody>().AddForce( Vector3.Normalize(transform.forward + transform.up)  * throwForce);
        //isThrowing = false;
        yield break;
    }
}
