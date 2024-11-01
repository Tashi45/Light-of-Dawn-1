using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeLight : MonoBehaviour
{
   [SerializeField] private float targetPos = -3f;
   [SerializeField] private float darkerPos = 0f;
   //[SerializeField] private Light2D mainlight;
   [SerializeField] private Transform playerTransform;
   public Animator animator;

   // private void OnTriggerEnter2D(Collider2D col)
   // {
   //    if (col.CompareTag("Player"))
   //    {
   //       
   //       animator.SetBool("LightDown",true);
   //       
   //    }
   // }

   private void Update()
   { 
       //IDEA first code
      // if (playerTransform.transform.position.y >= targetPos || playerTransform.transform.position.y<=targetPos)
      // {
      //    if (playerTransform.transform.position.y <= targetPos)
      //    {
      //       animator.SetBool("LightUp",false);
      //       animator.SetBool("LightDown",true);
      //    }
      //
      //    else if (playerTransform.transform.position.y >= targetPos)
      //    {
      //       animator.SetBool("LightDown",false);
      //       animator.SetBool("LightUp",true);
      //    }
      // }
      //
      // if (playerTransform.transform.position.y >= darkerPos || playerTransform.transform.position.y <= darkerPos)
      // {
      //    if (playerTransform.transform.position.y <= darkerPos)
      //    {
      //       animator.SetBool("LowLightDown",true);
      //       animator.SetBool("LowLightUp",false);
      //    }
      //    else if (playerTransform.transform.position.y >= darkerPos)
      //    {
      //       animator.SetBool("LowLightDown",false);
      //       animator.SetBool("LowLightUp",true);
      //    }
      // }
      
     //convert to this code
      bool isMovingUp = playerTransform.transform.position.y > targetPos;
      
      animator.SetBool("LightUp", isMovingUp);
      animator.SetBool("LightDown", !isMovingUp);

      bool isMovingUpDarker = playerTransform.transform.position.y > darkerPos;

      animator.SetBool("LowLightUp", isMovingUpDarker);
      animator.SetBool("LowLightDown", !isMovingUpDarker);

      

      
   }
}
