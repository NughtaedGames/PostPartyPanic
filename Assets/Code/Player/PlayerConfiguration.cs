using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
    
public class PlayerConfiguration
{

    public PlayerConfiguration(PlayerInput playerInput, InputDevice dev)
    {
        playerIndex = playerInput.playerIndex;
        Input = playerInput;
        device = dev;
    }
    
    public PlayerInput Input { get; set; }
    public int playerIndex { get; set; }
    public int costumeIndex { get; set; }
    public bool isReady { get; set; }
    
    public InputDevice  device { get; set; }
}
