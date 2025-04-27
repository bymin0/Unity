using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    InputReader _inputReader;
    StarterManager _starterManager;
    public StarterManager StarterManager { get { return _starterManager; } }

    public bool IsPlacementMode { get; private set; } = true;
    private int startCount = 0;
    private const int maxStartCount = 3;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Initialize();
            InitializeInput();
        }
        else
            Destroy(this);
    }

    private void Initialize()
    {
        _inputReader = FindObjectOfType<InputReader>();
        _starterManager = FindObjectOfType<StarterManager>();

        _inputReader.Initialize();
    }
    private void OnDestroy()
    {
        _inputReader.PlayerActions.StartChain.performed -= OnStartChain;
    }

    private void InitializeInput()
    {
        DominoPlacer dominoPlacer = FindObjectOfType<DominoPlacer>();
        _inputReader.PlayerActions.PlaceDimino.performed += dominoPlacer.OnPlaceDomino;
        _inputReader.PlayerActions.StartChain.performed += OnStartChain;
    }
    public void OnStartChain(InputAction.CallbackContext context)
    {
        //if (!IsPlacementMode) return;
        startCount++;
        IsPlacementMode = false;

        ActivateAllDominos();
    }

    private void ActivateAllDominos()
    {
        Domino[] dominos = FindObjectsOfType<Domino>();
        foreach (Domino domino in dominos)
            domino.Activate();

        _starterManager.SpawnStarter();
    }

    public void ResetToPlacementMode()
    {
        IsPlacementMode = true;
    }
}
