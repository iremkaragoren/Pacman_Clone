using UnityEngine;
using System.Collections;
using Events;
using Script.Enemy;

public class GhostMoodAnimation : MonoBehaviour
{
    private SpriteRenderer sourceSpriteRenderer;
    [SerializeField] private Sprite frightenedSpriteRenderer;
    [SerializeField] private Sprite deadthSpriteRenderer;

    [SerializeField] private Sprite[] frightenedMood = new Sprite[2];
    [SerializeField] private float changeInterval = 0.5f;
    private GhostMovementAnim m_ghosMovementAnim;
    
   
    private Coroutine m_coroutine;
    private Sprite originalSprite; 
    private Color originalColor;
    private GhostMovement ghostMovement;
    
    private float timer;
    
    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        m_ghosMovementAnim = GetComponent<GhostMovementAnim>();
        sourceSpriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = sourceSpriteRenderer.sprite;
        originalColor = sourceSpriteRenderer.color;
    }

    private void OnEnable()
    {
        ExternalEvents.TimePaused += OnTimePaused;
    }
    

    public void OnGhostTurnBack()
    {
        sourceSpriteRenderer.sprite = originalSprite;
        sourceSpriteRenderer.color = originalColor;
    }
    
    

    private void OnTimePaused(bool isPaused)
    {
        if (isPaused)
        {
            m_coroutine=StartCoroutine(FrightenedDuring(7));
        }
        else
        {
            TurnBackMood();
        }
    }

    private IEnumerator FrightenedDuring(float totalTime)
    {
        timer = 0;
        int index = 0;
        while (timer <= totalTime)
        {
            m_ghosMovementAnim.OnFrighened();
            sourceSpriteRenderer.sprite = frightenedMood[index];
            index = (index + 1) % frightenedMood.Length;
            // sourceSpriteRenderer.sprite = frightenedSpriteRenderer;
            sourceSpriteRenderer.color = Color.white;
            yield return new WaitForSeconds(changeInterval);
            timer++;
        }
        
        TurnBackMood();
    }

    private void TurnBackMood()
    {
        if (m_coroutine != null)
        {
            StopCoroutine(m_coroutine);
            m_coroutine = null;
        }
        
        sourceSpriteRenderer.sprite = originalSprite;
        sourceSpriteRenderer.color = originalColor;
    }

    private void OnDisable()
    {
        ExternalEvents.TimePaused -= OnTimePaused;
    }
}