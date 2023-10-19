using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public float speed;
    public bool hasToFloat;
    
    private void Update()
    {
        if (hasToFloat)
        {
            
            this.GetComponent<Rigidbody>().AddForce(transform.up * speed, ForceMode.Acceleration);
        }
    }
}
