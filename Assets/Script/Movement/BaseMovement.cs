using Events;
using Script;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BaseMovement : MonoBehaviour
{
    
    [SerializeField] float m_speed = 5.0f;
    protected Rigidbody2D m_rb;
    protected Vector2 direction;
    
    public bool IsStopped;
    public bool IsAtNode = true;
    

    protected virtual void Awake()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }
    
    
    protected virtual void Update()
    {
        MoveToDirection();
        MoveIfNeeded();
    }
    
    private void MoveToDirection()
    {
        if (IsAtNode && CanMoveInDirection(direction) )
        {
          ChangeDirection();
        }
    }

    protected void MoveIfNeeded()
    {
        if (!IsStopped&& IsAtNode)
        {
            Move();
            
        }
    }

    protected void ChangeDirection()
    {
        IsStopped = false;
    }


    private bool CanMoveInDirection(Vector2 dir)
    {
        int layerMask = 1 << LayerMask.NameToLayer("Character");
        layerMask = ~layerMask;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 3, layerMask);
        Debug.DrawRay(transform.position, dir * 2.5f, Color.red);

        if (hit.collider != null)
            if (hit.collider.TryGetComponent<BorderDetector>(out _))
            {
                return false;
            }

        return true;
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out NodeDetector node))
        {
            IsAtNode = true;
            
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out NodeDetector node))
        {
            IsAtNode = false;

        }
    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent<BorderDetector>(out _))
        {
            Debug.Log("stop");
            Stop();
        }
    }


    protected void Stop()
    {
        m_rb.velocity = Vector2.zero;
        IsStopped = true;
        IsAtNode = true;
    }


    protected void Move()
    {
        Vector2 moveVector = direction * m_speed;
        m_rb.velocity = moveVector;
    }
}