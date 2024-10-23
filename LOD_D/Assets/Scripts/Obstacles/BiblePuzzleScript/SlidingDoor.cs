using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openDistance = 3f;      // ระยะที่ประตูจะเลื่อนขึ้น
    [SerializeField] private float moveSpeed = 5f;         // ความเร็วในการเลื่อน
    [SerializeField] private float closeDelay = 0.5f;      // ดีเลย์ก่อนปิด

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private float closeTimer = 0f;

    private void Start()
    {
        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.up * openDistance;
    }

    private void Update()
    {
        // เลื่อนประตูอย่างนุ่มนวล
        if (isOpen)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPosition,
                moveSpeed * Time.deltaTime
            );
        }
        else
        {
            if (closeTimer <= 0)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    closedPosition,
                    moveSpeed * Time.deltaTime
                );
            }
            else
            {
                closeTimer -= Time.deltaTime;
            }
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
    }

    public void CloseDoor()
    {
        isOpen = false;
        closeTimer = closeDelay;
    }
}
