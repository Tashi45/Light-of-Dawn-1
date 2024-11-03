using UnityEngine;
using System.Collections.Generic;

public class RockCollector : MonoBehaviour
{
    public BossChapterOne boss;
    public int maxRockCapacity = 4;
    public int targetSum = 21;
    public float damageAmount = 25f;
    
    private List<Rock> collectedRocks = new List<Rock>();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rock") && collectedRocks.Count < maxRockCapacity)
        {
            Rock rock = other.GetComponent<Rock>();
            if (rock != null && rock.IsBeingHeld())
            {
                rock.SetInCollector(true); // เซ็ตสถานะว่าอยู่ใน collector
                collectedRocks.Add(rock);
                Destroy(other.gameObject); // ตอนนี้จะทำลายได้เพราะ isInCollector = true
                CheckRocks();
            }
        }
    }

    void CheckRocks()
    {
        int sum = 0;
        foreach (Rock rock in collectedRocks)
        {
            sum += rock.value;
        }

        Debug.Log($"Current sum: {sum}, Rocks count: {collectedRocks.Count}"); // Debug log

        // เช็คทั้งผลรวมและจำนวนหิน
        if (sum == targetSum && collectedRocks.Count == maxRockCapacity)
        {
            if (boss != null)
            {
                boss.TakeDamage(damageAmount);
                Debug.Log($"Boss took damage! Sum: {sum}, Rocks: {collectedRocks.Count}");
            }
            ClearRocks();
        }
        else if (sum > targetSum || collectedRocks.Count > maxRockCapacity)
        {
            Debug.Log("Clearing rocks - Sum exceeded or too many rocks");
            ClearRocks();
        }
        else if (collectedRocks.Count == maxRockCapacity && sum != targetSum)
        {
            Debug.Log("Clearing rocks - Wrong sum with max rocks");
            ClearRocks();
        }
    }

    public int GetCurrentCount()
    {
        return collectedRocks.Count;
    }

    public int GetCurrentSum()
    {
        int sum = 0;
        foreach (Rock rock in collectedRocks)
        {
            sum += rock.value;
        }
        return sum;
    }

    void ClearRocks()
    {
        collectedRocks.Clear();
        Debug.Log("Rocks cleared!");
    }
}

