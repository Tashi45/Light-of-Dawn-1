using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float openDistance = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float closeDelay = 0.5f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen = false;
    private float closeTimer = 0f;
    public bool IsUp;
    
    private BoxCollider2D doorCollider;

    private void Start()
    {
        closedPosition = transform.position;
        if (IsUp == true)
        {
            openPosition = closedPosition + Vector3.up * openDistance;
        }
        else
        {
            openPosition = closedPosition + Vector3.left * openDistance;
        }
        
        doorCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                openPosition,
                moveSpeed * Time.deltaTime
            );
            // ปิด Collider ทันทีที่ประตูเริ่มเปิด
            doorCollider.enabled = false;
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
                
                // เปิด Collider กลับเมื่อประตูเริ่มปิด
                doorCollider.enabled = true;
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
        AudioManager.Instance.PlaySFX("PressureDoor");
    }

    public void CloseDoor()
    {
        isOpen = false;
        closeTimer = closeDelay;
    }
}