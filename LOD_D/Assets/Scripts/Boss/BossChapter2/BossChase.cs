using UnityEngine;
using System.Collections;

public class BossChase : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float minDistanceToPlayer = 2f;

    [Header("Combat")]
    [SerializeField] private float damage = 25f;
    [SerializeField] private float attackCooldown = 0.5f;
    [SerializeField] private float playerSpeedBoostMultiplier = 1.2f;
    [SerializeField] private float speedBoostDuration = 1f;
    [SerializeField] private float defaultPlayerSpeed = 6f;
    private float lastDamageTime;
    [SerializeField] private float damageCooldown = 3f;
    
    [Header("Laser Attack")]
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private float laserCooldown = 5f;
    [SerializeField] private Transform laserSpawnPoint;
    [SerializeField] private float minLaserDistance = 3f;
    [SerializeField] private float laserWarningDuration = 0.5f;
    private float lastLaserTime;
    private bool isPreparingLaser = false;

    private Transform player;
    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private float lastAttackTime;
    private PlayerMovement playerMovement;
    private Coroutine speedBoostCoroutine;

    private void Start()
    {
        AudioManager.Instance.PlaySFX("LargeBoss");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = playerObj.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.Data.runMaxSpeed = defaultPlayerSpeed;
            }
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= detectionRange && distance > minDistanceToPlayer)
        {
            MoveTowardsPlayer();
            
            if (!isPreparingLaser && Time.time >= lastLaserTime + laserCooldown && distance >= minLaserDistance)
            {
                StartCoroutine(PrepareLaserAttack());
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = ((Vector2)player.position - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;
        
        if (direction.x > 0 && !isFacingRight || direction.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private IEnumerator PrepareLaserAttack()
    {
        isPreparingLaser = true;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            AudioManager.Instance.PlaySFX("LargeBoss");
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = Color.red;
            
            yield return new WaitForSeconds(laserWarningDuration);
            
            spriteRenderer.color = originalColor;
        }
        else
        {
            yield return new WaitForSeconds(laserWarningDuration);
        }

        ShootLaser();
        lastLaserTime = Time.time;
        isPreparingLaser = false;
    }
    
    private void ShootLaser()
    {
        if (laserPrefab != null && laserSpawnPoint != null)
        {
            Instantiate(laserPrefab, laserSpawnPoint.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX("Outgoing");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null && Time.time >= lastDamageTime + damageCooldown)
        {
            health.TakeDamage(damage);
            lastDamageTime = Time.time;
        }

        if (playerMovement != null)
        {
            if (speedBoostCoroutine != null)
            {
                StopCoroutine(speedBoostCoroutine);
            }
            speedBoostCoroutine = StartCoroutine(BoostPlayerSpeed());
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null && Time.time >= lastDamageTime + damageCooldown)
        {
            health.TakeDamage(damage);
            lastDamageTime = Time.time;
        }
    }

    public void BoostPlayerSpeedFromLaser()
    {
        if (playerMovement != null)
        {
            if (speedBoostCoroutine != null)
            {
                StopCoroutine(speedBoostCoroutine);
            }
            speedBoostCoroutine = StartCoroutine(BoostPlayerSpeed());
        }
    }

    private IEnumerator BoostPlayerSpeed()
    {
        playerMovement.Data.runMaxSpeed = defaultPlayerSpeed * playerSpeedBoostMultiplier;

        yield return new WaitForSeconds(speedBoostDuration);

        if (playerMovement != null)
        {
            playerMovement.Data.runMaxSpeed = defaultPlayerSpeed;
        }
        speedBoostCoroutine = null;
    }

    private void OnDisable()
    {
        ResetPlayerSpeed();
    }

    private void OnDestroy()
    {
        ResetPlayerSpeed();
    }

    private void ResetPlayerSpeed()
    {
        if (playerMovement != null)
        {
            if (speedBoostCoroutine != null)
            {
                StopCoroutine(speedBoostCoroutine);
                speedBoostCoroutine = null;
            }
            playerMovement.Data.runMaxSpeed = defaultPlayerSpeed;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minDistanceToPlayer);
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, minLaserDistance);
    }
}