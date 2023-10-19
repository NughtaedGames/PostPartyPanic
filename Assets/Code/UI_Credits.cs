using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Credits : MonoBehaviour
{
    public float speed = 4.0f;
    public int length = 4000;
    float time;
    public GameObject Credits;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Credits.transform.position.y <= length)
        {
            time += Time.deltaTime * speed;
            Credits.transform.position = new Vector3(Credits.transform.position.x, time, Credits.transform.position.z);
        }
        else
        {
            time = 0;
            Credits.transform.position = new Vector3(Credits.transform.position.x, 0, Credits.transform.position.z);
        }
    }
}
