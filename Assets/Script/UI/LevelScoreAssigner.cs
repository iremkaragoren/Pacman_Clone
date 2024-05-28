using UnityEngine;
using TMPro;

public class LevelScoreAssigner : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelScoreText;

    private readonly string levelScore = "LevelScore";
    private int score;

    private void OnEnable()
    {
        ExternalEvents.LevelComplete += OnLevelUp;
        if (PlayerPrefs.HasKey(levelScore))
        {
            PlayerPrefs.DeleteKey(levelScore);
        }
        score = PlayerPrefs.GetInt(levelScore, 1);
        levelScoreText.text = score.ToString();
    }

    private void OnLevelUp()
    {
        score += 1;
        levelScoreText.text = score.ToString();
        PlayerPrefs.SetInt(levelScore, score);
    }

    private void OnDisable()
    {
        ExternalEvents.LevelComplete -= OnLevelUp;
    }
}