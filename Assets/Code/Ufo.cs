using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using Random = System.Random;

public class Ufo : MonoBehaviour
{
    public bool shouldMove;


    [SerializeField] private GameObject UFOContent;
    [SerializeField] private Transform[] routes;
    private float tParam;
    private int routeStart = 0;
    [SerializeField]
    private float speedModifier = 0.5f;
    private Vector3 ufoPosition;

    private void Awake()
    {
        GameEventManager.AddListener<StartUFO>(StartUFO);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            if (UFOContent.activeSelf)
            {
                return;
            }
            Debug.Log("UFO starts moving");
            shouldMove = true;
        }

        if (shouldMove)
        {
            StartCoroutine(FollowRoute(routeStart));
        }
    }

    private void StartUFO(StartUFO e)
    {
        if (UFOContent.activeSelf)
        {
            return;
        }
        Debug.Log("UFO starts moving");
        shouldMove = true;
    }
    
    private IEnumerator FollowRoute(int routeNumber)
    {
        shouldMove = false;
        UFOContent.SetActive(true);
        AkSoundEngine.PostEvent("UFO_Hovering", gameObject);
        var radius = 25;
        tParam = 0f;
        
        var r1 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        var r2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        
        while (Vector2.Distance(r1, r2) > 30 || Vector2.Distance(r1, r2) < 20)
        {
            r2 = UnityEngine.Random.insideUnitCircle.normalized * radius;
        }

        
        routes[routeNumber].GetChild(0).position = new Vector3(r1.x, 1 , r1.y);
        routes[routeNumber].GetChild(3).position = new Vector3(-r1.x, 1 , -r1.y);

        routes[routeNumber].GetChild(1).position = new Vector3(r2.x, 1 , r2.y);
        routes[routeNumber].GetChild(2).position = new Vector3(-r2.x, 1 , -r2.y);
        
        var p0 = routes[routeNumber].GetChild(0).position;
        var p1 = routes[routeNumber].GetChild(1).position;
        var p2 = routes[routeNumber].GetChild(2).position;
        var p3 = routes[routeNumber].GetChild(3).position;

        while (tParam < 1)
        {
            tParam += Time.deltaTime * speedModifier;
            
            ufoPosition = Mathf.Pow(1- tParam, 3) * p0 + 
                          3*Mathf.Pow(1 - tParam, 2)*tParam* p1 + 
                          3*(1-tParam) * Mathf.Pow(tParam,2) * p2 + 
                          Mathf.Pow(tParam,3)* p3;

            UFOContent.transform.position = ufoPosition;
            yield return new WaitForFixedUpdate();

        }
        tParam = 0f;
        UFOContent.SetActive(false);
        AkSoundEngine.PostEvent("Stop_UFO_Hovering", gameObject);
    }
    
}
