using UnityEngine;

public class Artifact : MonoBehaviour
{
    [SerializeField] GameObject _artifact;
    [SerializeField] GameObject[] _hiddenBridges; // เก็บสะพานหลายอันในรูปแบบ array
    public bool IsArtifact { get; private set; } = false;

    void Start()
    {
        // ซ่อนสะพานทั้งหมดตั้งแต่เริ่มเกม
        foreach (GameObject bridge in _hiddenBridges)
        {
            if (bridge != null)
            {
                bridge.SetActive(false);
            }
        }
    }

    void Update()
    {
        // เช็คว่ากด E และมี artifact แล้วหรือยัง
        if (Input.GetKeyDown(KeyCode.E) && IsArtifact)
        {
            ShowAllHiddenBridges();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == _artifact)
        {
            IsArtifact = true;
            Debug.Log("Collected artifact!");
            Destroy(other.gameObject);
        }
    }

    private void ShowAllHiddenBridges()
    {
        foreach (GameObject bridge in _hiddenBridges)
        {
            if (bridge != null)
            {
                bridge.SetActive(true);
                Debug.Log($"Bridge {bridge.name} appeared!");
            }
        }
    }
}