using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera m_mainCamera;
    private float m_screenWidth;
    private float m_screenHeight;

    void Start()
    {
        m_mainCamera = Camera.main;
        
        m_screenWidth = m_mainCamera.orthographicSize * 2f * m_mainCamera.aspect;
        m_screenHeight = m_mainCamera.orthographicSize * 2f;
    }

    void Update()
    {
        
        Vector3 characterPosition = transform.position;

       
        if (characterPosition.x > m_screenWidth / 4f)
        {
            characterPosition.x = -m_screenWidth / 4f;
        }
        
        else if (characterPosition.x < -m_screenWidth / 4f)
        {
            characterPosition.x = m_screenWidth / 4f;
        }

       
        if (characterPosition.y > m_screenHeight / 2f)
        {
            characterPosition.y = -m_screenHeight / 2f;
        }
       
        else if (characterPosition.y < -m_screenHeight / 2f)
        {
            characterPosition.y = m_screenHeight / 2f;
        }
        
        transform.position = characterPosition;
    }
}
