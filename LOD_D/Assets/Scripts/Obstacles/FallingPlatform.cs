using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FallingPlatform : MonoBehaviour
{
    [SerializeField] private float fallDelay = 1f;
    private float destroyDelay = 1f;
    [SerializeField] private float respawnDelay = 5f;
    [SerializeField] private float YPos = 1.5f;
    [SerializeField] private bool respawn;
    private bool hasRespawned;
    
    [SerializeField] private Rigidbody2D rb;

    public void Start()
    {
        EnableMovingPlatScript();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
            {
                StartCoroutine(Fall());
            }

    }

    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(fallDelay);
        rb.bodyType = RigidbodyType2D.Dynamic;
        
        if (respawn == true)
        {
            yield return new WaitForSeconds(respawnDelay);
            Respawn();
        }
        else
        {
           
        }
        Destroy(gameObject,destroyDelay);
        
    }
    
    public void EnableMovingPlatScript()
    {
        MovingPlatform movingPlat = GetComponent<MovingPlatform>();
        movingPlat.enabled = true;
    }
    
    private void Respawn()
    {
        if (hasRespawned) return;

        hasRespawned = true;
            Vector3 respawnPosition = transform.position;
            respawnPosition.y = YPos; 
        
            GameObject newObject = Instantiate(gameObject, respawnPosition, transform.rotation);
            newObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;

    }
}
