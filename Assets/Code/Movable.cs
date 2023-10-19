using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [SerializeField] private List<Vector3> handles;


    private void Start()
    {
        foreach (Transform child in transform)
        {
            handles.Add(child.position);
        }
    }
}
