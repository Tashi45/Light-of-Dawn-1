using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audiomixer;
    
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        LoadVolume();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OnQuitClicked()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    public void UpdateMusicVolume (float volume) 
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
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }
}
