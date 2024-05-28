using System.Collections.Generic;
using Events;
using UnityEngine;

namespace Script.Enemy
{
    public class GhostMovementAnim : MonoBehaviour
    {
        private SpriteRenderer enemyRenderer;
        
        [SerializeField] private GameObject enemyParentGO;
        private Animator m_animator;
        
        public bool timePaused;
        

        private void Awake()
        {
            Initialize();
        }
        private void Initialize()
        {
            enemyRenderer = GetComponent<SpriteRenderer>();
        }
        
        private void OnEnable()
        {
            ExternalEvents.TimePaused += OnTimePaused;
            InternalEvents.PlayerDeath += OnPlayerDeath;
        }

        private void OnPlayerDeath()
        {
            for (int i = 0; i < enemyParentGO.transform.childCount; i++)
            {
                enemyParentGO.transform.GetChild(i).gameObject.SetActive(false);
            }
        }


        public void OnFrighened()
        {
            for (int i = 0; i < enemyParentGO.transform.childCount; i++)
            {
                enemyParentGO.transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            ExternalEvents.TimePaused -= OnTimePaused;
            InternalEvents.PlayerDeath -= OnPlayerDeath;
        }

        private void OnTimePaused(bool paused)
        {
            timePaused = paused;
        }
        
        
        public void OnDirectionChanged(Vector2 direction)
        {
            if (direction == Vector2.up)
            {
                enemyRenderer = enemyParentGO.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            
                enemyRenderer.flipY = true;
                enemyRenderer.flipX = false;
                enemyParentGO.transform.GetChild(0).gameObject.SetActive(true);
                enemyParentGO.transform.GetChild(1).gameObject.SetActive(false);
            
            }
        
            if (direction == Vector2.down)
            {
                enemyRenderer = enemyParentGO.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
                enemyRenderer.flipY = false;
                enemyRenderer.flipX = false;
                enemyParentGO.transform.GetChild(0).gameObject.SetActive(true);
                enemyParentGO.transform.GetChild(1).gameObject.SetActive(false);
            
            }

            if (direction == Vector2.left)
            {
                enemyRenderer = enemyParentGO.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
                enemyRenderer.flipY = false;
                enemyRenderer.flipX = false;
                enemyParentGO.transform.GetChild(0).gameObject.SetActive(false);
                enemyParentGO.transform.GetChild(1).gameObject.SetActive(true);
            }
        
            if (direction == Vector2.right)
            {
                enemyRenderer = enemyParentGO.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
                enemyRenderer.flipY = false;
                enemyRenderer.flipX = true;
                enemyParentGO.transform.GetChild(0).gameObject.SetActive(false);
                enemyParentGO.transform.GetChild(1).gameObject.SetActive(true);
            }
        }

        
        
    }
}
