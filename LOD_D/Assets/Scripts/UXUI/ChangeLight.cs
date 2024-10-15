using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ChangeLight : MonoBehaviour
{
   [SerializeField] private float targetPos = -3f;
   [SerializeField] private Light2D mainlight;
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
      if (playerTransform.transform.position.y <= targetPos)
      {
         animator.SetBool("LightUp",false);
         animator.SetBool("LightDown",true);
      }

      else if (playerTransform.transform.position.y >= targetPos)
      {
         animator.SetBool("LightDown",false);
         animator.SetBool("LightUp",true);
      }

      
   }
}
