using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    private void OnMouseDown() 
    {
        Debug.Log("ButtonPlayPressed");
        GameManager.instance.LoadNextPlayableScene();
    }
}
