using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter(Collision rockCol)
    {
        if (rockCol.gameObject.tag == "Ground")
        {
            Destroy(gameObject);
        }
    }
}
