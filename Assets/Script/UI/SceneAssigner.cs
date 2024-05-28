using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneAssigner : MonoBehaviour
{
    [SerializeField] private GameObject starterPanel;
   
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            starterPanel.SetActive(false);
            ExternalEvents.LevelStart?.Invoke();
        }
        
    }

   
}
