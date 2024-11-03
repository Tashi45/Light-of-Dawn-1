using UnityEngine;

public class ReversibleStructure : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Vector3 brokenPosition;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private float reverseSpeed = 2f;
    [SerializeField] private Animator animator; // ถ้ามี animation
    
    public bool canBeReversed = true;
    private bool isBroken = false;
    private bool isReversing = false;

    private void Start()
    {
        originalPosition = transform.position;
        // สามารถกำหนด brokenPosition ใน editor หรือคำนวณจาก originalPosition
        if (brokenPosition == Vector3.zero)
        {
            brokenPosition = originalPosition + Vector3.down * 5f;
        }
    }

    private void Update()
    {
        if (isReversing)
        {
            UpdateReverseAnimation();
        }
    }

    public void Break()
    {
        if (!isBroken)
        {
            isBroken = true;
            transform.position = brokenPosition;
            
            if (animator != null)
            {
                animator.SetTrigger("Break");
            }
        }
    }

    public void Reverse()
    {
        if (isBroken && canBeReversed && !isReversing)
        {
            isReversing = true;
            
            if (animator != null)
            {
                animator.SetTrigger("Reverse");
            }
        }
    }

    private void UpdateReverseAnimation()
    {
        // Lerp ไปยังตำแหน่งเดิม
        transform.position = Vector3.Lerp(
            transform.position,
            originalPosition,
            reverseSpeed * Time.deltaTime
        );

        // เช็คว่าถึงตำแหน่งเดิมแล้วหรือยัง
        if (Vector3.Distance(transform.position, originalPosition) < 0.01f)
        {
            transform.position = originalPosition;
            isReversing = false;
            isBroken = false;
        }
    }
}