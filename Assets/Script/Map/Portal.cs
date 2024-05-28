using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private bool isTeleporting = false;
    public float teleportCooldown = 0.5f; 
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.TryGetComponent<PortalDetector>(out PortalDetector portalDetector) && !isTeleporting)
        {
            StartCoroutine(TeleportPlayer(other.transform));
        }
        
        IEnumerator TeleportPlayer(Transform portalTransform)
        {
            isTeleporting = true;
            
            Transform destination = portalTransform.position == pointA.position ? pointB : pointA;
        
            yield return new WaitForSeconds(teleportCooldown); 
            transform.position = destination.position;

            yield return new WaitForSeconds(0.1f); 
            isTeleporting = false; 
        }
    }
}