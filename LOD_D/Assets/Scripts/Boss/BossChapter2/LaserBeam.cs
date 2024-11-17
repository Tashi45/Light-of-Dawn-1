using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float damage = 15f;
    [SerializeField] private float knockbackForce = 8f;
    
    [Header("Laser Properties")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private float lifetime = 3f;

    [Header("Speed Boost Settings")]
    [SerializeField] private float playerSpeedBoostMultiplier = 1.2f;
    [SerializeField] private float speedBoostDuration = 1f;
    [SerializeField] private float defaultPlayerSpeed = 6f;
    
    private Rigidbody2D rb;
    private bool hasHitPlayer = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.right * speed;
        AudioManager.Instance.PlaySFX("LaserShot");
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            
            PlayerHealth health = other.GetComponent<PlayerHealth>();
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            
            if (health != null && playerRb != null)
            {
                // ค้นหา BossChase component
                BossChase boss = FindObjectOfType<BossChase>();
                if (boss != null)
                {
                    // คำนวณทิศทาง knockback
                    Vector2 knockbackDirection = (other.transform.position - transform.position).normalized;
                    knockbackDirection.y = Mathf.Clamp(knockbackDirection.y, 0f, 0.5f);
                
                    // ใช้ระบบ TakeDamage
                    health.TakeDamage(damage);
                
                    // เพิ่ม knockback force
                    playerRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

                    // ใช้ระบบ speed boost จาก BossChase
                    boss.BoostPlayerSpeedFromLaser();
                }
            }
            
            Destroy(gameObject);
        }
    }
}