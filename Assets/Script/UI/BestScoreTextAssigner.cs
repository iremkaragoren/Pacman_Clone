using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class BestScoreTextAssigner : MonoBehaviour
{
    [SerializeField] private ScoreTextAssigner scoreTextAssigner;
    [SerializeField] private TextMeshProUGUI bestScoreText;

    private readonly string myReadOnlyString = "High Score";
    private int bestScore;

    private void OnEnable()
    {
        ExternalEvents.ScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(int score)
    {
        bestScore = PlayerPrefs.GetInt(myReadOnlyString, 1);
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(myReadOnlyString, score);
            PlayerPrefs.Save();  
        }
        bestScoreText.text = bestScore.ToString();
    }

    private void BestScoreAssigner()
    {
        bestScore = PlayerPrefs.GetInt(myReadOnlyString, 0);
        int score = scoreTextAssigner.Score;
        if (score >= bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(myReadOnlyString,score);
            bestScoreText.text = bestScore.ToString();
        }
        else
        {
            bestScoreText.text = bestScore.ToString();
        }
    }

    private void OnDisable()
    {
        ExternalEvents.ScoreChanged -= OnScoreChanged;
    }
}
