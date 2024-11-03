using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineDestructor : MonoBehaviour
{
    public float lifetime = 5f;
    private float timer = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
