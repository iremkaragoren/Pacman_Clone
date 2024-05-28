using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Script;
using Script.Enemy;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public abstract class GhostMovement : BaseMovement
{
    private List<NodeDetector> _currentNodesNeighbour = new();
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private CircleCollider2D _collider2D;
    [SerializeField] private GhostMovementAnim _ghostMovementAnim;
    [SerializeField] private GhostMoodAnimation _ghostMoodtAnim;
    public NodeDetector CurrentNode;

    [SerializeField] private LayerMask doorMask;
    protected NodeDetector LastVisited { get; private set; }
    
    [SerializeField] private List<GameObject> m_scatterNodesList = new();
    private int m_currentIndex = 0;
    
    private Vector2 staterPosition;
    
    private bool levelStart;

    public bool ghostEaten;

    private bool stateChanged;

    protected override void Awake()
    {
        base.Awake();
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _collider2D = GetComponent<CircleCollider2D>();
        _ghostMovementAnim = GetComponent<GhostMovementAnim>();
        m_currentIndex = 0;
        staterPosition = transform.position;
    }

    protected virtual void OnEnable()
    {
        ExternalEvents.LevelStart += OnLevelStart;
        ExternalEvents.StateChanged += OnStateChanged;
        InternalEvents.PlayerDeath += OnPlayerDeath;
        InternalEvents.PlayerDeathAnimationComplete += OnPlayerAnimComplete;
        ExternalEvents.LevelComplete += OnLevelCompleted;
    }

    private void OnLevelStart()
    {
        levelStart = true;
        
    }

    protected virtual void OnDisable()
    {
        ExternalEvents.StateChanged -= OnStateChanged;
        ExternalEvents.LevelComplete -= OnLevelCompleted;
        InternalEvents.PlayerDeath -= OnPlayerDeath;
        InternalEvents.PlayerDeathAnimationComplete -= OnPlayerAnimComplete;
    
    }

    public void PlayerEatenGhost()
    {
        ghostEaten = true;
        transform.position = staterPosition;
        _ghostMoodtAnim.OnGhostTurnBack();
        StartCoroutine(WaitDieMood());
    }
    
    IEnumerator WaitDieMood()
    {
        _ghostMoodtAnim.OnGhostTurnBack();
        yield return new WaitForSeconds(3f);
    }
    protected override void Update()
    {
        if (levelStart)
        {
            MoveIfNeeded();
            
        }
    }

    private void OnLevelCompleted()
    {
        StartCoroutine(WaitAndResetPositions());
        _spriteRenderer.enabled = false;
        Stop();
    }

    IEnumerator WaitAndResetPositions()
    {
        yield return new WaitForSeconds(1.2f);
        ResetGhost();
        
        yield return new WaitForSeconds(1);
        
        ChangeDirection();
        
    }
    

    private void OnPlayerAnimComplete()
    {
        ResetGhost();
    }

    public void ResetGhost()
    {
        transform.position = staterPosition;
        _spriteRenderer.enabled = true;
        _collider2D.enabled = true;
        Collider2D tmpNode = Physics2D.OverlapCircle(staterPosition, 0.5f,doorMask);
        
        if(tmpNode && tmpNode.TryGetComponent(out NodeDetector triggeredNode))
        {
             NodeDetection(triggeredNode);
        }
        InternalEvents.EnemyOnNode?.Invoke();
        
    }

    private void OnPlayerDeath()
    {
        
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        
    }

    private void OnStateChanged()
    {
        stateChanged = true;
    }
    
    protected override void OnTriggerEnter2D(Collider2D other)
    {
            if (other.TryGetComponent(out NodeDetector triggeredNode))
            {
                NodeDetection(triggeredNode);
            }
    }

    private void NodeDetection(NodeDetector triggeredNode)
    {
            this.CurrentNode = triggeredNode;
            IsAtNode = true;
            this.gameObject.transform.position = triggeredNode.transform.position;
            _currentNodesNeighbour = triggeredNode.Neighbors;
            direction = CalculateDirectionWithNeighbours(_currentNodesNeighbour);
            _ghostMovementAnim.OnDirectionChanged(direction);
    }


    protected virtual Vector2 CalculateDirectionWithNeighbours(List<NodeDetector> currentNodesNeighbour)
    {
        Vector2 tempDirection;

        if (GameStateHandler.Instance.CurrentState == GameState.Scatter)
        {
            if (Vector2.Distance(transform.position, m_scatterNodesList[m_currentIndex].transform.position) < 0.1)
            {
                m_currentIndex = (m_currentIndex + 1) % m_scatterNodesList.Count;
            }
            
            tempDirection = CalculateScatterTarget(currentNodesNeighbour.Where(n => n != LastVisited).ToList(), m_scatterNodesList, m_currentIndex) - (Vector2)CurrentNode.transform.position;
            LastVisited = CurrentNode; 
        }
        else if (GameStateHandler.Instance.CurrentState == GameState.Frightened)
        {
            tempDirection = CalculateFrightened(currentNodesNeighbour) - (Vector2)CurrentNode.transform.position;
        }
        else
        {
            tempDirection = CalculateChaseTarget(currentNodesNeighbour) - (Vector2)CurrentNode.transform.position;
        }

        return NormalizeDirection(tempDirection);
    }


    private Vector2 NormalizeDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x < 0 ? new Vector2(-1, 0) : new Vector2(1, 0);
        }
        else
        {
            return direction.y < 0 ? new Vector2(0, -1) : new Vector2(0, 1);
        }
    }


    public abstract Vector2 CalculateChaseTarget(List<NodeDetector> neighbors);
    
  
    
    private Vector2 CalculateScatterTarget(List<NodeDetector> neighbors, List<GameObject> scatterList, int scatterIndex)
    {
        float minDistSquaredToTarget = float.MaxValue;
        Vector2 closestNeighborPosition = Vector2.zero;
        GameObject targetPos = scatterList[scatterIndex];
        foreach (NodeDetector neighbor in neighbors)
        {
            if (LastVisited != null && neighbor == LastVisited) continue; 

            Vector2 neighborPosition = neighbor.transform.position;
            Vector2 distanceToTarget = (Vector2)targetPos.transform.position - neighborPosition;
            float distSquared = distanceToTarget.sqrMagnitude;

            if (distSquared < minDistSquaredToTarget)
            {
                minDistSquaredToTarget = distSquared;
                closestNeighborPosition = neighborPosition;
            }
        }

        return closestNeighborPosition;
    }

    private Vector2 CalculateFrightened(List<NodeDetector> neighbors)
    {
        if (neighbors.Count > 0)
        {
            int randomNodeIndex = Random.Range(0, neighbors.Count);
            return neighbors[randomNodeIndex].transform.position;
        }
        else
        {
            return Vector2.zero;
            
        }
       
    }

}
