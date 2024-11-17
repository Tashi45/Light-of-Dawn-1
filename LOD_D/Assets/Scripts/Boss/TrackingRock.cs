using UnityEngine;
using System;
using System.Collections.Generic;

public class TrackingRock : MonoBehaviour
{
    private Transform playerTransform;
    private Rigidbody2D rb;
    public float lifetime = 7f;
    public float damage = 15f;
    private bool hasHitPlayer = false;
    
    
    // ตัวแปรสำหรับระบบ checkpoint
    private Queue<Vector2> checkpoints = new Queue<Vector2>();
    private Vector2 currentTarget;
    private float recordInterval = 0.2f;
    private float lastRecordTime;
    private int maxCheckpoints = 5;
    private float checkpointReachDistance = 0.5f;
    
    // ตัวแปรสำหรับการเคลื่อนที่
    public float initialSpeed = 1.5f;
    public float maxSpeed = 5f;
    public float accelerationTime = 1.5f;
    private float currentAccelerationTime = 0f;
    private float currentSpeed;
    private Vector2 currentDirection;
    
    // ตัวแปรสำหรับการลอยหนีพื้น
    private float minHeightFromGround = 2f;
    private float upwardForce = 5f;
    private LayerMask groundLayer;
    
    public static event Action onRockDestroyed;

    public void Initialize(Transform player)
    {
        playerTransform = player;
        currentSpeed = initialSpeed;
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        
        // ตั้งค่า layer และ collision
        gameObject.layer = LayerMask.NameToLayer("TrackingRock");
        groundLayer = LayerMask.GetMask("Ground");
        lastRecordTime = -recordInterval;

        if (playerTransform != null)
        {
            currentDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
            rb.velocity = currentDirection * currentSpeed;
        }
    }

    void Update()
    {
        if (hasHitPlayer || playerTransform == null) return;

        if (lifetime > 0)
        {
            RecordPlayerPosition();
            MoveTowardsCheckpoint();
            CheckGroundDistance();
            lifetime -= Time.deltaTime;
        }
        else
        {
            DestroyRock();
        }
    }

    void RecordPlayerPosition()
    {
        if (Time.time >= lastRecordTime + recordInterval)
        {
            lastRecordTime = Time.time;
            Vector2 playerPos = playerTransform.position;

            if (checkpoints.Count == 0 || Vector2.Distance(playerPos, checkpoints.Peek()) > 1f)
            {
                checkpoints.Enqueue(playerPos);
                
                while (checkpoints.Count > maxCheckpoints)
                {
                    checkpoints.Dequeue();
                }
            }
        }
    }

    void MoveTowardsCheckpoint()
    {
        Vector2 moveDirection;

        if (checkpoints.Count == 0)
        {
            if (playerTransform != null)
            {
                moveDirection = ((Vector2)playerTransform.position - (Vector2)transform.position).normalized;
                UpdateMovement(moveDirection);
            }
            return;
        }

        if (currentTarget == Vector2.zero && checkpoints.Count > 0)
        {
            currentTarget = checkpoints.Peek();
        }

        moveDirection = (currentTarget - (Vector2)transform.position).normalized;
        UpdateMovement(moveDirection);

        if (Vector2.Distance(transform.position, currentTarget) < checkpointReachDistance)
        {
            checkpoints.Dequeue();
            currentTarget = checkpoints.Count > 0 ? checkpoints.Peek() : Vector2.zero;
        }
    }

    void UpdateMovement(Vector2 direction)
    {
        if (currentAccelerationTime < accelerationTime)
        {
            currentAccelerationTime += Time.deltaTime;
            currentSpeed = Mathf.Lerp(initialSpeed, maxSpeed, currentAccelerationTime / accelerationTime);
        }

        // ทำให้การเคลื่อนที่นุ่มนวลขึ้น
        currentDirection = Vector2.Lerp(currentDirection, direction, 0.1f).normalized;
        rb.velocity = currentDirection * currentSpeed;
    }

    void CheckGroundDistance()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, minHeightFromGround, groundLayer);
        
        if (hit.collider != null)
        {
            float distanceToGround = hit.distance;
            float upForce = upwardForce * (1 - (distanceToGround / minHeightFromGround));
            rb.AddForce(Vector2.up * upForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // ชนพื้น
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.AddForce(Vector2.up * upwardForce, ForceMode2D.Impulse);
            return;
        }

        // ทำลายหินเมื่อชนกับวัตถุใดๆ ยกเว้นหินด้วยกัน
        if (!collision.gameObject.CompareTag("TrackingRock"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damage);
                    
                }
            }
            DestroyRock();
        }
    }

    private void DestroyRock()
    {
        onRockDestroyed?.Invoke();
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        onRockDestroyed?.Invoke();
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            // วาด checkpoint path
            Vector3 previousPos = transform.position;
            foreach (Vector2 checkpoint in checkpoints)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(previousPos, checkpoint);
                Gizmos.DrawWireSphere(checkpoint, 0.2f);
                previousPos = checkpoint;
            }

            // วาดเส้นไปยังเป้าหมายปัจจุบัน
            if (currentTarget != Vector2.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, currentTarget);
            }

            // วาดทิศทางการเคลื่อนที่
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, currentDirection * 2f);
        }
    }
}