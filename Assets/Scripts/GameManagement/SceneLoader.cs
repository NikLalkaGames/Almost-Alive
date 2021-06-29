using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance = null;

    [SerializeField] private float _delayDuration;

    private Coroutine _loadingRoutine;

    private void Awake()
    {
        Debug.Log("SceneLoader Awake");
        if (instance == null) instance = this;
    }

    public void CoroutineLoading(string sceneName)
    {
        if (_loadingRoutine == null)
            _loadingRoutine = StartCoroutine(LoadScene(sceneName, _delayDuration));

        _loadingRoutine = null;
    }

    public IEnumerator LoadScene(string sceneName, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoad;    // subscribe to event on scene load
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        SceneManager.sceneLoaded += OnSceneLoad;    // subscribe to event on scene load
    }

    private void OnSceneLoad(Scene loadedScene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + loadedScene.name);
        SetActivePlayableScene(loadedScene);
        GameManager.instance.RefreshGamingStats();
        
        SceneManager.sceneLoaded -= OnSceneLoad;
        Debug.Log("OnSceneLoad actions has called successfully");
    }

    private void SetActivePlayableScene(Scene current)
    {
        SceneManager.SetActiveScene(current);
        Debug.Log("Active scene: " + SceneManager.GetActiveScene().name);
    }
}
