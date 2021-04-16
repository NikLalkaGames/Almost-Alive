using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance = null;

    [SerializeField] private float delayDuration;

    private Coroutine loadingRoutine;

    private void Awake()
    {
        Debug.Log("SceneLoader Awake");
        if (instance == null) instance = this;
    }

    public void CoroutineLoading(string sceneName)
    {
        if (loadingRoutine == null)
            loadingRoutine = StartCoroutine(LoadScene(sceneName, delayDuration));
    }

    public IEnumerator LoadScene(string sceneName, float delayDuration)
    {
        yield return new WaitForSeconds(delayDuration);
        SceneManager.LoadScene(sceneName);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());   // unload scene async
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

    public void Defeat()
    {
        if (loadingRoutine == null)
            loadingRoutine = StartCoroutine(LoadScene("EntryMenu", 0.01f)); // in future return to restart screen
        Debug.Log("GoToMenu");
    }
}
