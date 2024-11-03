using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeMover : MonoBehaviour
{
    //public GameObject objectToMove;
    public Vector3 newPos; 
    public float delayTime = 1f;

    public Transform waypoint;

    private bool triggered = false;

    private void Start()
    {
        newPos = waypoint.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&!triggered)
        {
            triggered = true;
            Invoke("MoveObject", delayTime);
        }
    }

    private void MoveObject()
    {
        this.gameObject.transform.position = newPos;
    }
}
