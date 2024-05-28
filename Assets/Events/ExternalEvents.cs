using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ExternalEvents
{
    public static UnityAction LevelStart;
    public static UnityAction LevelFail;
    public static UnityAction LevelComplete;
    public static UnityAction GameOver;
    public static UnityAction<int> GameTimer;
    public static UnityAction StateChanged;
    public static UnityAction<bool> TimePaused;
    public static UnityAction<int> ScoreChanged;
    public static UnityAction LevelUp;
    public static UnityAction<bool> FrightenedMoodOn;
    public static UnityAction PowerfullFoodReady;




}
