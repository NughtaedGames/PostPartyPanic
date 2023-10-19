using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Ragdoll : DragRigidbody
{
    [SerializeField] private GameObject ragdoll;
    [SerializeField] private GameObject ragdollParent;
    [SerializeField] private GameObject baloons;
    [SerializeField] private Rigidbody handConnection;
    private bool isSnapped;
    
    private bool canBeSnapped;

    public override void StartBeingDragged(PlayerMovement playerMovement)
    {
        base.StartBeingDragged(playerMovement);
        
        int newLayer = 8;
        playerMovement.GetComponent<ConfigurableJoint>().xMotion = ConfigurableJointMotion.Limited;
        playerMovement.GetComponent<ConfigurableJoint>().yMotion = ConfigurableJointMotion.Limited;
        playerMovement.GetComponent<ConfigurableJoint>().zMotion = ConfigurableJointMotion.Limited;
        SetLayerRecursively(ragdoll, newLayer);
    }
    
    public override void StopBeingDragged()
    {
        int newLayer = 0;
    
        SetLayerRecursively(this.gameObject, newLayer);
        
        base.StopBeingDragged();
    }

    public override void CheckForObjectSnapping()
    {
        if (canBeSnapped)
        {
            snapObject = true;
        }
    }
    
    // public override void IsBeingCarried(PlayerMovement playerMovement)
    // {
    //     ActivateSnappingObject();
    //     
    //     ragdoll.transform.SetParent(playerMovement.transform);
    //     ragdoll.transform.position = playerMovement.transform.TransformPoint(playerMovement.carrySpot);
    //     
    //     var rigidbody = ragdoll.GetComponent<Rigidbody>();
    //     rigidbody.useGravity = false;
    //     //rigidbody.velocity = Vector3.zero;
    //     //rigidbody.angularVelocity = Vector3.zero;
    //     rigidbody.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;
    //     savedMass = rigidbody.mass;
    //     rigidbody.mass = 0;
    //     ragdoll.transform.eulerAngles = new Vector3(0, 0, 0);
    // }

    protected override void SnapObject()
    {
        var fixedJoint = baloons.GetComponent<FixedJoint>();

        baloons.transform.position = handConnection.position;
        baloons.GetComponent<Balloon>().hasToFloat = true;
        fixedJoint.connectedBody = handConnection;
        if (!isSnapped)
        {
            isSnapped = true;
            GameEventManager.Raise(new IncreaseAmountOfCleanedObjects());
        }
    }
    
    
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
       
        obj.layer = newLayer;
       
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == baloons.gameObject)
        {
            canBeSnapped = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == baloons.gameObject)
        {
            canBeSnapped = false;
        }
    }

    public override void ResetObject()
    {
        ragdollParent.SetActive(false);
        baloons.SetActive(false);
    }
}
