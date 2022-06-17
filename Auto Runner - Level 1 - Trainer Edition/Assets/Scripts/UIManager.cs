using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [Header(("UI elements references"))] 
    public TMP_Text scoreText; // Reference to the score Text element 

    public Slider progressSlider; // Reference to the Slider
    public Button PauseButton; // Reference to the pause button

    [Header("Total number of pickups on the scene")]
    [SerializeField] private int totalPickups; // the total number of pickups on the scene

    [Header("Game over Panel")] 
    public GameObject GameOverPanel;

    private int currentPickups; // the number of pickups left
    
    public static UIManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if(Instance != this)
            Destroy(gameObject);
        
    }

    // Start is called before the first frame update
    void Start()
    {
        GameOverPanel.SetActive(false); // deactivate the game over panel at the start of the level
        totalPickups = GameObject.FindGameObjectsWithTag("Pickup").Length; // we calculate the total number of pickups on the level
        currentPickups = 0; // we initialize the number of pickups we currently have
        Debug.Log($"Total number of pickups = {totalPickups}");
        progressSlider.maxValue = totalPickups; // we change the max value of the slider to be the total number of pickups
        progressSlider.value = 0; // We initialize the slider at zero
    }

    public void UpdateScore() // called to update the score
    {
        currentPickups++; // we increment the number of pickups that we have by 1
        progressSlider.value = currentPickups; // this is going to show the number of pickups that we have 
    }

    public void GameOver() // Called when the game is over and shows the game over panel
    {
        GameOverPanel.SetActive(true); // we show the game over panel
        Time.timeScale = 0f;  // we stop time
    }

    public void RestartGame()
    {
        Debug.Log("Restarting level");
        SceneManager.LoadScene("Level1"); // Loading level1
    }
    
    public void StartGame()
    {
        Debug.Log("Restarting level");
        SceneManager.LoadScene("Level1"); // Loading level1
    }

    public void ExitGame()
    {
        Debug.Log("Quitting the game");
        Application.Quit();
    }
}
