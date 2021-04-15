using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [SerializeField] private SceneLoader SceneLoader = new SceneLoader();

    private PlayerController playerController;

    private GameObject playerUI;

    private void Awake() 
    {
        Debug.Log("GameManager Awake");
        if (instance == null) instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        Application.targetFrameRate = 60;
        StartCoroutine(SceneLoader.LoadScene("EntryMenu", 0.5f));
    }

    public void RefreshGamingStats()
    {
        // may be more code
    }

    private void RefreshPlayerData()
    {

    }
}