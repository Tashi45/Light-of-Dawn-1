using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (other.gameObject.CompareTag("Vine") || other.gameObject.CompareTag("Spike"))
        {
            if (GetComponent<Rigidbody2D>() != null)
            {
                if (!isInvulnerable && other.gameObject.CompareTag("Vine"))
                {
                    Debug.Log("-20 HP");
                    health -= bDamage.damange;
                    animator.SetBool("IsHurt",true);
                }

                else if (!isInvulnerable && other.gameObject.CompareTag("Spike"))
                {
                    health -= 10;
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Spike"))
        {
            health -= 10;
            animator.SetBool("IsHurt",true);
        }
        isInvulnerable = true;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
    }
    


    private void Die()
    {
        rb.isKinematic = true;
        animator.SetBool("IsDead",true);

        
        StartCoroutine(LoadLastSceneAfterDelay());
        
        
    }

    IEnumerator LoadLastSceneAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(lastScene);
    }
}