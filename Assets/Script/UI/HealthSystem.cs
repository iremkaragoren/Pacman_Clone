// using System;
// using Events;
// using UnityEngine;
// using UnityEngine.SceneManagement;
//
// namespace Script.UI
// {
//     public class HealthSystem:MonoBehaviour
//     {
//         [SerializeField] private GameData_SO gameData;
//         [SerializeField] private GameObject[] health;
//         private int currenHealth;
//
//         private void Awake()
//         {
//             currenHealth = health.Length;
//         }
//
//         private void OnEnable()
//         {
//             InternalEvents.PlayerDeathAnimationComplete += LoseLife;
//         }
//
//         private void LoseLife()
//         {
//             if (currenHealth < gameData.PacManHealth)
//             {
//                 currenHealth--;
//                 health[currenHealth].SetActive(false);
//             }
//
//             if (currenHealth <= 0)
//             {
//                 string sceneName = SceneManager.GetActiveScene().name;
//                 SceneManager.LoadScene(sceneName);
//             }
//             
//         }
//
//         private void OnDisable()
//         {
//             InternalEvents.PlayerDeathAnimationComplete -= LoseLife;
//         }
//     }
// }