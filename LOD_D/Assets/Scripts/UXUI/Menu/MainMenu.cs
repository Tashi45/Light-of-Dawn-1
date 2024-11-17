using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject SettingsMenu;
    public AudioMixer audiomixer;


    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void OnQuitClicked()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SettingsMenu.SetActive(!SettingsMenu.activeSelf);
        }
    }
    
    public void SetVolume(float volume) 
    {
        audiomixer.SetFloat("volume", volume);
    }
    
    
}
