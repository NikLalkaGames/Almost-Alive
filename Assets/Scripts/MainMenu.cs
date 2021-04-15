using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : SceneLoader
{
    public void GoToGame()
    {
        if (loadingRoutine == null)
            loadingRoutine = StartCoroutine(LoadScene("MainScene", 0.3f));
        Debug.Log("GoToGame");
    }

    public void GoToMovements()
    {
        if (loadingRoutine == null)
            loadingRoutine = StartCoroutine(LoadScene("MovementScene", 0.3f));
        Debug.Log("GoToMovements");
    }

    public void GoToCreators()
    {
        if (loadingRoutine == null)
            loadingRoutine = StartCoroutine(LoadScene("CreatorsScene", 0.3f));
        Debug.Log("GoToCreators");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}