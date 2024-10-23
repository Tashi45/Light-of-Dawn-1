using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public bool locked;
    public bool keyPickedUp;
    public GameObject keys;
    
    private Animator anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        locked = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Key") && keyPickedUp)
        {
            anim.SetTrigger("open");
            locked = false;
        }
    }

    private void OnDestroy()
    {
        Destroy(this.gameObject);
        Destroy(keys);
    }
}
