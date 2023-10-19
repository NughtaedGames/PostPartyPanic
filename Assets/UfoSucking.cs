using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class UfoSucking : MonoBehaviour
{
    private List<MovableRigidbody> mRbList = new List<MovableRigidbody>();

    public float suckObjectForce;
    public Vector3 torque;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MovableRigidbody>() is MovableRigidbody mrb)
        {
            mRbList.Add(mrb);
        }
    }

    void FixedUpdate()
    {
        if (mRbList.Count > 0)
        {
            //Debug.LogError("rbList[i].rb.useGravity");

            for (int i = 0; i < mRbList.Count; i++)
            {
                //Debug.LogError(rbList[i].rb.useGravity);
                mRbList[i].rb.useGravity = false;
                mRbList[i].rb.AddForce(((transform.position +new Vector3(0,5,0)) - mRbList[i].transform.position).normalized * suckObjectForce, ForceMode.Impulse);
                mRbList[i].rb.AddTorque(torque);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MovableRigidbody>() is MovableRigidbody mrb)
        {
            mrb.GetComponent<Rigidbody>().useGravity = true;
            mRbList.Remove(mrb);
        }
    }
}
