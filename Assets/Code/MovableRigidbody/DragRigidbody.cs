using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;


public class DragRigidbody : MovableRigidbody
{
    public List<Transform> contactPoints;

    public Transform contactPointParent;

    List<GameObject> snappingEffects = new List<GameObject>();

    private bool isSoundRunning;
    
    private void Reset()
    {
        rb = GetComponent<Rigidbody>();
    }

    // private void Awake()
    // {
    //     
    //     
    //     //StartCoroutine(nameof(DeactivateAfter10));
    // }



    private IEnumerator DeactivateAfter10()
    {
        yield return new WaitForSeconds(7f);
        DeactivateOutline();
    }
    

    
    void Start()
    {

        
        if (contactPointParent == null)
        {
            Debug.LogError("ContactPointParent for " + this + " missing!");
            return;
        }
        foreach (Transform child in contactPointParent)
        {
            contactPoints.Add(child);
        }
    }

    public virtual void StartBeingDragged(PlayerMovement playerMovement) { }

    public virtual void StopBeingDragged() { }

    void OnDrawGizmos()
    {
    //     if (!Application.isPlaying)
    //     {
    //         if (outlineObject == null)
    //         {
    //             return;
    //             //CreateOutlines();
    //         }
    //         Gizmos.color = Color.white;
    //         if (this.GetComponent<MeshFilter>())
    //         {
    //             Gizmos.DrawMesh(this.GetComponent<MeshFilter>().sharedMesh, outlineTransform.position, outlineTransform.rotation);
    //         }
    //         
    //     }
    //     
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere( transform.TransformPoint(centerOfMass) , 0.2f);
    //     
        if (contactPointParent == null)
        {
            return;
        }
        foreach (Transform child in contactPointParent)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(child.position, 0.2f);
        }
    
    }

    private void Update()
    {
        if(GetComponent<Rigidbody>().velocity.magnitude > 0.2 && !isSoundRunning)
        {

            isSoundRunning = true;
            AkSoundEngine.PostEvent("Player_Drag", gameObject);
        }
        else if (GetComponent<Rigidbody>().velocity.magnitude < 0.2 && isSoundRunning)
        {
            isSoundRunning = false;
            AkSoundEngine.PostEvent("Stop_Player_Drag", gameObject);
        }

        base.Update();
    }
}
