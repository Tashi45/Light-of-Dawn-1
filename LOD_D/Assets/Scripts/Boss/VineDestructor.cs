using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineDestructor : MonoBehaviour
{
    public float lifetime = 5f;
    private float timer = 0f;
    public float warningDuration = 2f;
    public float blinkInterval = 0.2f;
    
    private SpriteRenderer spriteRenderer;
    private bool isWarning = false;
    
    void Start()
    {
        timer = 0f;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        // Start warning when approaching destruction time
        if (timer >= lifetime - warningDuration && !isWarning)
        {
            isWarning = true;
            StartCoroutine(BlinkWarning());
        }
        
        // Destroy after lifetime
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkWarning()
    {
        if (spriteRenderer != null)
        {
            // Blink until destruction
            while (timer < lifetime)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
                yield return new WaitForSeconds(blinkInterval);
            }
            
            // Ensure sprite is visible before destruction
            spriteRenderer.enabled = true;
        }
    }
}