using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;

public class PlayerMovementAnim : MonoBehaviour
{
    private SpriteRenderer playerSpriteRenderer;
    private SpriteRenderer starterSpriteRenderer;
    [SerializeField] private GameData_SO gameData;
    private void Awake()
    {
        Initialize();
    }
    private void Initialize()
    {
        playerSpriteRenderer = gameData.Pacman.GetComponent<SpriteRenderer>();
        starterSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        ExternalEvents.LevelStart += OnLevelStart;
        InternalEvents.PlayerDirectionChanged += OnPlayerDirChanged;
    }
    
    private void OnDisable()
    {
        ExternalEvents.LevelStart -= OnLevelStart;
        InternalEvents.PlayerDirectionChanged -= OnPlayerDirChanged;
    }

    private void OnLevelStart()
    {
        starterSpriteRenderer.sprite = playerSpriteRenderer.sprite;
    }

    private void OnPlayerDirChanged(Vector2 direction)
    {
        if (direction == Vector2.left)
        {
            transform.eulerAngles = new Vector3(0, 0,-180);
        }
        
        else if (direction == Vector2.right)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        
        else  if (direction == Vector2.down)
        {
            transform.eulerAngles = new Vector3(0, 0, -90);
        }
        
        else if (direction == Vector2.up)
        {
            transform.eulerAngles = new Vector3(0, 0, 90);
        }
        
    }

    
}
