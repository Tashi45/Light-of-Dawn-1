using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Invisfloor : MonoBehaviour
{
    public Transform targetWaypoint; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Box"))
        {
            other.transform.position = targetWaypoint.position;
            //AudioManager.Instance.PlaySFX("Warp");
        }
    }
}
