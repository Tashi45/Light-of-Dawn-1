using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float pressedYPosition = 0.1f;  // ตำแหน่ง Y เมื่อถูกกด
    [SerializeField] private float returnSpeed = 5f;         // ความเร็วในการคืนตำแหน่งเดิม
    [SerializeField] private Color activatedColor = Color.green;  // สีเมื่อถูกกด

    [Header("References")]
    [SerializeField] private SlidingDoor connectedDoor;      // ประตูที่จะควบคุม

    private Vector3 originalPosition;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;
    private int objectsOnPlate = 0;
    private bool isPressed = false;

    private void Start()
    {
        originalPosition = transform.position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // เช็คว่าเป็น Player หรือ Box หรือไม่
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            objectsOnPlate++;
            if (objectsOnPlate == 1)  // เพิ่งมีวัตถุชิ้นแรกเหยียบ
            {
                PressPlate();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // เช็คว่าเป็น Player หรือ Box หรือไม่
        if (other.CompareTag("Player") || other.CompareTag("Box"))
        {
            objectsOnPlate--;
            if (objectsOnPlate == 0)  // ไม่มีวัตถุเหยียบแล้ว
            {
                ReleasePlate();
            }
        }
    }

    private void PressPlate()
    {
        isPressed = true;
        // เลื่อนปุ่มลง
        transform.position = new Vector3(
            transform.position.x,
            originalPosition.y - pressedYPosition,
            transform.position.z
        );
        // เปลี่ยนสี
        spriteRenderer.color = activatedColor;
        // สั่งเปิดประตู
        if (connectedDoor != null)
        {
            connectedDoor.OpenDoor();
        }
    }

    private void ReleasePlate()
    {
        isPressed = false;
        // คืนตำแหน่งปุ่ม
        transform.position = originalPosition;
        // คืนสีเดิม
        spriteRenderer.color = originalColor;
        // สั่งปิดประตู
        if (connectedDoor != null)
        {
            connectedDoor.CloseDoor();
        }
    }
}
