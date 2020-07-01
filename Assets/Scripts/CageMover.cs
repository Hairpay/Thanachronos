using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageMover : MonoBehaviour
{
    Actions myState;
    public float speed;
    public Color objectColor;
    public Color baseColor;
    public Color fadeColor;

    public Rigidbody2D body;
    public Vector3 pointA;
    public Vector3 pointB;
    public float t;
    public int side = 1;

    public enum Actions
    {
       Apparition,
       Disparition,
       Mouvement
    }

    // Start is called before the first frame update
    void Start()
    {
        body = gameObject.GetComponent<Rigidbody2D>();
        objectColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(objectColor.r,objectColor.g,objectColor.b,objectColor.a);
        fadeColor = new Color(0, 0, 0, 0);
        gameObject.GetComponent<SpriteRenderer>().color = fadeColor;

        if (gameObject.GetComponent<BoxCollider2D>() != null)
        {
            gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        myState = Actions.Apparition;

        if (pointB.x < transform.position.x)
        {
            side = -1;
        }
        else
        {
            side = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (myState)
        {
            case Actions.Apparition:            
                Apparition();
                break;
            case Actions.Disparition:         
                Disparition();
                break;
            case Actions.Mouvement:              
               Mouvement();
                break;
           
        }
    }
    public void Apparition()
    {
        if (t < 1)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, Mathf.Lerp(1, 0, t));
            objectColor = Color.Lerp(fadeColor, baseColor, t);
            gameObject.GetComponent<SpriteRenderer>().color = objectColor ;
            t += 0.5f * Time.deltaTime;
            if (t > 0.3f && gameObject.GetComponent<BoxCollider2D>() != null)
            {              
               gameObject.GetComponent<BoxCollider2D>().isTrigger = false;                
            }
        }        
        else
        {
            t = 0;
            myState = Actions.Mouvement;           
        }    
    }
    public void Disparition()
    {
        if (t < 1)
        {
            objectColor = Color.Lerp(baseColor, fadeColor, t);
            gameObject.GetComponent<SpriteRenderer>().color = objectColor;         
            t += 0.5f * Time.deltaTime;
            if (t > 0.75f && gameObject.GetComponent<BoxCollider2D>() != null)
            {            
                gameObject.GetComponent<BoxCollider2D>().isTrigger = true;               
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Mouvement()
    {
        if (Mathf.Abs(gameObject.transform.position.x) - Mathf.Abs(pointB.x) > 0.05f * side)
        {
            body.velocity = new Vector2(speed * side, body.velocity.y);          
        }
        else
        {
            body.velocity = new Vector2(0,0);
            myState = Actions.Disparition;
            gameObject.layer = 0;
        }
    }
}
