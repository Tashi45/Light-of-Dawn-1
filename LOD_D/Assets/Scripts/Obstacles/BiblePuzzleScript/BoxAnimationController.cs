using System;
using UnityEngine;

public class BoxAnimationController : MonoBehaviour
{
    private Animator animator;
    public string openTriggerName = "Open"; // ชื่อ trigger parameter ใน Animator
    
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // เรียกใช้ฟังก์ชันนี้เพื่อเล่น animation เปิดกล่อง
    public void PlayOpenAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(openTriggerName);
        }
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}