using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    
    [SerializeField] private SceneLoader _sceneLoader;

    private PlayerController _playerController;

    private GameObject _playerUI;

    private void Awake() 
    {
        Debug.Log("GameManager Awake");
        if (instance == null) instance = this;
        Application.targetFrameRate = 60;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start() 
    {
        StartCoroutine(SceneLoader.instance.LoadScene("EntryMenu", 0.5f));
    }

    public void RefreshGamingStats()
    {
        // may be more code
    }

    private void RefreshPlayerData()
    {

    }
}