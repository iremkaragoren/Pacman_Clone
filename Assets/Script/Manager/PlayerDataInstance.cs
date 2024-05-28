using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataInstance : MonoBehaviour
{
    public static PlayerDataInstance Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
        
    }
    
    
}
