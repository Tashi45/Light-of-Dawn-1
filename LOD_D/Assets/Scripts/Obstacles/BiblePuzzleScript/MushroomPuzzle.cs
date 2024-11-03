using UnityEngine;
using System.Collections.Generic;

public class MushroomPuzzle : MonoBehaviour
{
    [SerializeField] private int[] correctSequence; // ลำดับที่ถูกต้องของเห็ด
    [SerializeField] private GameObject[] mushrooms; // อาเรย์ของเห็ดแต่ละดอก
    [SerializeField] private GameObject player; // อ้างอิง Player
    [SerializeField] private float interactionRange = 2f; // ระยะการมีปฏิสัมพันธ์
    [SerializeField] private GameObject door;
    
    private List<int> playerSequence = new List<int>(); // ลำดับที่ผู้เล่นกดเห็ด
    private bool isPuzzleSolved = false;

    void Update()
    {
        if (isPuzzleSolved) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckMushroomInteraction();
        }
    }

    private void CheckMushroomInteraction()
    {
        for (int i = 0; i < mushrooms.Length; i++)
        {
            float distanceToPlayer = Vector2.Distance(
                player.transform.position,
                mushrooms[i].transform.position
            );

            if (distanceToPlayer <= interactionRange)
            {
                PullMushroom(i);
                break;
            }
        }
    }

    private void PullMushroom(int mushroomIndex)
    {
        // ถ้าเห็ดตัวนั้นถูกกดแล้ว จะไม่สามารถกดซ้ำได้
        if (playerSequence.Contains(mushroomIndex)) return;

        playerSequence.Add(mushroomIndex);
        HideMushroom(mushroomIndex); // ซ่อนเห็ดเมื่อถูกกด
        CheckSequence();
    }

    private void HideMushroom(int mushroomIndex)
    {
        mushrooms[mushroomIndex].SetActive(false); // ซ่อนเห็ด
    }

    private void CheckSequence()
    {
        if (playerSequence.Count == correctSequence.Length)
        {
            bool isCorrect = true;
            for (int i = 0; i < playerSequence.Count; i++)
            {
                if (playerSequence[i] != correctSequence[i])
                {
                    isCorrect = false;
                    break;
                }
            }

            if (isCorrect)
            {
                SolvePuzzle();
            }
            else
            {
                ResetPuzzle();
            }
        }
    }



    private void ResetPuzzle()
    {
        // แสดงเห็ดกลับมา
        foreach (var mushroom in mushrooms)
        {
            mushroom.SetActive(true); // แสดงเห็ดทั้งหมด
        }
    
        playerSequence.Clear();
        Debug.Log("Puzzle Reset - Try again!");
    }

    private void SolvePuzzle()
    {
        isPuzzleSolved = true;
        Debug.Log("Puzzle Solved!");

        OpenDoor();
    }
    private void OpenDoor()
    {
        if (door != null)
        {
            door.SetActive(false); // ทำให้ประตูหายไปหรือเปิดออก
            Debug.Log("Door Opened!");
        }
    }

    // เพิ่มการแสดง Gizmos
    private void OnDrawGizmos()
    {
        if (mushrooms == null) return;

        // วาดวงกลมแสดงระยะสำหรับเห็ดแต่ละดอก
        foreach (GameObject mushroom in mushrooms)
        {
            if (mushroom != null)
            {
                // กำหนดสีของ Gizmos (สีฟ้าโปร่งแสง)
                Gizmos.color = new Color(0, 1, 1, 0.3f);
                // วาดวงกลมทึบแสง
                Gizmos.DrawSphere(mushroom.transform.position, interactionRange);
                
                // กำหนดสีขอบ (สีฟ้าเข้ม)
                Gizmos.color = new Color(0, 1, 1, 1f);
                // วาดเส้นขอบวงกลม
                Gizmos.DrawWireSphere(mushroom.transform.position, interactionRange);
            }
        }
    }
}