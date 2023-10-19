using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unicorn : MonoBehaviour
{
    public ParticleSystem unicorn_stars;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Particles();
        }
    }

    public void Particles()
    {
        unicorn_stars.Play();
    }
}
