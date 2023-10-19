using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPositioning : MonoBehaviour
{
    public Transform hand;

    private void Start()
    {
        hand.parent = this.transform;
        hand.localPosition = new Vector3(0, 0, 0);
    } 
}
