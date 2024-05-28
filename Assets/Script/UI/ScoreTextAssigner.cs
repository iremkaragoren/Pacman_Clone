using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using TMPro;
using UnityEngine;

public class ScoreTextAssigner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private int score;
    public int Score => score;

    private void Start()
    {
        score = 0;
    }

    private void OnEnable()
    {
        InternalEvents.NormalFoodEating += OnNormalFoodAte;
        InternalEvents.ExtraFoodEating += OnExtraFoodAte;
        InternalEvents.PlayerEatenGhost += OnPlayerEatenGhost;
        ExternalEvents.PowerfullFoodReady += OnPowerfullFoodActive;
    }

    private void OnPowerfullFoodActive()
    {
        AddScore(100);
    }

    private void OnPlayerEatenGhost()
    {
        AddScore(200);
    }

    private void OnExtraFoodAte()
    {
        AddScore(50);
    }

    private void OnNormalFoodAte(int normalFood)
    {
        AddScore(10);
    }

    private void AddScore(int amount)
    {
        score += amount;
        scoreText.text = score.ToString();
        ExternalEvents.ScoreChanged?.Invoke(score);
    }

    private void OnDisable()
    {
        InternalEvents.NormalFoodEating -= OnNormalFoodAte;
        InternalEvents.ExtraFoodEating -= OnExtraFoodAte;
        InternalEvents.PlayerEatenGhost -= OnPlayerEatenGhost;
        ExternalEvents.PowerfullFoodReady -= OnPowerfullFoodActive;
    }
}
