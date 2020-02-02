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
    public GameObject menuOverlay;
    public GameObject failOverlay;
    public GameObject successOverlay;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        if (menuOverlay != null)
        {
            menuOverlay.SetActive(currentGameState == EGameState.ReadyToStart);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentGameState == EGameState.ReadyToStart)
            {
                StartGame();
            }
        }
    }

    private void StartGame()
    {
        currentGameState = EGameState.Playing;
        
        if (menuOverlay != null)
            menuOverlay.SetActive(false);
    }

    public void PlayerDestroyed()
    {
        if (currentGameState == EGameState.Playing)
        {
            Debug.Log("Player object destroyed, trigger end game");
        }
    }

    public void FailGame()
    {
        if (currentGameState == EGameState.Playing)
        {
            currentGameState = EGameState.ReadyToEnd;

            StartCoroutine(FailGameSequence());
        }
    }

    private IEnumerator FailGameSequence()
    {
        yield return null;
    }

    public void WinGame()
    {
        if (currentGameState == EGameState.Playing)
        {
            currentGameState = EGameState.ReadyToEnd;
        }
    }
}
