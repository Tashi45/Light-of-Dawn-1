using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] bgmSounds, sfxSounds, ambientSounds;
    public AudioSource bgmSource, sfxSource, ambientSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           //DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Chapter1_Scene1")
        {
            PlayBGM("ForestBGM1");
            PlayAmbient("ForestWind");
        }
        else if (SceneManager.GetActiveScene().name == "Chapter1_Scene2")
        {
            PlayBGM("ForestBGM2");
            PlayAmbient("ForestWind");
        }
        else if (SceneManager.GetActiveScene().name == "Chapter1_Scene3")
        {
            PlayBGM("CaveBGM");
            PlayAmbient("CaveAmbient1");
        }
        else if (SceneManager.GetActiveScene().name == "Chapter1_Scene4")
        {
            PlayBGM("CaveBGM");
            PlayAmbient("CaveAmbient2");
        }
    }

    public void PlayBGM(string name)
    {
        Sound s = Array.Find(bgmSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            bgmSource.clip = s.clip;
            bgmSource.Play();
        }
    }
    
    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            ambientSource.clip = s.clip;
            ambientSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        
        if (s == null)
        {
            Debug.Log("Sound not found");
        }

        else
        {
            sfxSource.PlayOneShot(s.clip);
        }
    }

}
