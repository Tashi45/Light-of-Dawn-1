using UnityEngine;
using System.Collections;

public class TreasureSpawnController : MonoBehaviour 
{
    [Header("Item Settings")]
    public GameObject treasureItem;
    public Transform spawnPoint;
    public float popUpHeight = 1f;
    public float popUpDuration = 0.5f;
    
    [Header("Gizmos Settings")]
    public Color gizmosColor = Color.yellow;
    public float sphereRadius = 0.1f;

    [Header("Effects")]
    public GameObject sparkleEffect;
    public AudioClip itemAppearSound;

    private AudioSource audioSource;
    private bool hasSpawned = false;
    private Vector3 originalItemPosition; // เก็บตำแหน่งเริ่มต้นของ item

    #region GIZMOS
    private void OnDrawGizmos()
    {
        if (spawnPoint != null)
        {
            Gizmos.color = gizmosColor;

            // จุดเริ่มต้น
            Vector3 startPos = spawnPoint.position;
            Gizmos.DrawWireSphere(startPos, sphereRadius);

            // จุดสิ้นสุด
            Vector3 endPos = startPos + (Vector3.up * popUpHeight);
            Gizmos.DrawWireSphere(endPos, sphereRadius);

            // เส้นเชื่อม
            Gizmos.DrawLine(startPos, endPos);

            // แสดงลูกศรชี้ขึ้น
            Vector3 arrowSize = Vector3.one * 0.1f;
            Vector3 middlePoint = Vector3.Lerp(startPos, endPos, 0.5f);
            DrawArrow(middlePoint, Vector3.up * 0.2f);
        }
    }

    // ฟังก์ชันวาดลูกศร
    private void DrawArrow(Vector3 pos, Vector3 direction)
    {
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0) * Vector3.right * 0.1f;
        Vector3 arrowTip = pos + direction;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0) * Vector3.left * 0.1f;

        Gizmos.DrawRay(arrowTip, right);
        Gizmos.DrawRay(arrowTip, left);
    }
    #endregion
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        if (treasureItem != null)
        {
            // เก็บตำแหน่งเริ่มต้น
            originalItemPosition = treasureItem.transform.position;
            treasureItem.SetActive(false);
        }
    }

    public void SpawnTreasure()
    {
        if (!hasSpawned && treasureItem != null)
        {
            Debug.Log("Spawning Treasure");
            StartCoroutine(PopUpAnimation());
            hasSpawned = true;
        }
    }

    private IEnumerator PopUpAnimation()
    {
        // ย้าย item ไปที่จุดเริ่มต้นและแสดง
        treasureItem.transform.position = spawnPoint.position;
        treasureItem.SetActive(true);
        Debug.Log($"Starting position: {spawnPoint.position}");

        Vector3 startPos = spawnPoint.position;
        Vector3 endPos = startPos + (Vector3.up * popUpHeight);
        Debug.Log($"End position will be: {endPos}");

        // เล่น effect ถ้ามี
        if (sparkleEffect != null)
        {
            Instantiate(sparkleEffect, startPos, Quaternion.identity);
        }

        if (itemAppearSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(itemAppearSound);
        }

        float elapsedTime = 0;
        while (elapsedTime < popUpDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / popUpDuration;
            
            // ใช้ ease out cubic
            float easedProgress = 1 - Mathf.Pow(1 - progress, 3);
            
            Vector3 newPosition = Vector3.Lerp(startPos, endPos, easedProgress);
            treasureItem.transform.position = newPosition;
            Debug.Log($"Current position: {newPosition}");
            
            yield return null;
        }

        // ให้แน่ใจว่า item อยู่ในตำแหน่งสุดท้าย
        treasureItem.transform.position = endPos;
        Debug.Log($"Final position: {endPos}");
    }

    // เพิ่มฟังก์ชัน reset สำหรับทดสอบ
    public void ResetTreasure()
    {
        hasSpawned = false;
        if (treasureItem != null)
        {
            treasureItem.transform.position = originalItemPosition;
            treasureItem.SetActive(false);
        }
    }
}