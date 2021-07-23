using System.Collections;
using System.Collections.Generic;
using GameManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToGame()
    {
        SceneLoader.instance.CoroutineLoading("MainScene");
        Debug.Log("GoToGame");
    }

    public void GoToMovements()
    {
        SceneLoader.instance.CoroutineLoading("MovementScene");
        Debug.Log("GoToMovements");
    }

    public void GoToCreators()
    {
        SceneLoader.instance.CoroutineLoading("CreatorsScene");
        Debug.Log("GoToCreators");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}