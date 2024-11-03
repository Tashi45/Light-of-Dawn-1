using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverPuzzleManager : MonoBehaviour
{
    public Lever[] levers = new Lever[4];
    public int[] correctStages = new int[4]; // เก็บ stage ที่ถูกต้องของแต่ละคันโยก
    public UnityEvent onPuzzleSolved;
    public AudioClip leverSound;
    public AudioClip solvedSound;
    
    private AudioSource audioSource;
    private bool isPuzzleSolved = false;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void Update()
    {
        if (!isPuzzleSolved)
        {
            CheckPuzzleSolution();
        }
    }

    private void CheckPuzzleSolution()
    {
        bool isSolved = true;
        
        for (int i = 0; i < levers.Length; i++)
        {
            if (!levers[i].IsInStage(correctStages[i]))
            {
                isSolved = false;
                break;
            }
        }

        if (isSolved && !isPuzzleSolved)
        {
            isPuzzleSolved = true;
            if (solvedSound != null)
            {
                audioSource.PlayOneShot(solvedSound);
            }
            onPuzzleSolved.Invoke();
        }
    }

    public void PlayLeverSound()
    {
        if (leverSound != null)
        {
            audioSource.PlayOneShot(leverSound);
        }
    }
}


