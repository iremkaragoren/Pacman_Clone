using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class GameStateHandler : MonoBehaviour
{
   private GameState currentState;
   private int scatterTime = 7;
   private int scatterTime2 = 5;
   private int chaseTime = 20;
   public Timer timer;

   private bool gamePaused;

   public GameState CurrentState
   {
      get { return currentState; }
      private set
      {
         if (currentState != value)
         {
            currentState = value;
            ExternalEvents.StateChanged?.Invoke();
         }
      }
   }
   
   public static GameStateHandler Instance { get; set; }
    
   
   

   private void OnEnable()
   {
      ExternalEvents.GameTimer += OnGameTimer;

      ExternalEvents.TimePaused += OnTimePaused;

   }

   private void OnTimePaused(bool timePaused)
   {
      if (timePaused)
      {
         gamePaused = true;
         CurrentState = GameState.Frightened;
         ExternalEvents.FrightenedMoodOn?.Invoke(true);
      }
      else
      {
         gamePaused = false;
      }
   }
    
   private void OnDisable()
   {
       ExternalEvents.GameTimer -= OnGameTimer;
       ExternalEvents.TimePaused -= OnTimePaused;
      
   }

   private void OnGameTimer(int timer)
   {
      if (timer <=scatterTime)
      {
         CurrentState = GameState.Scatter;
      }
      
      else if (timer >= scatterTime && timer <= scatterTime+chaseTime)
      {
         CurrentState = GameState.Chase;
      }
      
      else if (timer >= scatterTime + chaseTime&& timer<=chaseTime+(2*scatterTime))
      {
         CurrentState = GameState.Scatter;
      }
   
      else if(timer>=chaseTime+(2*scatterTime) && timer<= (2*(chaseTime+scatterTime)))
      {
         CurrentState = GameState.Chase;
      }
      
      else if (timer >= (2 * (chaseTime + scatterTime)) && timer <= (2 * (chaseTime + scatterTime)) + scatterTime2)
      {
         CurrentState = GameState.Scatter;
      }
   
      else if(timer >= (2 * (chaseTime + scatterTime)) + scatterTime2 && timer <= (2 * (chaseTime + scatterTime)) + scatterTime2+chaseTime)
      {
         CurrentState = GameState.Chase;
      }
       
      else if (timer >= (2 * (chaseTime + scatterTime)) + scatterTime2 + chaseTime && timer<=(2 * (chaseTime + scatterTime)) + scatterTime2 + scatterTime2)
      {
         CurrentState = GameState.Scatter;
      }
   
      else
      {
         CurrentState = GameState.Chase;
      }
       
   }

   private void Awake()
   {
      if (Instance ==null)
      {
         Instance = this;
      }

      else
      {
         Destroy(this);
      }
   }
   
}


public enum GameState
{
   Scatter,
   Chase,
   Frightened
}