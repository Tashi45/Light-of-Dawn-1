using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RockCollectorUI : MonoBehaviour
{
    public RockCollector collector;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI sumText;
    
    void Update()
    {
        if (collector != null)
        {
            // ใช้ฟังก์ชันแทนการเข้าถึง collectedRocks โดยตรง
            countText.text = $"Rocks: {collector.GetCurrentCount()}/4";
            sumText.text = $"Sum: {collector.GetCurrentSum()}/21";
        }
    }
}