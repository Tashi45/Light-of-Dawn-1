using UnityEngine;

public class SettingMenuActivate : MonoBehaviour
{
    public GameObject SettingsMenu;
    private GameObject player;
    private MonoBehaviour playerMovementScript;
    private Rigidbody2D playerRigidbody;

    void Start()
    {
        // หา GameObject ของผู้เล่น
        player = GameObject.FindGameObjectWithTag("Player");
        
        if (player != null)
        {
            // หา PlayerMovement script และ Rigidbody2D
            playerMovementScript = player.GetComponent("PlayerMovement") as MonoBehaviour;
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            
            if (playerMovementScript == null)
            {
                Debug.LogError("PlayerMovement script not found on player.");
            }
            if (playerRigidbody == null)
            {
                Debug.LogError("Rigidbody2D not found on player.");
            }
        }
        else
        {
            Debug.LogError("Player not found. Make sure it has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleSettingsMenu();
        }
    }

    void ToggleSettingsMenu()
    {
        // สลับสถานะเมนูการตั้งค่า
        SettingsMenu.SetActive(!SettingsMenu.activeSelf);

        if (playerMovementScript != null)
        {
            // สลับการควบคุมการเคลื่อนที่
            playerMovementScript.enabled = !SettingsMenu.activeSelf;

            // หยุดการเคลื่อนที่ทันทีเมื่อเปิดเมนู
            if (SettingsMenu.activeSelf && playerRigidbody != null)
            {
                playerRigidbody.velocity = Vector2.zero;
            }
        }
    }

    // เพิ่มฟังก์ชันสำหรับปุ่มปิดเมนูถ้าต้องการ
    public void CloseSettings()
    {
        if (SettingsMenu.activeSelf)
        {
            ToggleSettingsMenu();
        }
    }
}