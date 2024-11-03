using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ActivateCutScene : MonoBehaviour
{
    [SerializeField] private PlayableDirector _playableDirector;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            _playableDirector.Play();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
