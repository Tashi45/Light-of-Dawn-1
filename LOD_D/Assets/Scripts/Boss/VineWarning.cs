using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineWarning : MonoBehaviour
{
    public float warningDuration = 1.5f;
    public Color warningColor = new Color(1f, 0f, 0f, 0.5f);
    public float pulseSpeed = 2f;
    
    // เพิ่มตัวแปรสำหรับควบคุมการกระพริบ
    public float blinkSpeed = 8f; // ความเร็วในการกระพริบ
    public float minAlpha = 0.2f; // ความโปร่งใสต่ำสุด
    public float maxAlpha = 0.8f; // ความโปร่งใสสูงสุด

    private SpriteRenderer spriteRenderer;
    private float startTime;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = warningColor;
        startTime = Time.time;
        Destroy(gameObject, warningDuration);
    }

    void Update()
    {
        // ปรับ scale
        float scaleX = 1f + (Mathf.Sin((Time.time - startTime) * pulseSpeed) * 0.1f);
        transform.localScale = new Vector3(1f, 4.3f, 1f);

        // เพิ่ม effect กระพริบ
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, 
            (Mathf.Sin((Time.time - startTime) * blinkSpeed) + 1f) / 2f);
        
        Color newColor = warningColor;
        newColor.a = alpha;
        spriteRenderer.color = newColor;
    }
}


