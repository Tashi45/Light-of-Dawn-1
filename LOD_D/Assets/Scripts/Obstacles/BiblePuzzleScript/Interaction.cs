using UnityEngine;

public class Interaction : MonoBehaviour
{
    public GameObject objectToActivate; // อ้างอิงถึง GameObject ที่จะเปิด
    public float interactionDistance = 2f; // ระยะห่างที่สามารถโต้ตอบได้
    public KeyCode interactionKey = KeyCode.E; // ปุ่มที่ใช้ในการโต้ตอบ

    private GameObject player; // อ้างอิงถึง GameObject ของผู้เล่น

    void Start()
    {
        // หา GameObject ของผู้เล่น (สมมติว่ามี tag เป็น "Player")
        player = GameObject.FindGameObjectWithTag("Player");
        objectToActivate.SetActive(false);
        
        // ตรวจสอบว่าพบ player และ objectToActivate หรือไม่
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure it has the 'Player' tag.");
        }
        if (objectToActivate == null)
        {
            Debug.LogError("Object to activate is not assigned.");
        }
    }

    void Update()
    {
        if (player != null && objectToActivate != null)
        {
            // คำนวณระยะห่างระหว่างผู้เล่นกับ numpad
            float distance = Vector2.Distance(transform.position, player.transform.position);

            // ตรวจสอบว่าผู้เล่นอยู่ในระยะโต้ตอบและกดปุ่มที่กำหนด
            if (distance <= interactionDistance && Input.GetKeyDown(interactionKey))
            {
                // สลับสถานะการแสดงของ objectToActivate
                objectToActivate.SetActive(!objectToActivate.activeSelf);
            }
        }
    }

    // วาดเส้น Gizmos เพื่อแสดงระยะโต้ตอบใน Scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}