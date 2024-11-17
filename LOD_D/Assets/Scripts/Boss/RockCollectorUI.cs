using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RockCollectorUI : MonoBehaviour
{
    public RockCollector collector;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI sumText;
    public TextMeshProUGUI wrongAttemptsText;
    
    void Update()
    {
        if (collector != null)
        {
            countText.text = $"Rocks: {collector.GetCurrentCount()}/4";
            sumText.text = $"Sum: {collector.GetCurrentSum()}/21";
            wrongAttemptsText.text = $"Wrong Attempts: {collector.GetWrongAttempts()}/2";
        }
    }
}