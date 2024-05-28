using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script;
using Unity.VisualScripting;
using UnityEngine;

public class PinkyMovement : GhostMovement
{
    public override Vector2 CalculateChaseTarget(List<NodeDetector> neighbors)
    {
        Vector2 playerPos = PlayerMovement.Instance.PlayerPos() + (PlayerMovement.Instance.PlayerDir() * 4.0f);
        
        List<NodeDetector> closestNeighbors = new List<NodeDetector>();
        float minDistSquared = float.MaxValue;
        
        foreach (NodeDetector neighbor in neighbors)
        {
            Vector2 targetDistance = playerPos - (Vector2)neighbor.transform.position;
            float distSquared = targetDistance.sqrMagnitude;

            if (distSquared < minDistSquared)
            {
                closestNeighbors.Clear();
                closestNeighbors.Add(neighbor);
                minDistSquared = distSquared;
            }
            else if (distSquared == minDistSquared)
            {
                closestNeighbors.Add(neighbor);
            }
        }

       
        NodeDetector priorityNode = SelectPriorityNode(closestNeighbors, playerPos);
        return priorityNode != null ? priorityNode.transform.position : Vector2.zero;
    }

    private NodeDetector SelectPriorityNode(List<NodeDetector> closestNeighbors, Vector2 target)
    {
        if (closestNeighbors.Count == 1)
        {
            return closestNeighbors[0];
        }

        NodeDetector priorityNode = null;
        float minY = float.MaxValue, minX = float.MaxValue, maxY = float.MinValue;
        foreach (var neighbor in closestNeighbors)
        {
            Vector2 direction = (Vector2)neighbor.transform.position - target;
            if (direction.y < minY)
            {
                priorityNode = neighbor;
                minY = direction.y;
                minX = Mathf.Abs(direction.x);
            }
            else if (Mathf.Approximately(direction.y, minY) && Mathf.Abs(direction.x) < minX)
            {
                priorityNode = neighbor;
                minX = Mathf.Abs(direction.x);
            }
            else if (direction.y > maxY && priorityNode == null)
            {
                priorityNode = neighbor;
                maxY = direction.y;
            }
        }

        return priorityNode;
    }
}


