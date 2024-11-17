using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class RockCollector : MonoBehaviour
{
    public BossChapterOne boss;
    public int maxRockCapacity = 4;
    public int targetSum = 21;
    public float damageAmount = 25f;
    public float healAmount = 25f;  // Amount to heal boss when player makes enough mistakes
    
    [Header("Wrong Attempts UI")]
    public Image wrongAttemptsBar;  // Bar showing progress towards boss heal
    public int attemptsNeededForHeal = 2;  // Number of wrong attempts needed to heal boss
    
    private List<Rock> collectedRocks = new List<Rock>();
    private int wrongAttempts = 0;

    void Start()
    {
        UpdateWrongAttemptsUI();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rock") && collectedRocks.Count < maxRockCapacity)
        {
            Rock rock = other.GetComponent<Rock>();
            if (rock != null && rock.IsBeingHeld())
            {
                rock.SetInCollector(true);
                collectedRocks.Add(rock);
                Destroy(other.gameObject);
                CheckRocks();
            }
        }
    }

    void CheckRocks()
    {
        int sum = 0;
        foreach (Rock rock in collectedRocks)
        {
            sum += rock.value;
        }

        Debug.Log($"Current sum: {sum}, Rocks count: {collectedRocks.Count}");

        if (sum == targetSum && collectedRocks.Count == maxRockCapacity)
        {
            if (boss != null)
            {
                boss.TakeDamage(damageAmount);
                Debug.Log($"Boss took damage! Sum: {sum}, Rocks: {collectedRocks.Count}");
            }
            ClearRocks();
        }
        else if (collectedRocks.Count == maxRockCapacity)
        {
            // Wrong combination submitted
            HandleWrongAttempt();
            ClearRocks();
        }
        else if (sum > targetSum)
        {
            // Sum exceeded
            HandleWrongAttempt();
            ClearRocks();
        }
    }

    void HandleWrongAttempt()
    {
        wrongAttempts++;
        UpdateWrongAttemptsUI();
        
        if (wrongAttempts >= attemptsNeededForHeal)
        {
            // Heal boss
            if (boss != null)
            {
                float newHealth = Mathf.Min(boss.bossHealth + (boss.bossMaxHealth * (healAmount / 100f)), boss.bossMaxHealth);
                boss.bossHealth = newHealth;
                boss.UpdateHealthBar();
                Debug.Log($"Boss healed! New health: {boss.bossHealth}");
            }
            
            // Reset wrong attempts
            wrongAttempts = 0;
            UpdateWrongAttemptsUI();
            
            // Play heal effect or animation here if needed
        }
    }

    void UpdateWrongAttemptsUI()
    {
        if (wrongAttemptsBar != null)
        {
            float fillAmount = (float)wrongAttempts / attemptsNeededForHeal;
            wrongAttemptsBar.fillAmount = fillAmount;
            
            // Optional: Change color based on progress
            wrongAttemptsBar.color = Color.Lerp(Color.yellow, Color.red, fillAmount);
        }
    }

    public int GetCurrentCount()
    {
        return collectedRocks.Count;
    }

    public int GetCurrentSum()
    {
        int sum = 0;
        foreach (Rock rock in collectedRocks)
        {
            sum += rock.value;
        }
        return sum;
    }

    public int GetWrongAttempts()
    {
        return wrongAttempts;
    }

    void ClearRocks()
    {
        collectedRocks.Clear();
        Debug.Log("Rocks cleared!");
    }
}