using System.Collections;
using System.Collections.Generic;
using Script;
using UnityEngine;

// public class CalculatePinkyDistance : MonoBehaviour
// {
//     public Vector2 CalculateTargetDistance(List<NodeDetector> Neighbors)
//     {
//         float minDistSquared = float.MaxValue;
//         Vector2 closestNeighborPosition = Vector2.zero;
//
//         foreach (NodeDetector neighbor in Neighbors)
//         {
//             Vector2 playerPos = PlayerMovement.Instance.PlayerPos() + (PlayerMovement.Instance.PlayerDir() * 4.0f);
//             Vector2 targetDistance = playerPos - (Vector2)neighbor.transform.position;
//             float distSquared = targetDistance.sqrMagnitude;
//
//             if (distSquared < minDistSquared)
//             {
//                 minDistSquared = distSquared;
//                 closestNeighborPosition = neighbor.transform.position;
//                 Debug.Log(neighbor.gameObject.name);
//             }
//         }
//
//         return closestNeighborPosition;
//     }
// }
