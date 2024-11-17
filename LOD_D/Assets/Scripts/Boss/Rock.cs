using UnityEngine;

public class Rock : MonoBehaviour
{
    public int value;
    private bool isBeingHeld = false;
    private bool isInCollector = false;

    public void SetHeld(bool held)
    {
        isBeingHeld = held;
        Debug.Log($"Rock is being held: {held}");
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }

    public void SetInCollector(bool inCollector)
    {
        isInCollector = inCollector;
        Debug.Log($"Rock is in collector: {inCollector}");
    }

    public bool IsInCollector()
    {
        return isInCollector;
    }
    
    // เปลี่ยนเป็น OnCollision2D
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // เพิ่ม Debug ให้ละเอียดขึ้น
        Debug.Log($"OnCollisionEnter2D - Colliding with: {collision.gameObject.name}");
        Debug.Log($"IsBeingHeld: {isBeingHeld}");
        Debug.Log($"Collision object tag: {collision.gameObject.tag}");

        // ตรวจสอบว่า AudioManager exists
        if (AudioManager.Instance == null)
        {
            Debug.LogError("No AudioManager found in scene!");
            return;
        }

        // ตรวจสอบการชนกับพื้น
        if (!isBeingHeld && collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("Attempting to play RockDropping sound");
            AudioManager.Instance.PlaySFX("RockDropping");
        }
    }

    private void OnDestroy()
    {
        if (!isInCollector && isBeingHeld)
        {
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}