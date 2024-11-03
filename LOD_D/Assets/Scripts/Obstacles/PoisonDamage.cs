using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonDamage : MonoBehaviour
{
    public float damagePerSecond = 5f;
    private bool isTriggering = false;
    private PlayerHealth playerHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            isTriggering = true;
            playerHealth = other.GetComponent<PlayerHealth>();
            
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isTriggering = false;
        }
    }

    private void Update()
    {
        if (isTriggering)
        {
            playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
