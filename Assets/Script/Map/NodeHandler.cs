using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Script;
using UnityEngine;

public class NodeHandler : MonoBehaviour
{
    private List<NodeDetector> allNodes;
    [SerializeField] private float checkNodesDistance = 6.0f;
    

    private void Awake()
    {
        allNodes = GetComponentsInChildren<NodeDetector>().ToList();
        allNodes.ForEach(nodeDetector => nodeDetector.InitializeNeighbors(checkNodesDistance));
    }
}