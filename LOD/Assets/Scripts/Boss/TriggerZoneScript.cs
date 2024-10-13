using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZoneScript : MonoBehaviour
{
    public GameObject boss;
    
    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.CompareTag("Player"))
        {
            boss.SetActive(true);
        }
    }
}
