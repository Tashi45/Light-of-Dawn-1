using UnityEngine;

public class Rock : MonoBehaviour
{
    public int value;
    private bool isBeingHeld = false;
    private bool isInCollector = false; // เพิ่มตัวแปรเช็คว่าอยู่ใน collector หรือไม่

    public void SetHeld(bool held)
    {
        isBeingHeld = held;
    }

    public bool IsBeingHeld()
    {
        return isBeingHeld;
    }

    // เพิ่มฟังก์ชันสำหรับเช็คว่าอยู่ใน collector
    public void SetInCollector(bool inCollector)
    {
        isInCollector = inCollector;
    }

    public bool IsInCollector()
    {
        return isInCollector;
    }

    // ป้องกันการถูกทำลายถ้าไม่ได้อยู่ใน collector
    private void OnDestroy()
    {
        if (!isInCollector && isBeingHeld)
        {
            // ยกเลิกการทำลาย
            GameObject.DontDestroyOnLoad(gameObject);
        }
    }
}