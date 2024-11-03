using UnityEngine;

public class NumpadInteraction : MonoBehaviour
{
    public GameObject objectToActivate;
    public float interactionDistance = 2f;
    public KeyCode interactionKey = KeyCode.E;

    private GameObject player;
    private MonoBehaviour playerMovementScript;
    private Rigidbody2D playerRigidbody; // เพิ่มการอ้างอิงถึง Rigidbody2D

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

        if (player != null)
        {
            //PlayerMovement playerMovementScript = player.GetComponent<PlayerMovement>();
            playerMovementScript = player.GetComponent("PlayerMovement") as MonoBehaviour;
            playerRigidbody = player.GetComponent<Rigidbody2D>(); // หา Rigidbody2D
            
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
        if (player != null && objectToActivate != null && playerMovementScript != null)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);

            if (distance <= interactionDistance && Input.GetKeyDown(interactionKey))
            {
                objectToActivate.SetActive(!objectToActivate.activeSelf);
                playerMovementScript.enabled = !objectToActivate.activeSelf;

                // เมื่อปิดการเคลื่อนที่ ให้หยุดการเคลื่อนที่ของ player ทันที
                if (!playerMovementScript.enabled && playerRigidbody != null)
                {
                    playerRigidbody.velocity = Vector2.zero; // รีเซ็ตความเร็ว
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