using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;

public class FireWarning : MonoBehaviour
{

    [SerializeField] private SpriteRenderer fireWarning;
    
    void Start()
    {
        GameEventManager.AddListener<StartFire>(ActivateFireWarning);        
    }

    void ActivateFireWarning(GameEvent e)
    {
        StartCoroutine(FireWarningCo());
    }

    IEnumerator FireWarningCo()
    {
        fireWarning.enabled = true;
        yield return new WaitForSecondsRealtime(5);
        yield break;
    }
}
