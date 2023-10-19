using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.VFX;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Outline))]
public class MovableRigidbody : MonoBehaviour
{
    protected Outline outline;

    public bool isBeingHeld;
    
    public Vector3 startPosition;
    public Quaternion startRotation;
    private GameObject snappingObject;
    //[HideInInspector]
    public Transform snappingTransform;
    //[HideInInspector]
    //public Transform contactPointParent;
    
    [SerializeField] private Vector3 centerOfMass;
    
    public Rigidbody rb;
    
    [SerializeField]
    public GameObject outlineMaskObject;
    [SerializeField]
    public GameObject wireFrameObject;

    private float snappingSpeed = 1f;
    protected float snappinDistance = 0.5f;
    protected bool isSnapped;
    protected bool snapObject = false;
    
    public Transform snappingEffectTransform;

    protected void Awake()
    {
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        UnparentSnapping();
        
        GameEventManager.AddListener<HighlightObjects>(ActivateOutline);
        outline = GetComponent<Outline>();
        outline.enabled = false;
        
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
        
        GameEventManager.Raise(new IncreaseMaxAmountOfCleanupObjects());
    }

    private void UnparentSnapping()
    {
        if (snappingTransform)
        {
            snappingTransform.transform.parent.parent = null;
            DeactivateSnappingObject();
        }
    }


    public virtual void ActivateSnappingObject()
    {
        if (snappingTransform)
        {
            snappingTransform.gameObject.SetActive(true);
        }
    }
    
    public virtual void DeactivateSnappingObject()
    {
        if (snappingTransform)
        {
            snappingTransform.gameObject.SetActive(false);
        }
        
    }
    
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
        {
            if (snappingObject == null)
            {
                return;
                //CreateOutlines();
            }
            Gizmos.color = Color.white;
            if (this.GetComponent<MeshFilter>())
            {
                Gizmos.DrawMesh(this.GetComponent<MeshFilter>().sharedMesh, snappingTransform.position, snappingTransform.rotation);
            }
            
        }
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere( transform.TransformPoint(centerOfMass) , 0.2f);
        
        // if (contactPointParent == null)
        // {
        //     return;
        // }
        // foreach (Transform child in contactPointParent)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawSphere(child.position, 0.2f);
        // }

    }
    
    public virtual void ActivateOutline()
    {
        outline.enabled = true;
    }

    public virtual void DeactivateOutline()
    {
        outline.enabled = false;
    }

    public void ActivateOutline(HighlightObjects e)
    {
        if (!isSnapped)
        {
            ActivateOutline();
            StartCoroutine(DeactivateOutlineAfter5());
        }
    }
    
    private IEnumerator DeactivateOutlineAfter5()
    {
        yield return new WaitForSeconds(5f);
        DeactivateOutline();
    }
    


    public virtual void CheckForObjectSnapping()
    {
        
        // Debug.LogError("trans pos: " + this.transform.position);
        // Debug.LogError("snapping pos: " + snappingTransform.parent.position);

        Vector3 aPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 bPos = new Vector3(snappingTransform.parent.position.x, 0, snappingTransform.parent.position.z);

        if (Vector3.Distance(aPos, bPos) < snappinDistance && !isSnapped)
        {
            snapObject = true;
        }
    }

    protected virtual void SnapObject()
    {
        transform.position = Vector3.MoveTowards(transform.position, snappingTransform.parent.position, Time.deltaTime * snappingSpeed);

        transform.rotation = snappingTransform.parent.rotation;
        
        if (Vector3.Distance(transform.position, snappingTransform.parent.position) < 0.1 && !isSnapped)
        {
            //SnappingEffect();

            isSnapped = true;
            snapObject = false;
            var randomIndex = UnityEngine.Random.Range(0, snappingEffectTransform.childCount - 1);
            var shape = snappingEffectTransform.GetChild(randomIndex).GetComponent<ParticleSystem>().shape;
            shape.mesh = this.GetComponent<MeshFilter>().mesh;
            snappingEffectTransform.GetChild(randomIndex).GetComponent<ParticleSystem>().Play();
            GameEventManager.Raise(new IncreaseAmountOfCleanedObjects());
            
        }
        
        //var newDirection = Vector3.RotateTowards(transform.forward, snappingTransform.forward, Time.deltaTime * 1, 0.0f);
        //transform.rotation = Quaternion.LookRotation(newDirection.normalized);

        // // What rotation do we have to apply for M to become -S?
        // Quaternion rotationFromCurrent = Quaternion.FromToRotation(transform.forward, snappingTransform.parent.forward);
        //
        // // Calculate the new rotation
        // Quaternion newRotation = this.transform.rotation * rotationFromCurrent;
        //
        // // rotate the object
        // this.transform.rotation = newRotation;
    }

    // protected virtual void SnappingEffect()
    // {
    //     snappingEffect.Play();
    // }
    
    protected void Update()
    {


        if (!snappingTransform)
        {
            return;
        }
        if (snapObject && !isSnapped)
        {
            SnapObject();
        }
        
        if (Vector3.Distance(transform.position, snappingTransform.parent.position) > 0.8 && isSnapped)
        {
            GameEventManager.Raise(new DecreaseAmountOfCleanedObjects());
            isSnapped = false;
        }
        
    }

    
    public virtual void ResetObject()
    {
        rb.isKinematic = true;
        transform.position = startPosition + new Vector3(0,2,0);
        transform.rotation = startRotation;
        isSnapped = false;
        snapObject = false;
        rb.isKinematic = false;
    }
}
