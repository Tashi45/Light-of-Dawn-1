using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
 
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
 
    private Queue<DialogueLine> lines;
    private DialogueLine currentLine;
    
    public bool isDialogueActive = false;
    private bool isTyping = false;
    private bool canProceed = false;
 
    public float typingSpeed = 0.2f;
 
    public Animator animator;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
 
        lines = new Queue<DialogueLine>();
    }

    private void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isTyping)
            {
                // ถ้ากำลังพิมพ์อยู่ ให้แสดงข้อความทั้งหมดทันที
                CompleteCurrentLine();
            }
            else if (canProceed)
            {
                // ถ้าพิมพ์จบแล้วและสามารถไปบทสนทนาถัดไปได้
                DisplayNextDialogueLine();
            }
        }
    }

    private void CompleteCurrentLine()
    {
        if (currentLine != null)
        {
            StopAllCoroutines();
            dialogueArea.text = currentLine.line;
            isTyping = false;
            canProceed = true;
        }
    }
 
    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        animator.Play("show");
        lines.Clear();

        // เพิ่มทุกบรรทัดเข้า queue
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
 
        DisplayNextDialogueLine();
    }
 
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        canProceed = false;
        currentLine = lines.Dequeue();
 
        characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
 
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        isTyping = true;
        dialogueArea.text = "";

        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canProceed = true;
    }
 
    void EndDialogue()
    {
        isDialogueActive = false;
        canProceed = false;
        currentLine = null;
        animator.Play("hide");
    }
}