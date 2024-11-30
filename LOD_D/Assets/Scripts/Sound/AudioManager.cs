using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] bgmSounds, sfxSounds, ambientSounds;
    public AudioSource bgmSource, sfxSource, ambientSource;
    
    // Dictionary เก็บ clip ที่โหลดแล้ว
    private Dictionary<string, AudioClip> preloadedSFX = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> preloadedBGM = new Dictionary<string, AudioClip>();

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
        // Preload เสียงตาม scene
        PreloadSceneSounds();
    }

    private void PreloadSceneSounds()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        
        // ถ้าเป็น boss scene ให้ preload เสียงที่เกี่ยวข้อง
        if (sceneName == "Chapter1_Scene4")
        {
            PreloadAudio("LargeBoss", AudioType.SFX);
            PreloadAudio("BossBGM", AudioType.BGM);
            
            // เล่นเสียง background ปกติ
            PlayBGM("CaveBGM");
            PlayAmbient("CaveAmbient2");
        }
        else if (sceneName == "Chapter1_Scene1")
        {
            PlayBGM("ForestBGM1");
            PlayAmbient("ForestWind");
        }
        else if (sceneName == "Chapter1_Scene2")
        {
            PlayBGM("ForestBGM2");
            PlayAmbient("ForestWind");
        }
        else if (sceneName == "Chapter1_Scene3")
        {
            PlayBGM("CaveBGM");
            PlayAmbient("CaveAmbient1");
        }
        else if (sceneName == "Chapter1_Scene4 2")
        {
            PlayBGM("BossChapter2BGM");
            PlayAmbient("CaveAmbient3");
        }
    }

    public enum AudioType
    {
        SFX,
        BGM
    }

    public void PreloadAudio(string name, AudioType type)
    {
        Sound[] sourceArray = (type == AudioType.SFX) ? sfxSounds : bgmSounds;
        Dictionary<string, AudioClip> cache = (type == AudioType.SFX) ? preloadedSFX : preloadedBGM;

        if (!cache.ContainsKey(name))
        {
            Sound s = Array.Find(sourceArray, x => x.name == name);
            if (s != null && s.clip != null)
            {
                cache[name] = s.clip;
                // Force load into memory
                s.clip.LoadAudioData();
            }
            else
            {
                Debug.LogWarning($"Sound {name} not found for preloading");
            }
        }
    }

    public void PlayBGM(string name)
    {
        Sound s;
        if (preloadedBGM.ContainsKey(name))
        {
            bgmSource.clip = preloadedBGM[name];
            bgmSource.Play();
            return;
        }

        s = Array.Find(bgmSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }

        bgmSource.clip = s.clip;
        bgmSource.Play();
    }
    
    public void PlayAmbient(string name)
    {
        Sound s = Array.Find(ambientSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }

        ambientSource.clip = s.clip;
        ambientSource.Play();
    }

    public void PlaySFX(string name)
    {
        if (preloadedSFX.ContainsKey(name))
        {
            sfxSource.PlayOneShot(preloadedSFX[name]);
            return;
        }

        Sound s = Array.Find(sfxSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound not found");
            return;
        }

        sfxSource.PlayOneShot(s.clip);
    }
}