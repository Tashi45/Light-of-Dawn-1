using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerZoneScript : MonoBehaviour
{
    public GameObject boss;
    public GameObject bossHealthBar;
    public GameObject rockCountText;
    public GameObject rockVauleText;
    
    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.CompareTag("Player"))
        {
            AudioManager.Instance.PlayBGM("BossBGM");
            boss.SetActive(true);
            bossHealthBar.SetActive(true);
            rockCountText.SetActive(true);
            rockVauleText.SetActive(true);
            
            gameObject.SetActive(false);
        }
    }
}
