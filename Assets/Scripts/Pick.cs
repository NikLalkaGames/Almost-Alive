﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnMouseDown() 
    {
        Debug.Log("515");
        GameManager.instance.LoadNextPlayableScene();
    }
}