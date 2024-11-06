using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float maxHealth;
    public BossChapterOne bDamage;
    public bool isInvulnerable = false;
    public float invulnerabilityDuration;
    public float knockbackForce = 15f;
    public Image healthBar;
    public Animator animator;
    public Rigidbody2D rb;
    public string lastScene;
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
                isInvulnerable = false;
                animator.SetBool("IsHurt",false);
                
            }
        }
        if (health <= 0)
        {
            Die();
        }
    }
    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     if (other.gameObject.CompareTag("Vine") || other.gameObject.CompareTag("Spike"))
    //     {
    //         if (GetComponent<Rigidbody2D>() != null)
    //         {
    //             if (!isInvulnerable && other.gameObject.CompareTag("Vine"))
    //             {
    //                 Debug.Log("-20 HP");
    //                 health -= bDamage.damage;
    //                 animator.SetBool("IsHurt",true);
    //             }
    //
    //             else if (!isInvulnerable && other.gameObject.CompareTag("Spike"))
    //             {
    //                 health -= 10;
    //                 animator.SetBool("IsHurt",true);
    //             }
    //             
    //             
    //             Vector2 knockbackDirection = transform.position - other.transform.position;
    //             knockbackDirection.Normalize();
    //             knockbackDirection.y += Mathf.Clamp(knockbackDirection.y, 0f, 0f);
    //             GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    //             isInvulnerable = true;
    //             invulnerabilityDuration = 0.5f;
    //         }
    //     }
    //     if (other.gameObject.CompareTag("Rock"))
    //     {
    //         ContactPoint2D contact = other.GetContact(0);
    //         if (contact.normal.y < -0.5f && contact.relativeVelocity.y < -5f)
    //         {
    //                 if (!isInvulnerable)
    //                 {
    //                     Debug.Log("-5 HP");
    //                     health -= 5; 
    //                     isInvulnerable = true;
    //                     invulnerabilityDuration = 0.5f; 
    //                     Vector2 knockbackDirection = transform.position - other.transform.position;
    //                     knockbackDirection.Normalize();
    //                     knockbackDirection.y += Mathf.Clamp(knockbackDirection.y, 0f, 0f);
    //                     GetComponent<Rigidbody2D>().AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
    //                 } 
    //         }
    //     }
    //
    // }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isInvulnerable) return;
         switch (other.gameObject.tag)
            {
                case "Vine":
                case "Spike":
                    ApplyDamageAndKnockback(other, 10);
                    Debug.Log("TakeTake");
                    break;
                case "Rock":
                    ContactPoint2D contact = other.GetContact(0);
                    if (contact.normal.y < -0.5f && contact.relativeVelocity.y < -5f)
                    {
                        ApplyDamageAndKnockback(other, 5);
                    }
                    break;
            }
         
    }
    
    private void ApplyDamageAndKnockback(Collision2D other, int damage)
    {
        if (!isInvulnerable)
        {
            health -= damage;
            animator.SetBool("IsHurt", true);
            AudioManager.Instance.PlaySFX("Hurt");
            isInvulnerable = true;
            invulnerabilityDuration =0.5f;

            Vector2 knockbackDirection = transform.position - other.transform.position;
            knockbackDirection.Normalize();
            knockbackDirection.y += Mathf.Clamp(knockbackDirection.y, 0f, 0f);
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }

    }
    
    
    public void TakeDamage(float amount)
    {
        health -= amount;
    }
    
    private void Die()
    {
        rb.isKinematic = true;
        rb.velocity = Vector2.zero;
        animator.SetBool("IsDead",true);
        AudioManager.Instance.PlaySFX("Die");
        
        StartCoroutine(LoadLastSceneAfterDelay());
    }
    IEnumerator LoadLastSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(lastScene);
    }
}