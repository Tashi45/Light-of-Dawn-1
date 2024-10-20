using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public BossChapterOne bDamage;
    private bool isInvulnerable = false;
    public float invulnerabilityDuration;
    public float knockbackForce = 5f;
    public Image healthBar;
    public Animator animator;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()

    {
        maxHealth = health;
    }
    
    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = Mathf.Clamp(health / maxHealth, 0, 1);
        
        if (isInvulnerable)
        {
            invulnerabilityDuration -= Time.deltaTime;
            if (invulnerabilityDuration <= 0)
            {
                animator.SetBool("IsHurt",false);
                isInvulnerable = false;
            }
        }

        if (health <= 0)
        {
           
            Die();
            //Destroy(gameObject);
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Vine"))
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                if (!isInvulnerable)
                {
                    Debug.Log("-20 HP");
                    health -= bDamage.damange;
                    animator.SetBool("IsHurt",true);
                }
                Vector2 knockbackDirection = transform.position - other.transform.position;
                knockbackDirection.Normalize();
                knockbackDirection.y += Mathf.Clamp(knockbackDirection.y, 0f, 0f);
                GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
                isInvulnerable = true;
                invulnerabilityDuration = 0.5f;
                
            }
        }
    }

    private void Die()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;
        animator.SetBool("IsDead",true);
    }
}