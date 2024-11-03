using UnityEngine;

public class LeverController : MonoBehaviour
{
    public GameObject[] stages;
    public Transform[] leverPositions;
    public SpriteRenderer circleRenderer;
    public float interactionDistance = 2f; // ระยะห่างที่ผู้เล่นสามารถโต้ตอบกับ lever ได้

    private int currentStage = 0;
    private Transform playerTransform;

    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player not found. Make sure the player has the 'Player' tag.");
        }
        SetActiveStage(0);
        UpdateCircleColor();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            for (int i = 0; i < leverPositions.Length; i++)
            {
                if (Vector3.Distance(playerTransform.position, leverPositions[i].position) <= interactionDistance)
                {
                    SwitchStage(i);
                    break;
                }
            }
        }
    }

    void SwitchStage(int leverIndex)
    {
        currentStage = (currentStage + 1) % stages.Length;
        SetActiveStage(currentStage);
        UpdateCircleColor();
    }

    void SetActiveStage(int stageIndex)
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == stageIndex);
        }
    }

    void UpdateCircleColor()
    {
        if (circleRenderer != null)
        {
            circleRenderer.color = (currentStage == 3) ? Color.green : Color.red;
        }
    }
}