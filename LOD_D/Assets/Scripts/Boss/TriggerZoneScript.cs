using UnityEngine;
using System.Collections;

public class TriggerZoneScript : MonoBehaviour 
{
    public GameObject boss;
    public GameObject bossHealthBar;
    public GameObject rockCountText;
    public GameObject rockVauleText;
    public GameObject attempText;
    
    private void Start()
    {
        // Pre-load assets
        PreloadAssets();
    }

    private void PreloadAssets()
    {
        // Pre-instantiate but keep inactive
        boss.SetActive(false);
        bossHealthBar.SetActive(false);
        rockCountText.SetActive(false);
        rockVauleText.SetActive(false);
        attempText.SetActive(false);

        // Preload audio with correct type parameters
        AudioManager.Instance.PreloadAudio("LargeBoss", AudioManager.AudioType.SFX);
        AudioManager.Instance.PreloadAudio("BossBGM", AudioManager.AudioType.BGM);
    }

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
        // ลดการ delay ลงเนื่องจาก preload แล้ว
        AudioManager.Instance.PlaySFX("LargeBoss");
        yield return new WaitForSeconds(0.05f);

        rockCountText.SetActive(true);
        rockVauleText.SetActive(true);
        attempText.SetActive(true);
        bossHealthBar.SetActive(true);
        
        yield return new WaitForSeconds(0.05f);
        boss.SetActive(true);
        AudioManager.Instance.PlayBGM("BossBGM");
    }
}