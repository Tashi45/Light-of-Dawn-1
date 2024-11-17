using UnityEngine;
using Cinemachine;

public class BossRoomCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera bossRoomCamera;
    [SerializeField] private BossChapterOne bossScript;

    [Header("Screen Shake Settings")]
    [SerializeField] private float shakeIntensity = 2f;
    [SerializeField] private float shakeTime = 3f;
    
    private CinemachineBasicMultiChannelPerlin noise;
    private bool wasInPhase2; // เปลี่ยนจาก hasEnteredPhase2 เป็น wasInPhase2

    private void Start()
    {
        // ตั้งค่าเริ่มต้นให้กล้องติดตามผู้เล่น
        if (playerCamera != null)
            playerCamera.Priority = 10;
        if (bossRoomCamera != null)
        {
            bossRoomCamera.Priority = 0;
            noise = bossRoomCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            if (noise != null)
            {
                noise.m_AmplitudeGain = 0f;
            }
        }
        wasInPhase2 = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SwitchToBossRoomCamera();
        }
    }

    private void Update()
    {
        if (bossScript != null)
        {
            bool isCurrentlyPhase2 = bossScript.bossHealth <= 50f;

            // ตรวจสอบการเปลี่ยนแปลง Phase
            if (isCurrentlyPhase2 && !wasInPhase2)
            {
                // เข้าสู่ Phase 2
                ShakeCamera();
                wasInPhase2 = true;
            }
            else if (!isCurrentlyPhase2 && wasInPhase2)
            {
                // กลับสู่ Phase 1
                wasInPhase2 = false;
            }

            // ตรวจสอบ boss ตาย
            if (bossScript.bossHealth <= 0)
            {
                SwitchToPlayerCamera();
            }
        }
    }

    private void ShakeCamera()
    {
        if (noise != null)
        {
            StopAllCoroutines(); // หยุด coroutine เดิมที่อาจกำลังทำงานอยู่
            noise.m_AmplitudeGain = shakeIntensity;
            StartCoroutine(StopShaking());
        }
    }

    private System.Collections.IEnumerator StopShaking()
    {
        yield return new WaitForSeconds(shakeTime);
        if (noise != null)
        {
            noise.m_AmplitudeGain = 0f;
        }
    }

    private void SwitchToBossRoomCamera()
    {
        if (bossRoomCamera != null)
            bossRoomCamera.Priority = 15;
        if (playerCamera != null)
            playerCamera.Priority = 0;
    }

    private void SwitchToPlayerCamera()
    {
        if (playerCamera != null)
            playerCamera.Priority = 10;
        if (bossRoomCamera != null)
            bossRoomCamera.Priority = 0;
    }
}