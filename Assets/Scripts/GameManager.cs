using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private PlayerController playerController;
    private GameObject playerUI;
    public static int currectSceneIndex = 0;
    private bool gameOver;

    private void Awake() {
        Debug.Log("GameManager Awake");
        if (instance == null) instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() {

    }

    private void Update() {
        ActionHandler();
    }

    public void ActionHandler()
    {
        // if in gameover scene player can reload level or return to entry menu
        if (gameOver)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                RefreshGamingStats();
                RelaodLastPlayableScene();
                gameOver = false; // do not enter in gameover if block in next update
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene("EntryMenu");
                currectSceneIndex = 0;
                Destroy(this.gameObject);
                gameOver = false; // do not enter in gameover if block in next update
            }

        }

        // if not in menu then player can stop the game by pressing escape
        if (currectSceneIndex != 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 0;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
            }
        }
    }

    public void LoadNextPlayableScene()
    {
        SceneManager.LoadScene($"MainScene");
    }

    private IEnumerator LoadNextPlayableSceneWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene($"MainScene");
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void RelaodLastPlayableScene()
    {
        SceneManager.LoadScene($"MainScene");
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        SetActivePlayableScene();
        RefreshGamingStats();

        Debug.Log("OnSceneLoad actions has called successfully");
    }

    private void SetActivePlayableScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName($"MainScene"));
        Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
    }

    public void Defeat()
    {
        gameOver = true;    // in update if manager enter restart scene
        SceneManager.sceneLoaded -= OnSceneLoad; // unsubsribe scene load functioning stack
        SceneManager.LoadScene("GameOver");
    }

    public void RefreshGamingStats()
    {
        // may be more code
    }

    private void RefreshPlayerData()
    {

    }
}