using UnityEngine;

public class NumpadInteraction : MonoBehaviour
{
    public GameObject objectToActivate;
    public float interactionDistance = 2f;
    public KeyCode interactionKey = KeyCode.E;

    private GameObject player;
    private MonoBehaviour playerMovementScript;
    private Rigidbody2D playerRigidbody;
    private Keypad keypadScript; // เพิ่มตัวแปรอ้างอิงไปยัง Keypad script

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        objectToActivate.SetActive(false);
        
        if (player == null)
        {
            Debug.LogError("Player not found. Make sure it has the 'Player' tag.");
        }
        if (objectToActivate == null)
        {
            Debug.LogError("Object to activate is not assigned.");
        }

        // หา Keypad script
        keypadScript = GetComponent<Keypad>();

        if (player != null)
        {
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
    }

    void Update()
    {
        // ถ้า puzzle ถูกแก้แล้ว (isCorrect == true) จะไม่ทำการโต้ตอบกับ numpad
        if (keypadScript != null && keypadScript.IsCorrect)
        {
            return;
        }

        if (player != null && objectToActivate != null && playerMovementScript != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= interactionDistance && Input.GetKeyDown(interactionKey))
            {
                objectToActivate.SetActive(!objectToActivate.activeSelf);
                playerMovementScript.enabled = !objectToActivate.activeSelf;

                if (!playerMovementScript.enabled && playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector2.zero;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}