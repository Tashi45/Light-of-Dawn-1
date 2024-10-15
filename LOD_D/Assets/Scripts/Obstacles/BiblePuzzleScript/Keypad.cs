using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keypad : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Text Ans;
    public GameObject Door;

    private string Answer = "123456";

    public void Start()
    {
      
    }


    public void Number(int number)
    {
        Ans.text += number.ToString();
    }

    public void Execute()
    {
        if (Ans.text == Answer)
        {
            Ans.text = "CORRECT";
            animator.SetBool("IsOpen",true);
            //Door.SetActive(false);
        }
        else
        {
          Ans.text = "INCORRECT";
        }
    }

    public void cleanAns()
    {
        Ans.text = "";
    }
}
