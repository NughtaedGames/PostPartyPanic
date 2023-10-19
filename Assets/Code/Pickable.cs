using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using UnityEngine.Events;

public class Pickable : MonoBehaviour
{
    bool pickable = false;
    bool carrying = false;


    // Start is called before the first frame update
    void Start()
    {

    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<InReachEvent>(OnInReachEvent);
    }
    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<InReachEvent>(OnInReachEvent);
    }

    public virtual void OnInReachEvent(InReachEvent e)
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void pickupObject()
    {

    }

    void throwObject()
    {

    }

    void placeObject()
    {

    }
}
