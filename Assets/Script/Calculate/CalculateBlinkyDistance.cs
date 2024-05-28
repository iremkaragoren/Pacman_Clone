using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script;
using UnityEngine;
using Random = UnityEngine.Random;

public class CalculateBlinkyDistance
{
    public Vector2 CalculateTargetDistance(List<NodeDetector> neighbors)
    {
        float minDistSquared = float.MaxValue;
        Vector2 closestNeighborPosition = Vector2.zero;
        
        foreach (NodeDetector neighbor in neighbors)
        {
            Vector2 targetDistance = PlayerMovement.Instance.PlayerPos() - (Vector2)neighbor.transform.position;
            float distSquared = targetDistance.sqrMagnitude;

            if (distSquared < minDistSquared)
            {
                minDistSquared = distSquared;
                closestNeighborPosition = neighbor.transform.position;
            }
        }

        return closestNeighborPosition;
    }
    
    
}