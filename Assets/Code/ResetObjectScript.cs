using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObjectScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<MovableRigidbody>() is MovableRigidbody mr)
        {
            mr.ResetObject();
        }

        if (other.GetComponent<PlayerMovement>() is PlayerMovement pl)
        {
            pl.ResetObject();
        }
    }
}
