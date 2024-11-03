using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lever : MonoBehaviour
{
    [System.Serializable]
    public class StageSprites
    {
        public Sprite stage1;
        public Sprite stage2;
        public Sprite stage3;
        public Sprite stage4;
    }
    
    [Header("Stage Settings")]
    public StageSprites stageSprites;
    public SpriteRenderer leverSprite;
    public int currentStage = 1;
    public int maxStages = 4;

    [Header("Interaction Settings")]
    public float interactionRange = 2f;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject interactionPrompt;

    private GameObject player;
    private LeverPuzzleManager puzzleManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        puzzleManager = FindObjectOfType<LeverPuzzleManager>();
        UpdateLeverVisual();
        
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        bool isInRange = distance <= interactionRange;

        // แสดง/ซ่อน prompt
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(isInRange);
        }

        // ตรวจสอบการกดปุ่มเมื่ออยู่ในระยะ
        if (isInRange && Input.GetKeyDown(interactionKey))
        {
            CycleLeverStage();
            if (puzzleManager != null)
            {
                puzzleManager.PlayLeverSound();
            }
        }
    }

    public void CycleLeverStage()
    {
        currentStage++;
        if (currentStage > maxStages)
        {
            currentStage = 1;
        }
        UpdateLeverVisual();
    }

    private void UpdateLeverVisual()
    {
        switch (currentStage)
        {
            case 1:
                leverSprite.sprite = stageSprites.stage1;
                break;
            case 2:
                leverSprite.sprite = stageSprites.stage2;
                break;
            case 3:
                leverSprite.sprite = stageSprites.stage3;
                break;
            case 4:
                leverSprite.sprite = stageSprites.stage4;
                break;
        }
    }

    public bool IsInStage(int stage)
    {
        return currentStage == stage;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

