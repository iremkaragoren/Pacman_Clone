using Script;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateInkyDistance 
{
    
    public Vector2 CalculateTargetDistance(List<NodeDetector> neighbors, Vector2 blinkyPos)
    {
        float minDistSquared = float.MaxValue;
        Vector2 closestNeighborPosition = Vector2.zero;

        foreach (NodeDetector neighbor in neighbors)
        {
            Vector2 playerPos = PlayerMovement.Instance.PlayerPos() + (PlayerMovement.Instance.PlayerDir() * 2.0f);
            Vector2 blinkyDistance= blinkyPos - playerPos;
            Vector2 targetNode = blinkyDistance * 2;
            Debug.Log("target:" + targetNode);
            Vector2 targetDistance = targetNode - (Vector2)neighbor.transform.position;
            float distSquared = targetDistance.sqrMagnitude;

            if (distSquared < minDistSquared)
            {
                minDistSquared = distSquared;
                closestNeighborPosition = neighbor.transform.position;
                Debug.Log(neighbor.gameObject.name);
            }
        }

        return closestNeighborPosition;
    }


    public Vector2 CalculateScatterTarget(List<NodeDetector> neighbors, List<GameObject> scatterList,int scatterIndex)
    {
        float minDistSquared = float.MaxValue;
        Vector2 closestNeighborPosition = Vector2.zero;

        foreach (NodeDetector neighbor in neighbors)
        {
            GameObject targetPos = scatterList[scatterIndex];
            Vector2 targetDistance = (Vector2)targetPos.transform.position - (Vector2)neighbor.transform.position;
            float distSquared = targetDistance.sqrMagnitude;

            if (distSquared < minDistSquared)
            {
                minDistSquared = distSquared;
                closestNeighborPosition = neighbor.transform.position;
                Debug.Log(neighbor.gameObject.name);
            }
        }

        return closestNeighborPosition;
        
    }
}
