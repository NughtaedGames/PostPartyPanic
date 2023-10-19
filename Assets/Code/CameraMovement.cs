using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraMovement : MonoBehaviour
{
    public List<Transform> targets;

    [SerializeField]
    private Vector3Instance playerPos;

    public Vector3 offset;
    public float smoothTime = .5f;

    public float minZoom = 40;
    public float maxZoom = 10;
    public float zoomLimiter = 50f;

    private Vector3 velocity;
    private Camera cam;


    private void Awake()
    {
        GameEventManager.AddListener<RestartGame>(ResetTargets);
        GameEventManager.AddListener<PlayerJoined>(PlayerJoined);
    }

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    //updates Camera movement and zoom after Player input and so on
    private void LateUpdate()
    {
        if (targets.Count == 0)
            return;
        Move();
        Zoom();
    }

    // moves to center between Players
    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        //playerPos.vector3 = centerPoint;


        transform.position = Vector3.SmoothDamp(transform.position, centerPoint + offset, ref velocity, smoothTime);
    }


    // zooms depending on distance of players
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }


    // check the width of bound to check furthest apart player
    float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.size.x;
    }

    // create bound around players to get the center
    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }

    // adds player to list when called
    public void PlayerJoined(PlayerJoined pl)
    {
        targets.Add(pl.player);
    }

    // removes player form list when called
    public void PlayerLeft(Transform deleteTarget)
    {
        foreach (var target in targets)
        {
            if (target == deleteTarget)
            {
                targets.Remove(target);
                return;
            }
        }
    }

    private void ResetTargets(RestartGame e)
    {
        targets = new List<Transform>();
    }

    

}
