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
    public GameObject attempText;
    
    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.CompareTag("Player"))
        {
            StartCoroutine(ActivateBossSequence());
            GetComponent<Collider2D>().enabled = false;
        }
    }

    private IEnumerator ActivateBossSequence()
    {
        // เล่นเสียง SFX ก่อน
        AudioManager.Instance.PlaySFX("LargeBoss");
        yield return new WaitForSeconds(0.1f);

        // ค่อยๆ activate UI elements
        rockCountText.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        rockVauleText.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        attempText.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        bossHealthBar.SetActive(true);
        yield return new WaitForSeconds(0.1f);

        // สุดท้ายค่อย activate boss และเล่น BGM
        boss.SetActive(true);
        AudioManager.Instance.PlayBGM("BossBGM");
    }
}