using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    public PlayerInputSystems InputActions { get; private set; }
    public PlayerInputSystems.GamePlayActions PlayerActions { get; private set; }
    
    GameManager gameManager;

    public void Initialize()
    {
        gameManager = GameManager.Instance;
        InputActions = new PlayerInputSystems();
        PlayerActions = InputActions.GamePlay;
        InputActions.Enable();
    }

    private void OnEnable()
    {
        
    }
    private void OnDisable()
    {
        InputActions.Disable();
    }
}
