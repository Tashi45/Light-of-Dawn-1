using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisfloor : MonoBehaviour
{
    public Transform targetWaypoint;
    [SerializeField] private bool isPlayer;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayer == true)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                other.transform.position = targetWaypoint.position;
                PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(10);
                }
            }

        }
        
        if (other.gameObject.CompareTag("Box"))
        {
            other.transform.position = targetWaypoint.position;
            //AudioManager.Instance.PlaySFX("Warp");
        }
        
        
    }
}
