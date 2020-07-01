using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick2Platforms : MonoBehaviour
{
    public LayerMask layerMask;   
    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Environment");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vectorpos = new Vector2(transform.position.x, transform.position.y + 1);

        RaycastHit2D hit = Physics2D.Raycast(vectorpos, -Vector2.up, 0.75f, layerMask);
        Debug.DrawRay(vectorpos, -Vector2.up * 0.75f);

        if (hit.collider != null)
        {
            if(gameObject.transform.parent == null)
            {
                Debug.Log("j'ai touché " + hit.collider.name);
                gameObject.transform.parent = hit.transform;            
            }
        }

        else if (gameObject.transform.parent != null)
        {
            Debug.Log("j'ai plus rien touché");
            if (gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity.x < 0)
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(100, 0));
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-100, 0));
            }
          
            gameObject.transform.parent = null;         
        }
    }
}
