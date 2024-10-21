using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEffect : MonoBehaviour
{
    public float ghostDelay;
    private float ghostDelaySecond;
    public GameObject ghostObject;
    public bool makeGhost = false;
    
    // Start is called before the first frame update
    void Start()
    {
        ghostDelaySecond = ghostDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (makeGhost)
        {
            if (ghostDelaySecond>0) 
            {
             ghostDelaySecond -= Time.deltaTime; 
            }
            else 
            {
              //generate a effect
             GameObject currentGhost = Instantiate(ghostObject, transform.position, transform.rotation);
             Sprite currentSprite = GetComponent<SpriteRenderer>().sprite;
             currentGhost.transform.localScale = this.transform.localScale;
             currentGhost.GetComponent<SpriteRenderer>().sprite = currentSprite;
             
             ghostDelaySecond = ghostDelay; 
             Destroy(currentGhost,1f);
            }
        }
    }
}
