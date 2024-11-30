using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counting : MonoBehaviour
{
    public int deathCount = 0;
    public PlayerHealth playerHealth;
    public TextMeshProUGUI deathCountText;
    public bool CountReset = false;
    // Start is called before the first frame update
    
    public TextMeshProUGUI timerText;
    private float timeElapsed;
    void Start()
    {
        if (CountReset)
        {
            PlayerPrefs.DeleteKey("DeathCount");
            PlayerPrefs.DeleteKey("TimeElapsed");
        }
        //deathCountText.text = "Player Deaths: 0";
        deathCount = PlayerPrefs.GetInt("DeathCount", 0);
        deathCountText.text = "Deaths: " + deathCount;
        
        timeElapsed = PlayerPrefs.GetFloat("TimeElapsed", 0f);
        
        
    }
   
    // Update is called once per frame
    void Update()
    {
        if (playerHealth.hasDied != true && playerHealth.health <= 0)
        {
            PlayerDied();
        }
        
        timeElapsed += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timeElapsed / 60);
        int seconds = Mathf.FloorToInt(timeElapsed % 60);
        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        PlayerPrefs.SetFloat("TimeElapsed", timeElapsed);
    }
    
    public void PlayerDied()
    {
        deathCount++;
        PlayerPrefs.SetInt("DeathCount", deathCount);
        deathCountText.text = "Deaths: " + deathCount;
    }
    
}
