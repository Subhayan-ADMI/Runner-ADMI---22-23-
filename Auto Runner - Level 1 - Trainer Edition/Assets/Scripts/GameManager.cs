using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameStates
    {
        WAIT,
        GAMEPLAY,
        PAUSE,
        VICTORYSCREEN,
        LOSINGSCREEN
    }

    public GameStates currentGameState;

    public static GameManager instance;

    public int counter = 3;

    bool isCounting = false;

    public GameObject pauseMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameStates.WAIT;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentGameState)
        {
            case GameStates.WAIT:

                if (!isCounting)
                {
                    isCounting = true;
                    StartCoroutine(CountdowntoStart());
                }

                break;

            case GameStates.GAMEPLAY:

                if (pauseMenu.activeInHierarchy)
                {
                    pauseMenu.SetActive(false);
                }

                break;

            case GameStates.PAUSE:

                if (!pauseMenu.activeInHierarchy)
                {
                    pauseMenu.SetActive(true);
                }

                break;

            case GameStates.VICTORYSCREEN:

                break;

            case GameStates.LOSINGSCREEN:

                break;
        }
    }

    IEnumerator CountdowntoStart()
    {
        while (counter > 0)
        {
            yield return new WaitForSeconds(1f);
            counter--;
        }

        StartGame();
    }

    void StartGame()
    {
        isCounting = false;

        if (currentGameState != GameStates.GAMEPLAY)
        {
            currentGameState = GameStates.GAMEPLAY;
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (currentGameState != GameStates.PAUSE)
        {
            currentGameState = GameStates.PAUSE;
        }
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        if (currentGameState != GameStates.GAMEPLAY)
        {
            currentGameState = GameStates.GAMEPLAY;
        }
    }

    public void RestartLevel()
    {
        
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
