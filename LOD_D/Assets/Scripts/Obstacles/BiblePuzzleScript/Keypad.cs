using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    [SerializeField] private Text Ans;
    public GameObject Door;
    
    /*public GameObject Player;
    private PlayerMovement playerMovement;*/

    private string Answer = "123456";
    private bool isCorrect = false;
    private bool isCooldown = false;
    
    
    public void Number(int number)
    {
        if (!isCorrect && !isCooldown)
        {
            Ans.text += number.ToString();
        }
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "CORRECT";
            Door.SetActive(false);
            isCorrect = true;
            StartCoroutine(DisableKeypad());
            
        }
        else
        {
          StartCoroutine(IncorrectCooldown()); 
        }
    }

    public void Reset()
    {
        Ans.text = "";
    }

    private IEnumerator DisableKeypad()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
    
    private IEnumerator IncorrectCooldown()
    {
        Ans.text = "INCORRECT";
        isCooldown = true;
        yield return new WaitForSeconds(1f);
        Ans.text = ""; // ล้างข้อความเพื่อให้ใส่รหัสใหม่
        isCooldown = false;
    }
}
