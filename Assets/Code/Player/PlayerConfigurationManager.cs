using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class PlayerConfigurationManager : MonoBehaviour
{
    public Image test;
    private List<PlayerConfiguration> playerConfigs;
    public Transform horizontalLayoutTransform;
    public GameObject playerPrefab;
    public int selectedCostume;

    public static PlayerConfigurationManager Instance { get; private set; }

    private void Awake()
    {
        //PlayerInputManager.instance.onPlayerJoined += HandlePlayerJoin;
        //SceneManager.activeSceneChanged += OnLevelFinishedLoading;
        SceneManager.sceneLoaded += OnLevelFinishedLoading;

        if (Instance != null)
        {
            Debug.Log("SINGLETON - Trying to creat second instance");
        }
        else
        {
            Instance = this;
            playerConfigs = new List<PlayerConfiguration>();
        }
        
        
    }

    private void OnEnable()
    {
        DontDestroyOnLoad(Instance);
    }

    public void SetPlayerCostume(int index, PlayerMovement costume)
    {
        costume.SetPlayerCostume(index);
        //chose costume at playerConfigs[index]
    }

    public IEnumerator ReadyPlayer(int index, int costumeIndex)
    {
        playerConfigs[index].isReady = true;
        playerConfigs[index].costumeIndex = costumeIndex;
        if (playerConfigs.All(p => p.isReady == true))
        {
            //Load new Scene!
            yield return new WaitForSecondsRealtime(1);
            GameEventManager.Raise(new StartGame());
        }
    }
    public void UnreadyPlayer(int index, int costumeIndex)
    {
        playerConfigs[index].isReady = false;
        playerConfigs[index].costumeIndex = costumeIndex;
    }

    void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
        {
            playerConfigs = new List<PlayerConfiguration>();
            return;
        }
        for (int i = 0; i < playerConfigs.Count; i++)
        {
            
            SceneManager.SetActiveScene(scene);
            Debug.LogError("JoinPlayer count: " + playerConfigs.Count);
            //var player = Instantiate(playerPrefab);
            var player = PlayerInput.Instantiate(playerPrefab, playerConfigs[i].playerIndex , null, -1 , playerConfigs[i].device);
            SetPlayerCostume(playerConfigs[i].costumeIndex ,player.GetComponent<PlayerMovement>());
        }
    }
    
    public void HandlePlayerJoin(PlayerInput pi)
    {


        pi.transform.SetParent(horizontalLayoutTransform);
        pi.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 0.6f);
        
        if (!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            Debug.LogError("Player joined " + pi.playerIndex);
            playerConfigs.Add(new PlayerConfiguration(pi, pi.GetComponent<PlayerInput>().devices[0]));
        }
    }
    
    public void HandlePlayerLeave(PlayerInput pi)
    {
        if (!playerConfigs.Any(p => p.playerIndex == pi.playerIndex))
        {
            Debug.LogError("Player left " + pi.playerIndex);
            playerConfigs.Remove(new PlayerConfiguration(pi, pi.GetComponent<PlayerInput>().devices[0]));
        }
    }
    
}
