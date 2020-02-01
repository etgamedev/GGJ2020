using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EGameState
{
    MainMenu,
    ReadyToStart,
    Playing,
    ReadyToEnd,
    End
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public EGameState currentGameState;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Update()
    {
        
    }

    public void PlayerDestroyed()
    {
        if (currentGameState == EGameState.Playing)
        {
            Debug.Log("Player object destroyed, trigger end game");
        }
    }
}
