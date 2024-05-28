using System;
using System.Collections;
using Events;
using Script;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : BaseMovement
{
    [SerializeField] private GameObject[] pacmanHealth = new GameObject[2];
    private Vector2 m_startPosition;
    private int m_currentIndex = 0;
    private Vector2 m_nextDirection = new(-1, 0);
    private Animator m_animator;
    private float m_deathAnimationDuration;
    private bool m_hasReceivedFirstInput;
    private CircleCollider2D m_collider;

    private Coroutine coroutine;

    private bool m_frightenedMood;
    private bool m_levelStart;
    private bool m_isPlayerDeath;
    private bool m_levelCompleted = false;

    public static PlayerMovement Instance { get; set; }

    protected override void Awake()
    {
        base.Awake();

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Initialize();
        m_animator.enabled = false;
    }
    
    private void Initialize()
    {
        m_animator = GetComponent<Animator>();
        m_collider = GetComponent<CircleCollider2D>();
        m_startPosition = transform.position;
    }

    private void Start()
    {
        AnimationClip[] clips = m_animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip clip in clips)
        {
            if (clip.name == "Die")
            {
                m_deathAnimationDuration = clip.length;
            }
        }
    }
    
    protected override void Update()
    {
        if (!m_levelCompleted && m_levelStart && !m_isPlayerDeath)
        {
            HandleInput();
            MoveIfNeeded();
        }
    }

    private void OnEnable()
    {
        ExternalEvents.LevelStart += OnLevelStart;
        ExternalEvents.LevelComplete += OnLevelCompleted;
        ExternalEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        ExternalEvents.LevelStart -= OnLevelStart;
        ExternalEvents.LevelComplete -= OnLevelCompleted;
        ExternalEvents.GameOver -= OnGameOver;
    }

    private void OnLevelStart()
    {
        m_animator.enabled = true;
        m_levelStart = true;
    }

    private void OnGameOver()
    {
        transform.position = m_startPosition;
    }

    private void OnLevelCompleted()
    {
        
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(WaitAndEnableAnimator(m_deathAnimationDuration));
       
    }

    IEnumerator WaitAndEnableAnimator(float duration)
    {
        m_levelCompleted = true;
        m_isPlayerDeath = true;
        IsStopped = true;
        m_rb.velocity = Vector2.zero;
        m_animator.SetTrigger("Die");
        m_rb.isKinematic = true;
        
        yield return new WaitForSeconds(duration);
        
        foreach (GameObject health in pacmanHealth)
        {
            if (health != null)
            {
                health.SetActive(true);
            }
        }
        transform.position = m_startPosition;
        
        m_animator.ResetTrigger("Die");
        
        m_levelCompleted = false;
        m_isPlayerDeath = false;
        IsStopped = false;
        m_rb.isKinematic = false;
    }

   

    public Vector2 PlayerPos()
    {
        return transform.position;
    }

    public Vector2 PlayerDir()
    {
        return direction;
    }

  

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
            vertical = 0;
        else
            horizontal = 0;

        Vector2 inputDirection = new Vector2(horizontal, vertical).normalized;
        if (!m_hasReceivedFirstInput) inputDirection = m_nextDirection;
        if (inputDirection != Vector2.zero)
            m_nextDirection = inputDirection;
        if ((!m_hasReceivedFirstInput || IsAtNode) && CanMoveInDirection(m_nextDirection))
        {
            ChangeDirection(m_nextDirection);
            InternalEvents.PlayerDirectionChanged?.Invoke(m_nextDirection);
        }
    }

    private void ChangeDirection(Vector2 newDirection)
    {
        direction = newDirection;
        m_hasReceivedFirstInput = true;
        IsStopped = false;
    }

    private void MoveIfNeeded()
    {
        if (!IsStopped)
        {
            Move();
            m_animator.enabled = true;
        }
    }

    private bool CanMoveInDirection(Vector2 dir)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Character");
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 1, layerMask);
        Debug.DrawRay(transform.position, dir * 1.0f, Color.green);

        if (hit.collider != null)
            if (hit.collider.TryGetComponent<BorderDetector>(out _))
            {
                return false;
            }

        return true;
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out NodeDetector node))
        {
            IsAtNode = true;
            this.gameObject.transform.position = node.transform.position;
        }

        if (other.TryGetComponent<GhostMovement>(out GhostMovement ghost))
        {
            if (GameStateHandler.Instance.CurrentState != GameState.Frightened)
            {
                StartDieAnimation();
                InternalEvents.PlayerDeath?.Invoke();
            }
            else
            {
                ghost.PlayerEatenGhost();
            }
        }
    }

    private void StartDieAnimation()
    {
        m_isPlayerDeath = true;
        IsStopped = true;
        m_rb.velocity = Vector2.zero;
        m_animator.SetTrigger("Die");
        m_rb.isKinematic = true;

        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }

        coroutine = StartCoroutine(WaitForAnimation(m_deathAnimationDuration));
    }

    IEnumerator WaitForAnimation(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnPlayerAnimationComplete();
        InternalEvents.PlayerDeathAnimationComplete?.Invoke();
    }

    private void OnPlayerAnimationComplete()
    {
        if (m_currentIndex < pacmanHealth.Length)
        {
            if (pacmanHealth[m_currentIndex] != null)
            {
                pacmanHealth[m_currentIndex].SetActive(false);
            }

            m_currentIndex++;

            if (m_currentIndex < pacmanHealth.Length)
            {
                transform.position = m_startPosition;
                m_animator.ResetTrigger("Die");
                IsStopped = false;
                m_rb.isKinematic = false;
                m_isPlayerDeath = false;
            }
            else
            {
                string sceneName = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(sceneName);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out NodeDetector node))
        {
            IsAtNode = false;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<BorderDetector>(out _))
        {
            Vector2 collisionDirection =
                (other.contacts[0].point - new Vector2(transform.position.x, transform.position.y)).normalized;
            if (Vector2.Dot(collisionDirection, direction) > 0.5) Stop();

            m_animator.enabled = false;
        }
    }

    private void Stop()
    {
        m_rb.velocity = Vector2.zero;
        IsStopped = true;
        IsAtNode = true;
    }
}
