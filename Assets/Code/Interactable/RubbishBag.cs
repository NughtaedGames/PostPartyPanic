using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

public class RubbishBag : CarriableRigidbody
{
    
    public List<TrashObject> trashObjects;
    private bool isBeingUsed;
    [SerializeField] private AnimationCurve flyingCurve;
    [SerializeField] private AnimationCurve flyingSpeed;
    [SerializeField] private Transform flyingToPosition;
    public VisualEffect dustEffect;
    
    public override IEnumerator Use()
    {
        if (isUsable)
        {
            isBeingUsed = true;
            
            if (isBeingUsed && trashObjects.Count > 1)
            {
                for (int i = trashObjects.Count -1; i > 0; i--)
                {

                    if (trashObjects[i].transform.position != flyingToPosition.position)
                    {
                        trashObjects[i].SetupFlyingPath(flyingCurve, flyingSpeed, flyingToPosition, this);
                        //UpdateFlyingPath(trashObjects[i]);
                    }
                    else
                    {
                        //trashObjects[i].animationStarted = false;
                    }
                
                    //Destroy(trashObjects[i].gameObject);
                }
            }
            
        }
        yield break;
    }

    public override IEnumerator StopUsing()
    {
        if (isUsable)
        {
            isBeingUsed = false;
        }
        yield break;
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<TrashObject>() is TrashObject trash)
        {
            trashObjects.Add(trash);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (trashObjects.Contains(other.gameObject.GetComponent<TrashObject>()))
        {
            trashObjects.Remove(other.gameObject.GetComponent<TrashObject>());
            //other.gameObject.GetComponent<TrashObject>().DeactivateHighlight();
        }
    }

    //private float _animationTimePosition;
    // private void UpdateFlyingPath(TrashObject obj)
    // {
    //     if (!obj.animationStarted)
    //     {
    //         Debug.LogError("DUUUUUDE");
    //         obj._startPosition = obj.transform.position;
    //         obj.animationStarted = true;
    //         obj._animationTimePosition = 0;
    //         obj.GetComponent<Rigidbody>().useGravity = false;
    //     }
    //     obj._animationTimePosition += Time.deltaTime;
    //     Vector3 modifiedDest = flyingToPosition.transform.position;
    //     modifiedDest.y = flyingCurve.Evaluate(obj._animationTimePosition); //flyingToPosition.transform.position.y + flyingCurve.Evaluate(obj._animationTimePosition);
    //     Debug.LogError("Y: " + modifiedDest.y);
    //     
    //     obj.transform.position = Vector3.Lerp(obj._startPosition, flyingToPosition.transform.position, flyingSpeed.Evaluate(obj._animationTimePosition));
    //     //Debug.LogError("Y: " + modifiedDest.y);
    //     obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y + modifiedDest.y, obj.transform.position.z);
    //     
    //     
    //     // if (!obj.animationStarted)
    //     // {
    //     //     obj._startPosition = obj.transform.position;
    //     //     obj.animationStarted = true;
    //     //     obj._animationTimePosition = 0;
    //     // }
    //
    //     //obj.GetComponent<Rigidbody>().MovePosition(new Vector3(obj.transform.position.y, obj._startPosition.y + flyingCurve.Evaluate(_animationTimePosition) ,obj.transform.position.z));
    //
    //
    // }
}
