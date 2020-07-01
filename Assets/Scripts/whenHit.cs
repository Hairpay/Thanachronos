using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whenHit : MonoBehaviour
{
    public int hit;
    public bool iframes;
    public int side;
    public bool locked;

    void Start()
    {
        gameObject.layer = 11;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Phantom" && iframes == false && locked == false)
        {
            hit++;
            iframes = true;
           
            if(gameObject.transform.position.x < other.transform.position.x)
            {
                side = -1;
            }
            else
            {
                side = 1;
            }

            StopAllCoroutines();
            StartCoroutine(iframe());
        }
    }
    IEnumerator iframe()
    {
        yield return new WaitForSeconds(0.3f);
        iframes = false;
             
    }
}


