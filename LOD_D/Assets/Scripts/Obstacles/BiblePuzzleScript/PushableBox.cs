using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableBox : MonoBehaviour
{
    [Header("Box Settings")]
    [SerializeField] private float pushForce = 2f;
    [SerializeField] private float maxPushSpeed = 5f;
    [SerializeField] private float groundDrag = 2f;
    [SerializeField] private LayerMask playerLayer;  // เพิ่ม Layer สำหรับ Player

    private Rigidbody2D rb;
    private bool isBeingPushed = false;
    private GameObject player;
    private Collider2D boxCollider;
    private PhysicsMaterial2D boxPhysicsMaterial;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<Collider2D>();
        
        // สร้าง Physics Material 2D ใหม่
        boxPhysicsMaterial = new PhysicsMaterial2D();
        boxPhysicsMaterial.friction = 0.6f;        // ค่าความเสียดทาน
        boxPhysicsMaterial.bounciness = 0f;        // ไม่ต้องการให้กระดอน
        
        // กำหนดค่าเริ่มต้น
        boxCollider.sharedMaterial = boxPhysicsMaterial;
        rb.drag = groundDrag;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;  // ป้องกันการทะลุ
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // เช็คว่าการชนเกิดจากด้านข้างหรือไม่
            foreach (ContactPoint2D contact in collision.contacts)
            {
                // ถ้าเป็นการชนด้านข้าง (normal.y ใกล้ 0)
                if (Mathf.Abs(contact.normal.y) < 0.5f)
                {
                    Vector2 pushDirection = transform.position - collision.transform.position;
                    pushDirection.y = 0;
                    pushDirection.Normalize();

                    float playerInput = Input.GetAxisRaw("Horizontal");
                    
                    // ผลักเมื่อผู้เล่นกดปุ่มในทิศทางที่ถูกต้อง
                    if ((pushDirection.x > 0 && playerInput > 0) || 
                        (pushDirection.x < 0 && playerInput < 0))
                    {
                        isBeingPushed = true;
                        PushBox(pushDirection);
                    }
                    return;
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // เช็คว่าผู้เล่นอยู่ด้านบนกล่องหรือไม่
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y < -0.5f)  // ผู้เล่นอยู่ด้านบน
                {
                    // ปรับค่าความเสียดทานให้สูงขึ้นเมื่อผู้เล่นยืนบนกล่อง
                    boxPhysicsMaterial.friction = 1.0f;
                    boxCollider.sharedMaterial = boxPhysicsMaterial;
                    return;
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBeingPushed = false;
            // คืนค่าความเสียดทานกลับเป็นค่าปกติ
            boxPhysicsMaterial.friction = 0.6f;
            boxCollider.sharedMaterial = boxPhysicsMaterial;
        }
    }

    void PushBox(Vector2 direction)
    {
        if (rb.velocity.magnitude < maxPushSpeed)
        {
            rb.AddForce(direction * pushForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // จำกัดความเร็วสูงสุด
        if (rb.velocity.magnitude > maxPushSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxPushSpeed;
        }

        // ชะลอความเร็วเมื่อไม่ได้ถูกผลัก
        if (!isBeingPushed && rb.velocity.magnitude > 0.1f)
        {
            rb.velocity *= 0.95f;
        }
    }
}
