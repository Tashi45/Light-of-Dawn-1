using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audiomixer;
    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        // รับค่าความละเอียดทั้งหมดและเรียงจากมากไปน้อย
        resolutions = Screen.resolutions
            .Select(resolution => new Resolution 
            { 
                width = resolution.width,
                height = resolution.height,
                refreshRate = resolution.refreshRate
            })
            .OrderByDescending(r => r.width)
            .ThenByDescending(r => r.height)
            .ThenByDescending(r => r.refreshRate)
            .ToArray();

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        
        // ใช้ HashSet เพื่อเก็บความละเอียดที่ไม่ซ้ำกัน
        HashSet<string> uniqueResolutions = new HashSet<string>();
        List<Resolution> uniqueResolutionsList = new List<Resolution>();
        
        int currentResolutionIndex = 0;
        Resolution currentResolution = Screen.currentResolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string resolutionString = $"{resolutions[i].width} x {resolutions[i].height}";
            
            // เพิ่มเฉพาะความละเอียดที่ไม่ซ้ำ
            if (uniqueResolutions.Add(resolutionString))
            {
                options.Add(resolutionString);
                uniqueResolutionsList.Add(resolutions[i]);

                // ตรวจสอบว่าเป็นความละเอียดปัจจุบันหรือไม่
                if (resolutions[i].width == currentResolution.width && 
                    resolutions[i].height == currentResolution.height)
                {
                    currentResolutionIndex = uniqueResolutionsList.Count - 1;
                }
            }
        }

        // อัพเดทตัวแปร resolutions ให้เก็บเฉพาะความละเอียดที่ไม่ซ้ำ
        resolutions = uniqueResolutionsList.ToArray();
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume) 
    {
        audiomixer.SetFloat("volume", volume);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}