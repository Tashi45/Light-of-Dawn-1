using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneBossChap2 : MonoBehaviour
{
    public GameObject bossChapter2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            bossChapter2.SetActive(true);
        }
    }
}
