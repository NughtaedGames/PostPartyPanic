using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICode : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            new WaitForSeconds(10);
            Debug.Log("Did uwu try to ESCAPE?? UwU");
            if (SceneManager.GetSceneByName("Ui station").isLoaded == false)
            {
                SceneManager.LoadSceneAsync("Ui station", LoadSceneMode.Additive);
            }
            else
                SceneManager.UnloadSceneAsync("Ui station");
        }
    }
}
