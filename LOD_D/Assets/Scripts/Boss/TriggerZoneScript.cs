using UnityEngine;
using System.Collections;

public class TriggerZoneScript : MonoBehaviour 
{
    public GameObject boss;
    public GameObject bossHealthBar;
    public GameObject rockCountText;
    public GameObject rockVauleText;
    public GameObject attempText;
    public BossTutorialManager tutorialManager;  // เพิ่มการอ้างอิงไปยัง tutorial manager
    
    private void Start()
    {
        PreloadAssets();
    }

    private void PreloadAssets()
    {
        boss.SetActive(false);
        bossHealthBar.SetActive(false);
        rockCountText.SetActive(false);
        rockVauleText.SetActive(false);
        attempText.SetActive(false);

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
        AudioManager.Instance.PlaySFX("LargeBoss");
        yield return new WaitForSeconds(0.05f);

        rockCountText.SetActive(true);
        rockVauleText.SetActive(true);
        attempText.SetActive(true);
        bossHealthBar.SetActive(true);
        
        yield return new WaitForSeconds(0.05f);
        boss.SetActive(true);
        
        // แสดง tutorial ก่อนเริ่มเพลง boss
        if (tutorialManager != null)
        {
            tutorialManager.ShowTutorial();
        }

        AudioManager.Instance.PlayBGM("BossBGM");
    }
}