using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabController : MonoBehaviour
{
    public Transform grabDetect;
    public Transform rockHolder;
    public float rayDist;
    
    private GameObject grabbedObject;
    private bool isFacingRight = true;
    private float lastScaleX;

    void Start()
    {
        lastScaleX = transform.localScale.x;
    }

    void Update()
    {
        if (transform.localScale.x != lastScaleX)
        {
            isFacingRight = transform.localScale.x > 0;
            lastScaleX = transform.localScale.x;
            
            if (grabbedObject != null)
            {
                Vector3 rockScale = grabbedObject.transform.localScale;
                rockScale.x = Mathf.Abs(rockScale.x) * (isFacingRight ? 1 : -1);
                grabbedObject.transform.localScale = rockScale;
            }
        }

        RaycastHit2D grabCheck = Physics2D.Raycast(grabDetect.position, Vector2.right * transform.localScale, rayDist);

        if (Input.GetKeyDown(KeyCode.P)||Input.GetKeyDown(KeyCode.R) && grabCheck.collider != null)
        {
            if (grabCheck.collider.gameObject.CompareTag("Rock"))
            {
                grabbedObject = grabCheck.collider.gameObject;
                grabbedObject.transform.parent = rockHolder;
                grabbedObject.transform.position = rockHolder.position;
                grabbedObject.GetComponent<Rigidbody2D>().isKinematic = true;

                // เพิ่มการเรียกใช้ SetHeld
                Rock rock = grabbedObject.GetComponent<Rock>();
                if (rock != null)
                {
                    rock.SetHeld(true);
                }

                Vector3 rockScale = grabbedObject.transform.localScale;
                rockScale.x = Mathf.Abs(rockScale.x) * (isFacingRight ? 1 : -1);
                grabbedObject.transform.localScale = rockScale;

                grabbedObject.GetComponent<Rigidbody2D>().constraints = 
                    RigidbodyConstraints2D.FreezePositionX | 
                    RigidbodyConstraints2D.FreezePositionY | 
                    RigidbodyConstraints2D.FreezeRotation;
            }
        }
        else if (Input.GetKeyUp(KeyCode.P)||Input.GetKeyDown(KeyCode.R) && grabbedObject != null)
        {
            // เพิ่มการเรียกใช้ SetHeld
            Rock rock = grabbedObject.GetComponent<Rock>();
            if (rock != null)
            {
                rock.SetHeld(false);
            }

            grabbedObject.transform.parent = null;
            grabbedObject.GetComponent<Rigidbody2D>().isKinematic = false;
            grabbedObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            grabbedObject = null;
        }
    }
}