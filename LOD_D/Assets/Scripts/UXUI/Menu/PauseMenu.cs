using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused = false;
    
    public GameObject pauseMenuUI;
    public AudioMixer audiomixer;
    
    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        // Load previously saved volume settings
        LoadVolume();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        } 
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }
    
    public void UpdateMusicVolume(float volume) 
    {
        audiomixer.SetFloat("MusicVolume", volume);
    }

    public void UpdateSoundVolume(float volume)
    {
        audiomixer.SetFloat("SFXVolume", volume);
    }

    public void SaveVolume()
    {
        audiomixer.GetFloat("MusicVolume", out float musicVolume);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        
        audiomixer.GetFloat("SFXVolume", out float sfxVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    public void LoadVolume()
    {
        // Load saved volumes or use default if not set
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0f);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 0f);

        // Set sliders
        musicSlider.value = musicVolume;
        sfxSlider.value = sfxVolume;

        // Apply volumes to mixer
        audiomixer.SetFloat("MusicVolume", musicVolume);
        audiomixer.SetFloat("SFXVolume", sfxVolume);
    }
    
    public void QuitGame()
    {
        // Save volume settings before quitting
        SaveVolume();
        Debug.Log("Quit");
        Application.Quit();
    }
}