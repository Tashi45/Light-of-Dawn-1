using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSafezone : MonoBehaviour
{
    public GameObject bossChapter2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Enter Safezone");
            bossChapter2.SetActive(false);
        }
    }
}
