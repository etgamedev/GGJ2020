using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject replayOverlay;

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

        if (failOverlay != null)
        {
            failOverlay.SetActive(currentGameState == EGameState.ReadyToEnd);
        }

        if (successOverlay != null)
        {
            successOverlay.SetActive(currentGameState == EGameState.ReadyToEnd);
        }

        if (replayOverlay != null)
        {
            successOverlay.SetActive(currentGameState == EGameState.End);
        }

        SoundManager.Instance.PlayBGM("BGM_MainMenu", 5f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentGameState == EGameState.ReadyToStart)
            {
                StartGame();
            }
            else if (currentGameState == EGameState.End)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void StartGame()
    {
        currentGameState = EGameState.Playing;
        SoundManager.Instance.PlayBGM("BGM_InGame", 5f);
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

            SoundManager.Instance.PlaySFX("SFX_Lose");

            StartCoroutine(FailGameSequence());
        }
    }

    private IEnumerator FailGameSequence()
    {
        if (failOverlay != null)
        {
            failOverlay.SetActive(true);

            var overlayAnimator = failOverlay.GetComponent<Animator>();
            overlayAnimator.SetTrigger("BlackIn");
        }

        yield return new WaitForSeconds(3.0f);

        currentGameState = EGameState.End;

        if (replayOverlay != null)
        {
            replayOverlay.SetActive(true);

            var animator = replayOverlay.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Flash");
        }
    }

    public void WinGame()
    {
        if (currentGameState == EGameState.Playing)
        {
            SoundManager.Instance.PlaySFX("SFX_Win");
            currentGameState = EGameState.ReadyToEnd;

            StartCoroutine(WinGameSequence());
        }
    }

    private IEnumerator WinGameSequence()
    {
        if (successOverlay != null)
            successOverlay.SetActive(true);

        yield return new WaitForSeconds(2.0f);

        currentGameState = EGameState.End;

        if (replayOverlay != null)
        {
            replayOverlay.SetActive(true);

            var animator = replayOverlay.GetComponent<Animator>();
            if (animator != null)
                animator.SetTrigger("Flash");
        }
    }
}
