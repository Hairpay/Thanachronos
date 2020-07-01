using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class basicAI : MonoBehaviour
{

    Actions myState;
    public GameObject pointA;
    public GameObject pointB;
    public Vector2 point1;
    public Vector2 point2;
    public float t;

    public Rigidbody2D body;
    public int speed;

    public int lasthit;
    public GameObject player;
    public float bumpforce = 2.5f;
    public int hitpoint = 3;

    public GameObject lightHead;
    private Color red;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        point1 = pointA.transform.position;
        point2 = pointB.transform.position;      
        gameObject.transform.position = new Vector2(Random.Range(point1.x,point2.x), Random.Range(point1.y, point2.y));

        player = GameObject.Find("Char");
        myState = Actions.patroltoLeft;
        body = gameObject.GetComponent<Rigidbody2D>();

        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(baseColor.r, baseColor.g, baseColor.b);
        red = new Color(255, 0, 0);
    }
    public enum Actions
    {
        patroltoLeft,
        patroltoRight,
        bumped,
        stop,
        wait
    }
    // Update is called once per frame
    void Update()
    {
        if (lasthit < gameObject.GetComponent<whenHit>().hit)
        {
            lasthit = gameObject.GetComponent<whenHit>().hit;
            myState = Actions.bumped;
            gameObject.GetComponent<whenHit>().locked = true;
            gameObject.GetComponentInChildren<dedzone>().once = true;          
            StopAllCoroutines();
            StartCoroutine(iframes());
            StartCoroutine(colorframes());
        }

        switch (myState)
        {
            case Actions.patroltoLeft:
                // Debug.Log("Patrol ongoing !");              
                PatroltoLeft();
                break;
            case Actions.patroltoRight:
                // Debug.Log("Patrol ongoing !");              
                PatroltoRight();
                break;
            case Actions.bumped:
                // Debug.Log("shoot ongoing !");
                Bumped();
                break;
            case Actions.stop:
                // Debug.Log("shoot ongoing !");
                Stop();
                break;
            case Actions.wait:
                // Debug.Log("wait ongoing !");
                Wait();            
                break;
        }
    }
    void Wait()
    {      
        StartCoroutine(pause());
        myState = Actions.stop;
    }
    void Stop()
    {
        
    }
    void PatroltoLeft()
    {
        body.velocity = new Vector2(speed * -1, body.velocity.y);
        if (gameObject.transform.position.x <= point1.x)
        {
            myState = Actions.wait;
        }
    }
    void PatroltoRight()
    {
        body.velocity = new Vector2(speed, body.velocity.y);
        if (gameObject.transform.position.x >= point2.x)
        {
            myState = Actions.wait;
        }
    }
    void Bumped()
    {
        myState = Actions.stop;

        if (lasthit < hitpoint)
        {          
            gameObject.GetComponent<SpriteRenderer>().color = red;

            if (player.transform.position.x <= gameObject.transform.position.x)
            {
                body.velocity = new Vector2(bumpforce, bumpforce);
            }
            else
            {
                body.velocity = new Vector2(-bumpforce, bumpforce);
            }
        }
        else
        {
            Debug.Log("aie, mob mort");
            // Destroy(gameObject);
            gameObject.GetComponent<Animator>().Play("ded");
            body.velocity = new Vector2(0, 0);
            body.isKinematic = true;
            StartCoroutine(death());

        }
        
    }

    IEnumerator pause()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<SpriteRenderer>().flipX = !gameObject.GetComponent<SpriteRenderer>().flipX;
        lightHead.transform.localPosition = new Vector3(-lightHead.transform.localPosition.x, lightHead.transform.localPosition.y, lightHead.transform.localPosition.z);

        if (gameObject.GetComponent<SpriteRenderer>().flipX == true)
        {
            myState = Actions.patroltoRight;
        }
        else
        {
            myState = Actions.patroltoLeft;
        }      
    }
    IEnumerator iframes()
    {
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<whenHit>().locked = false;
        gameObject.GetComponentInChildren<dedzone>().once = false;
        myState = Actions.wait;
    }
    IEnumerator colorframes()
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;
    }
    IEnumerator death()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
