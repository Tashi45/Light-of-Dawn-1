using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Text Ans;
    public GameObject Door;

    private string Answer = "123456";
    private bool isCorrect = false;
    private bool isCooldown = false;
    
    // เพิ่มตัวแปรสำหรับอ้างอิง NumpadInteraction
    private NumpadInteraction numpadInteraction;
    private GameObject player;
    private MonoBehaviour playerMovementScript;
    
    void Start()
    {
        // เปลี่ยนจาก GetComponentInParent เป็น GetComponent
        numpadInteraction = GetComponent<NumpadInteraction>();
        // if (numpadInteraction == null)
        // {
        //     Debug.LogError("NumpadInteraction script not found on the same GameObject!");
        // }

        // หา player และ movement script
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            //PlayerMovement playerMovementScript = player.GetComponent<PlayerMovement>();
            playerMovementScript = player.GetComponent("PlayerMovement")as MonoBehaviour;
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

            // ตรวจจับปุ่ม Enter สำหรับยืนยัน
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                Execute();
            }

            // ตรวจจับปุ่ม Backspace เพื่อลบทีละตัว และ Delete เพื่อลบทั้งหมด
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
        if (Ans.text.Length >= 6 && Ans.text != "CORRECT" && Ans.text != "INCORRECT") return;
        if (!isCorrect && !isCooldown)
        {
            Ans.text += number.ToString();
        }
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
        
        // เปิดการเคลื่อนที่ของผู้เล่นก่อนที่จะปิด keypad UI
        if (playerMovementScript != null)
        {
            playerMovementScript.enabled = true;
        }

        // หากมี parent object ที่ควบคุม UI ทั้งหมด ให้ปิด parent แทน
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