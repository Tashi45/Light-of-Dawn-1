using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
     PlayerHealth _playerHealth;
     public Transform respawnPoint;

     private SpriteRenderer _spriteRenderer;
     public Sprite passive, active;
     private Collider2D coll;

    private void Awake()
    {
        GetComponent<PlayerHealth>();
        _playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playerHealth.UpdateCheckpoint(respawnPoint.position);
            _spriteRenderer.sprite = active;
            AudioManager.Instance.PlaySFX("Checkpoint");
            coll.enabled = false;
        }
    }
}
