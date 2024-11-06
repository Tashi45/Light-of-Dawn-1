using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float topPosition = 5f;
    [SerializeField] private float bottomPosition = -5f;
    
    [Header("Optional Delay Settings")]
    [SerializeField] private float delayAtTop = 1f;
    [SerializeField] private float delayAtBottom = 1f;

    [Header("Damage Settings")]
    [SerializeField] private float damage = 100f; // ย้ายเป็น SerializeField เพื่อปรับค่าได้ใน Inspector
    
    private PlayerHealth playerHealth;
    private Vector3 startPosition;
    private bool movingDown = true;
    private float delayTimer = 0f;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (delayTimer > 0)
        {
            delayTimer -= Time.deltaTime;
            return;
        }

        if (movingDown)
        {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
            
            if (transform.position.y <= startPosition.y + bottomPosition)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y + bottomPosition, transform.position.z);
                movingDown = false;
                delayTimer = delayAtBottom;
            }
        }
        else
        {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            
            if (transform.position.y >= startPosition.y + topPosition)
            {
                transform.position = new Vector3(transform.position.x, startPosition.y + topPosition, transform.position.z);
                movingDown = true;
                delayTimer = delayAtTop;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // หา PlayerHealth component จาก Player ที่ชน
            playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                Debug.Log($"Player hit by spike trap! Dealing {damage} damage");
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("PlayerHealth component not found on player!");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // แสดง visual ในหน้า Editor เพื่อเห็นระยะการเคลื่อนที่
        Gizmos.color = Color.red;
        if (Application.isPlaying)
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, startPosition.y + topPosition, transform.position.z), 0.3f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, startPosition.y + bottomPosition, transform.position.z), 0.3f);
        }
        else
        {
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + topPosition, transform.position.z), 0.3f);
            Gizmos.DrawWireSphere(new Vector3(transform.position.x, transform.position.y + bottomPosition, transform.position.z), 0.3f);
        }
    }
}