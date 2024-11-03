using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ChangeSceneWithTrigger : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private float delayBeforeSceneChange = 2f;
    
    
    [SerializeField] private bool useTransition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (useTransition)
            {
                StartCoroutine("LoadSceneWithDelayAndTimeline");
            }
            else
            {
                SceneManager.LoadScene(sceneToLoad);
            }
        }
        
    }

    private IEnumerator LoadSceneWithDelayAndTimeline()
    {
        
        if (playableDirector != null)
        {
            playableDirector.Play();
        }
        else
        {
            Debug.LogWarning("PlayableDirector not found for transition!");
        }
        yield return new WaitForSeconds(delayBeforeSceneChange);
        SceneManager.LoadScene(sceneToLoad);
        
    }
}
