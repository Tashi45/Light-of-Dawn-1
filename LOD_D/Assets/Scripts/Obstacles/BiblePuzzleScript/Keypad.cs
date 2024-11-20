using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Text Ans;
    public GameObject Door;

    private string Answer = "2360";
    private bool isCorrect = false;
    private bool isCooldown = false;
    
    private NumpadInteraction numpadInteraction;
    private GameObject player;
    private MonoBehaviour playerMovementScript;
    
    void Start()
    {
        numpadInteraction = GetComponent<NumpadInteraction>();
        if (numpadInteraction == null)
        {
            Debug.LogError("NumpadInteraction script not found on the same GameObject!");
        }

        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovementScript = player.GetComponent("PlayerMovement") as MonoBehaviour;
            if (playerMovementScript == null)
            {
                Debug.LogError("PlayerMovement script not found on player!");
            }
        }
        else
        {
            Debug.LogError("Player not found!");
        }
    }
    
    void Update()
    {
        if (!isCorrect && !isCooldown)
        {
            // ตรวจจับการกดปุ่มตัวเลข 0-9 บน keyboard ปกติ
            for (KeyCode key = KeyCode.Alpha0; key <= KeyCode.Alpha9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    int number = (int)key - (int)KeyCode.Alpha0;
                    Number(number);
                }
            }

            // ตรวจจับการกดปุ่มตัวเลข 0-9 บน numpad
            for (KeyCode key = KeyCode.Keypad0; key <= KeyCode.Keypad9; key++)
            {
                if (Input.GetKeyDown(key))
                {
                    int number = (int)key - (int)KeyCode.Keypad0;
                    Number(number);
                }
            }

            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Execute();
            }

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                RemoveLastDigit();
            }
            else if (Input.GetKeyDown(KeyCode.Delete))
            {
                cleanAns();
            }
        }
    }
    
    public void Number(int number)
    {
        if (Ans.text.Length >= 4 && Ans.text != "CORRECT" && Ans.text != "INCORRECT") return;
        if (!isCorrect && !isCooldown)
        {
            Ans.text += number.ToString();
        }
    }
    
    public bool IsCorrect
    {
        get { return isCorrect; }
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "CORRECT";
            Door.SetActive(false);
            isCorrect = true;
            StartCoroutine(DisableKeypad());
        }
        else
        {
            StartCoroutine(IncorrectCooldown()); 
        }
    }

    private IEnumerator DisableKeypad()
    {
        yield return new WaitForSeconds(1f);
        
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // ปิดการใช้งาน NumpadInteraction
        if (numpadInteraction != null)
        {
            numpadInteraction.enabled = false;
        }

        Transform parentUI = transform.parent;
        if (parentUI != null)
        {
            parentUI.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator IncorrectCooldown()
    {
        Ans.text = "INCORRECT";
        isCooldown = true;
        yield return new WaitForSeconds(1f);
        Ans.text = "";
        isCooldown = false;
    }

    public void cleanAns()
    {
        Ans.text = "";
    }

    public void RemoveLastDigit()
    {
        if (Ans.text.Length > 0 && Ans.text != "CORRECT" && Ans.text != "INCORRECT")
        {
            Ans.text = Ans.text.Substring(0, Ans.text.Length - 1);
        }
    }
}