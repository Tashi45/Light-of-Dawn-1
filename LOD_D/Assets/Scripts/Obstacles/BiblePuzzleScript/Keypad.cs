using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public Animator animator;
    [SerializeField] public Text Ans;
    public GameObject Door;

    public string Answer = "2360";
    private bool isCorrect = false;
    private bool isCooldown = false;

    private NumpadInteraction numpadInteraction;
    private GameObject player;
    private MonoBehaviour playerMovementScript;
    
    void Start()
    {
        //Initialized();
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
           
            isCorrect = true;
            Door.SetActive(false);
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

    public Analytic _analytic;
    private IEnumerator IncorrectCooldown()
    {
        Ans.text = "INCORRECT";
        _analytic.PlayerPuzzle(1);
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
    
    // private async void Initialized()
    // {
    //     await UnityServices.InitializeAsync();
    //     AnalyticsService.Instance.StartDataCollection();
    // }
    // public void PlayerPuzzleCount(string playerMakingPuzzle,int playerPuzzleCount)
    // {
    //     //playerPuzzleCount++;
    //     CustomEvent customEventInput = new CustomEvent("PlayerPuzzle")
    //     {
    //         {"PuzzleCount", playerPuzzleCount},
    //         {"PlayerMakingPuzzle",playerMakingPuzzle}
    //     };
    //
    //     AnalyticsService.Instance.RecordEvent(customEventInput);
    //     Debug.Log($"Player puzzle. Total wrong: {playerPuzzleCount}");
    // }
    // private int playerPuzzleCount = 0;
    // private void PlayerPuzzleCount()
    // {
    //     playerPuzzleCount++;
    //
    //     // Send the death count to Unity Analytics
    //     Analytics.SendEvent("PlayerPuzzleCount", new Dictionary<string, object> {
    //         { "PuzzleCount", playerPuzzleCount }
    //     });
    //
    //     Debug.Log($"Player puzzle. Total wrong: {playerPuzzleCount}");
    // }
}