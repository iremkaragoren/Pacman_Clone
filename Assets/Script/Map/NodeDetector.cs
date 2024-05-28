using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Script
{
    public class NodeDetector:MonoBehaviour
    {
    
        public List<NodeDetector> Neighbors;
        
        public void InitializeNeighbors(float checkNodesDistance)
        {
            Vector2[] directions={ Vector2.down, Vector2.left, Vector2.right, Vector2.up };

            foreach (Vector2 direction in directions)
            {
                int characterLayerMask  = 1 << LayerMask.NameToLayer("Character");
                int doorLayerMask = 1 << LayerMask.NameToLayer("Door");
                int combinedLayerMask = characterLayerMask | doorLayerMask;
                int layerMask = ~combinedLayerMask;
                Vector2 rayOrigin = (Vector2)transform.position + direction.normalized * 1.0f;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, direction, checkNodesDistance,layerMask);
                Debug.DrawRay(rayOrigin, direction * 2.5f, Color.green);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent<NodeDetector>(out NodeDetector node))
                    {
                        if ( !Neighbors.Contains(node))
                            Neighbors.Add(node);
                    } 
                }
            }
        }
    }
}