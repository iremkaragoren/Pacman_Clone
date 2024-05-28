using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private int m_timer;
    private int m_pausedTimer;
    private bool m_gamePaused = false;
    private bool m_levelStart;
    
    
    private Coroutine m_gameTimerCoroutine;
   

    private void OnEnable()
    {
        ExternalEvents.LevelStart += OnLevelStart;
        ExternalEvents.LevelComplete += OnLevelComplete;
        InternalEvents.PlayerDeath += OnPlayerDeath;
        InternalEvents.ExtraFoodEating += OnExtraFootAte;
        InternalEvents.PlayerDeathAnimationComplete += OnPlayerTurnPosition;
        InternalEvents.EnemyOnNode += OnEnemyOnNode;

    }

    private void OnEnemyOnNode()
    {
       
        if (m_gamePaused)
        {
            m_gamePaused = false;
            m_pausedTimer = 0;
            ExternalEvents.TimePaused?.Invoke(false); 
        }

       
        if (m_gameTimerCoroutine != null)
        {
            StopGameTimerCoroutine();
            m_gameTimerCoroutine = null;
        }

        
        m_timer = 0; 
        m_gameTimerCoroutine = StartCoroutine(GameTimer());
    }


    private void OnPlayerDeath()
    {
        StopGameTimerCoroutine();
    }

    private void OnPlayerTurnPosition()
    {
        m_gameTimerCoroutine = StartCoroutine(GameTimer());
        Debug.Log("turn");
    }


    private void OnLevelComplete()
    {
        StopGameTimerCoroutine();
      
    }

    private void StopGameTimerCoroutine()
    {
        if (m_gameTimerCoroutine != null)
        {
            StopCoroutine(m_gameTimerCoroutine);
            m_gameTimerCoroutine = null;
        }

        m_timer = 0;

        
    }

    private void OnExtraFootAte()
    {
        m_gamePaused = true;
        ExternalEvents.TimePaused?.Invoke(true);
        m_gameTimerCoroutine = StartCoroutine(PausedTime());
    }

    IEnumerator PausedTime()
    {
        m_pausedTimer = 0;
        
        while (m_gamePaused)
        {
            yield return new WaitForSeconds(1.0f);
            m_pausedTimer++;
            
            if (m_pausedTimer >= 7)
            {
                m_gamePaused = false;
                ExternalEvents.TimePaused?.Invoke(false);
            }
        }
    }
    
    IEnumerator GameTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (!m_gamePaused)
            {
                m_timer++;
                ExternalEvents.GameTimer?.Invoke(m_timer);
            }
            
        }
    }
    
    private void OnLevelStart()
    {
        m_gameTimerCoroutine= StartCoroutine(GameTimer());
    }
    
    private void OnDisable()
    {
        ExternalEvents.LevelComplete -= OnLevelComplete;
        InternalEvents.PlayerDeath -= OnPlayerDeath;
        InternalEvents.ExtraFoodEating -= OnExtraFootAte;
        ExternalEvents.LevelStart -= OnLevelStart;
        InternalEvents.PlayerDeathAnimationComplete -= OnPlayerTurnPosition;
        InternalEvents.EnemyOnNode -= OnEnemyOnNode;
    }
}
