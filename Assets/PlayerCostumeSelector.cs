using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCostumeSelector : MonoBehaviour
{

    //public TextMeshProUGUI player;
    public GameObject readyDisplay;
    public GameObject readyButton;
    public Transform inputTypeIcons;
    //public GameObject readyButton;
    
    public int playerIndex;
    
    public Transform costumeList;
    public int currentCostumeIndex = 0;
    [SerializeField]
    private CostumeListInstance costumeListInstance;

    private bool canReady;

    private void Awake()
    {
        SetPlayerIndex(this.GetComponent<PlayerInput>().playerIndex);
        GameEventManager.AddListener<StartGame>(deleteOnStart);

        if (this.GetComponent<PlayerInput>().currentControlScheme == "Keyboard")
        {
            inputTypeIcons.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            inputTypeIcons.GetChild(1).gameObject.SetActive(true);
        }

        canReady = false;

    }

    public void SetPlayerIndex(int pi)
    {
        //player.SetText("Player " + (pi + 1).ToString());
        playerIndex = pi;
    }

    private void Start()
    {
        canReady = true;
    }

    public void ChangeCostume(int change)
    {
        UnreadyPlayer();
        AkSoundEngine.PostEvent("Take_Photo", gameObject);
        currentCostumeIndex += change;
        if (currentCostumeIndex > costumeListInstance.value.costumeList.Count - 1)
        {
            currentCostumeIndex = 0;
        }
        else if (currentCostumeIndex < 0)
        {
            currentCostumeIndex = costumeListInstance.value.costumeList.Count - 1;
        }

        for (int i = 0; i < costumeList.childCount; i++)
        {
            if (costumeList.GetChild(i).name == costumeListInstance.value.costumeList[currentCostumeIndex].imageName.name)
            {
                costumeList.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                costumeList.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    
    //Called by PlayerInput
    public void IncreaseCostume(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            Debug.LogError("+1");
            ChangeCostume(+1);
        }
    }
    
    public void DecreaseCostume(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            Debug.LogError("-1");
            ChangeCostume(-1);
        }
    }

    public void ReadyPlayer(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid() && canReady)
        {
            AkSoundEngine.PostEvent("Click", gameObject);
            StartCoroutine(PlayerConfigurationManager.Instance.ReadyPlayer(playerIndex, currentCostumeIndex));
            readyDisplay.SetActive(true);
            readyButton.SetActive(false);
        }
    }
    
    public void UnreadyPlayer()
    { 
        PlayerConfigurationManager.Instance.UnreadyPlayer(playerIndex, currentCostumeIndex);
        readyDisplay.SetActive(false);
        readyButton.SetActive(true);
    }

    public void deleteOnStart(StartGame e)
    {
        GameEventManager.RemoveListener<StartGame>(deleteOnStart);
        Destroy(this.gameObject);
    }
}
