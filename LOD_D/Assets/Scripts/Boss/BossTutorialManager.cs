using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BossTutorialManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Button continueButton;
    public PlayerMovement playerMovement;  // เปลี่ยนเป็น public เพื่อลากใน Inspector

    [Header("Animation Settings")]
    public float fadeInDuration = 0.5f;

    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = tutorialPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = tutorialPanel.AddComponent<CanvasGroup>();

        if (tutorialPanel != null)
            tutorialPanel.SetActive(false);

        // ถ้าไม่ได้ assign ใน Inspector ให้หาในฉาก
        if (playerMovement == null)
        {
            playerMovement = FindObjectOfType<PlayerMovement>();
            Debug.Log($"Found PlayerMovement: {playerMovement != null}");
        }
    }

    public void ShowTutorial()
    {
        Debug.Log($"ShowTutorial called, playerMovement: {playerMovement != null}");
        // ปิดการทำงานของ Player
        if (playerMovement != null)
        {
            playerMovement.canMove = false;
            Debug.Log("Player movement disabled");
        }

        string formattedText = "ROCK COLLECTOR INSTRUCTIONS\n" +
                               "COLLECT POWERFUL ROCKS TO DEFEAT THE BOSS!\n" +
                               "USE R KEY TO GRAB ROCKS\n" +
                               "EACH ROCK HAS A POWER VALUE (<color=#FFFFFF>3</color>, <color=#FFFFFF>5</color>, OR " +
                               "<color=#FFFFFF>8</color>)\n" +
                               "LOAD EXACTLY 4 ROCKS INTO THE LAUNCHER\n" +
                               "COMBINED POWER MUST EQUAL <color=#FFFFFF>21</color>\n" +
                               "CHOOSE YOUR ROCKS WISELY!";
            
        tutorialText.text = formattedText;

        // แสดง panel และหยุดเกม
        Time.timeScale = 0f;
        tutorialPanel.SetActive(true);

        // Fade in effect
        canvasGroup.alpha = 0f;
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 1f, fadeInDuration)
            .SetUpdate(true);

        // ตั้งค่าปุ่ม continue
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(CloseTutorial);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && tutorialPanel.activeSelf)
        {
            CloseTutorial();
        }
    }

    private void CloseTutorial()
    {
        DOTween.To(() => canvasGroup.alpha, x => canvasGroup.alpha = x, 0f, fadeInDuration)
            .SetUpdate(true)
            .OnComplete(() => {
                tutorialPanel.SetActive(false);
                Time.timeScale = 1f;
            
                if (playerMovement != null)
                {
                    playerMovement.canMove = true;
                    Debug.Log("Player movement enabled");
                }
            });
    }

    private void OnDestroy()
    {
        DOTween.Kill(canvasGroup);
    }
}