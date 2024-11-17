using UnityEngine;

[System.Serializable]
public class BridgeTriggerPair
{
    public GameObject bridge;
    public GameObject triggerPoint;
    public float triggerRadius = 2f;  // เพิ่มระยะการทริกเกอร์สำหรับแต่ละจุด
}

public class Artifact : MonoBehaviour
{
    [SerializeField] GameObject _artifact;
    [SerializeField] BridgeTriggerPair[] _bridgePairs;
    public bool IsArtifact { get; private set; } = false;

    void Start()
    {
        foreach (BridgeTriggerPair pair in _bridgePairs)
        {
            if (pair.bridge != null)
            {
                pair.bridge.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (!IsArtifact) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (BridgeTriggerPair pair in _bridgePairs)
            {
                float distance = Vector2.Distance(transform.position, pair.triggerPoint.transform.position);
                if (distance < pair.triggerRadius)
                {
                    ShowBridge(pair);
                    break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _artifact)
        {
            IsArtifact = true;
            Debug.Log("Collected artifact!");
            Destroy(other.gameObject);
        }
    }

    private void ShowBridge(BridgeTriggerPair pair)
    {
        if (pair.bridge != null && !pair.bridge.activeSelf)
        {
            pair.bridge.SetActive(true);
            Debug.Log($"Bridge {pair.bridge.name} appeared!");
        }
    }

    // แสดง Gizmos ในหน้า Scene View
    private void OnDrawGizmos()
    {
        DrawTriggerGizmos(false);
    }

    // แสดง Gizmos แม้ไม่ได้เลือก GameObject
    private void OnDrawGizmosSelected()
    {
        DrawTriggerGizmos(true);
    }

    private void DrawTriggerGizmos(bool selected)
    {
        if (_bridgePairs == null) return;

        foreach (BridgeTriggerPair pair in _bridgePairs)
        {
            if (pair.triggerPoint != null)
            {
                // กำหนดสีของ Gizmos
                if (selected)
                {
                    Gizmos.color = new Color(0, 1, 0, 0.5f); // สีเขียวโปร่งใส เมื่อเลือก
                }
                else
                {
                    Gizmos.color = new Color(1, 1, 0, 0.2f); // สีเหลืองโปร่งใส เมื่อไม่ได้เลือก
                }

                // วาดวงกลมแสดงระยะ Trigger
                Gizmos.DrawWireSphere(pair.triggerPoint.transform.position, pair.triggerRadius);
                
                // วาดเส้นเชื่อมระหว่างจุด Trigger กับ Bridge
                if (pair.bridge != null)
                {
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(pair.triggerPoint.transform.position, pair.bridge.transform.position);
                }
            }
        }
    }
}