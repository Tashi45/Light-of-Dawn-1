using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Analytics;
using UnityEngine.Analytics;

public class Analytic : MonoBehaviour
{
    //public PlayerHealth _playerHealth;
    //public Keypad Keypad;
   // public BossChapterOne boss;
    void Start()
    {
        Initialized();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerDie(1);
        }

        // if (_playerHealth.health <= 0 && _playerHealth.hasDied != true)
        // {
        //     PlayerDie(1);
        // }
        

        // if (Keypad.Ans.text != Keypad.Answer && Keypad.IsCorrect != true)
        // {
        //     PlayerPuzzle(1);
        // }
    }
    
    private async void Initialized()
    {
        await UnityServices.InitializeAsync();
        AnalyticsService.Instance.StartDataCollection();
    }
    
    public void PlayerDie(int Playerdead_Count)
    {
        CustomEvent customEventInput = new CustomEvent("PlayerDeath")
        {
            {"Playerdead_Count", Playerdead_Count}
        };
    
        AnalyticsService.Instance.RecordEvent(customEventInput);
        Debug.Log($"Player died. deaths+ {Playerdead_Count}");
    }
    public void PlayerPuzzle(int PuzzleCount)
    {
        CustomEvent customEventInput = new CustomEvent("PlayerPuzzleCount")
        {
            {"PuzzleCount", PuzzleCount}
        };
    
        AnalyticsService.Instance.RecordEvent(customEventInput);
        Debug.Log($"Player puzzle. wrong+ {PuzzleCount}");
    }
    public void BossDead (int BossDeadCount)
    {
        CustomEvent customEventInput = new CustomEvent("BossDead")
        {
            {"BossDeadCount", BossDeadCount}
        };
    
        AnalyticsService.Instance.RecordEvent(customEventInput);
        Debug.Log($"Boss. Dead+ {BossDeadCount}");
    }
    
}
