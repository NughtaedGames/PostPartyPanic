using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using TMPro;
using UnityEngine;

public class GameMechanics : MonoBehaviour
{
    public IntInstance amountOfMaxCleanupObjects;
    public IntInstance amountOfCleanedUpObjects;

    public IntInstance amountOfMaxCups;
    public IntInstance amountOfCleanedCups;

    public TextMeshProUGUI missingAmountText;
    public TextMeshProUGUI missingPErcentageCups;

    public GameObject timer;
    
    private void Awake()
    {
        GameEventManager.AddListener<IncreaseMaxAmountOfCleanupObjects>(IncreaseMaxAmountOfCleanupObjects);
        GameEventManager.AddListener<DecreaseMaxAmountOfCleanupObjects>(DecreaseMaxAmountOfCleanupObjects);
        GameEventManager.AddListener<IncreaseAmountOfCleanedObjects>(IncreaseAmountOfCleanedObjects);
        GameEventManager.AddListener<DecreaseAmountOfCleanedObjects>(DecreaseAmountOfCleanedObjects);
        GameEventManager.AddListener<RestartGame>(ResetAmounts);
        GameEventManager.AddListener<StartGame>(ResetAmounts2);

        GameEventManager.AddListener<SpawnCup>(CountCups);
        GameEventManager.AddListener<DestroyCup>(CountCollectedCups);
        missingPErcentageCups.text = "Cleaned house: 0%";

               ResetAmounts();
    }
    private void CountCups(SpawnCup e)
    {
        if (amountOfMaxCups.Integer < 500)
        {
            amountOfMaxCups.Integer++;
        }
        else
        {
            Debug.LogError("Holdon! There are WAYYYY too many cups, check your Global Variable!");
        }

    }
    private void CountCollectedCups(DestroyCup e)
    {
        int percentage = amountOfCleanedCups.Integer * 100 / (amountOfMaxCups.Integer - 4);
        if (percentage <= 100)
        {
            amountOfCleanedCups.Integer++;
            missingPErcentageCups.text = "Cleaned House: " + percentage + "%";
        }
        else
        {
            Debug.Log("HOLDON A MINUTE! There are" + percentage + " of cleaned cups! Check your daamn code");
        }

    }
    public void ActivateUI()
    {
        timer.SetActive(true);
        missingAmountText.gameObject.SetActive(true);
    }
    
    private void IncreaseMaxAmountOfCleanupObjects(IncreaseMaxAmountOfCleanupObjects e)
    {
        amountOfMaxCleanupObjects.Integer++;
        UpdateText();
    }
    private void DecreaseMaxAmountOfCleanupObjects(DecreaseMaxAmountOfCleanupObjects e)
    {
        amountOfMaxCleanupObjects.Integer--;
        UpdateText();
    }

    private void IncreaseAmountOfCleanedObjects(IncreaseAmountOfCleanedObjects e)
    {
        amountOfCleanedUpObjects.Integer++;
        UpdateText();
    }

    private void DecreaseAmountOfCleanedObjects(DecreaseAmountOfCleanedObjects e)
    {
        amountOfCleanedUpObjects.Integer--;
        UpdateText();
    }

    private void ResetAmounts()
    {
        amountOfMaxCleanupObjects.Integer = 0;
        amountOfCleanedUpObjects.Integer = 0;
        UpdateText();
    }
    
    private void ResetAmounts(RestartGame e)
    {
        amountOfMaxCleanupObjects.Integer = 0;
        amountOfCleanedUpObjects.Integer = 0;
        UpdateText();
        amountOfMaxCups.Integer = 0;
    }
    private void ResetAmounts2(StartGame e)
    {
        amountOfMaxCups.Integer = 0;
    }

    private void UpdateText()
    {
        missingAmountText.text = "Placed Furniture: " + amountOfCleanedUpObjects.Integer + "/" + amountOfMaxCleanupObjects.Integer;
    }
}
