using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BossAI : MonoBehaviour
{

    Actions myState;
    public Rigidbody2D body;
    public GameObject player;
    public GameObject eventLauncher;

    public int lasthit;
    public int lasthitSpot;
    public int hitpoints = 3;
    public float bumpforce;
    private float bumpRecoverTime;

    public Light redLight;
    private Color red;
    private Color baseColor;
    public GameObject weakbox;
    public GameObject dedbox;
    public float lightforce;
    public int pushSide;
    public ParticleSystem redVoid;
    public ParticleSystem sparks;
    public ParticleSystem wardust;
    public GameObject centerofmap;

    public float t;

    // Start is called before the first frame update
    void Start()
    {       
        player = GameObject.Find("Char");
        eventLauncher = GameObject.Find("voidBarrier");
        myState = Actions.inactive;
        body = gameObject.GetComponent<Rigidbody2D>();
        weakbox.GetComponent<whenHit>().locked = true;

        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(baseColor.r, baseColor.g, baseColor.b);
        red = new Color(255, 100, 100);

        lightforce = 0;
        redLight.intensity = lightforce;
        bumpRecoverTime = 0.7f;
        redVoid.Stop();
        wardust.Stop();
        sparks.Stop();
    }
    public enum Actions
    {
        move,
        inactive,
        bumped,
        weakened,
        damaged,
        riposte,
        interphase,
        warcry,
        wait
    }
    // Update is called once per frame
    void Update()
    {
        if (lasthit < gameObject.GetComponent<whenHit>().hit)
        {
            pushSide = gameObject.GetComponent<whenHit>().side;
            lasthit = gameObject.GetComponent<whenHit>().hit;

            if (lightforce < 30)
            {
                myState = Actions.bumped;
            }
            else
            {
                myState = Actions.riposte;                  
            }
            gameObject.GetComponent<whenHit>().locked = true;
            StopAllCoroutines();
            StartCoroutine(iframes());
        }
        if (lasthitSpot < weakbox.GetComponent<whenHit>().hit)
        {         
            lasthitSpot = weakbox.GetComponent<whenHit>().hit;
            myState = Actions.damaged;
            t = 0;
            lightforce = 0;
            redLight.intensity = lightforce;
            weakbox.GetComponent<whenHit>().locked = true;
            StopAllCoroutines();
        }

        switch (myState)
        {
            case Actions.move:
                // Debug.Log("Patrol ongoing !");              
                Move();
                break;
            case Actions.inactive:
                // Debug.Log("Patrol ongoing !");              
                Inactive();
                break;
            case Actions.bumped:
                // Debug.Log("shoot ongoing !");
                Bumped();
                break;
            case Actions.weakened:
                // Debug.Log("shoot ongoing !");
                Weakened();
                break;
            case Actions.damaged:
                // Debug.Log("shoot ongoing !");
                Damaged();
                break;
            case Actions.wait:
                // Debug.Log("wait ongoing !");
                Wait();
                break;
            case Actions.riposte:
                // Debug.Log("Patrol ongoing !");              
                Riposte();
                break;
            case Actions.interphase:
                // Debug.Log("Patrol ongoing !");              
                Interphase();
                break;
            case Actions.warcry:
                // Debug.Log("Patrol ongoing !");              
                Warcry();
                break;
        }
    }
    void Warcry()
    {
        myState = Actions.wait;
        wardust.Play();
        gameObject.GetComponent<whenHit>().locked = true;
        StartCoroutine(wait(1.5f));
    }
    void Interphase()
    {
        body.velocity = new Vector2(0, 0);
        body.isKinematic = true;
        dedbox.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        myState = Actions.wait;
        StartCoroutine(interphasage());
    }
    void Inactive()
    {
        if (Mathf.Abs(gameObject.transform.position.x - player.transform.position.x) < 12f)
        {
            Debug.Log("detected");
            myState = Actions.warcry;
          //  player.GetComponent<CameraBehaviour>().fixCamera(centerofmap.transform.position.x);
        }
    }
    void Wait()
    {
       
    }
    void Riposte()
    {
        weakbox.GetComponent<whenHit>().locked = true;
        gameObject.GetComponent<whenHit>().locked = true;
        dedbox.GetComponent<dedzone>().once = false;
        redVoid.Play();
        bumpRecoverTime = 0.25f;
        StartCoroutine(Ripostattack());
        
    }
    void Weakened()
    {
        if (t < 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, baseColor, t);
            redLight.intensity = Mathf.Lerp(lightforce , 0 , t);          
            t += bumpRecoverTime * Time.deltaTime;
        }
        else
        {
            myState = Actions.move;
            lightforce = 0;
            weakbox.GetComponent<whenHit>().locked = true;
            dedbox.GetComponent<dedzone>().once = false;
        }
       
    }
    void Move()
    {
        if (player.transform.position.x < gameObject.transform.position.x)
        {
            body.velocity = new Vector2(-2, 0);
            flipX(false);
        }
        else
        {
            body.velocity = new Vector2(2, 0);
            flipX(true);
        }
        myState = Actions.wait;
        gameObject.GetComponent<whenHit>().locked = false;
        dedbox.GetComponent<dedzone>().once = false;
        redVoid.Stop();
        sparks.Stop();
        StopAllCoroutines();
        StartCoroutine(wait(0.5f));
    }   
    void Bumped()
    {
        gameObject.GetComponent<SpriteRenderer>().color = red;
        lightforce = lightforce + 10;
        bumpRecoverTime = 0.7f;
        redLight.intensity = lightforce;    
        myState = Actions.weakened;
        dedbox.GetComponent<dedzone>().once = true;
       
        t = 0;     
        body.velocity = new Vector2(bumpforce * pushSide, 0);   
        if (pushSide < 0)
        {
            flipX(true);
        }
        else
        {
            flipX(false);
        }
    }
    void Damaged()
    {
        if(lasthitSpot < hitpoints)
        {
            if (t < 1)
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(255,0,0), baseColor, t);
                t += 0.9f * Time.deltaTime;
            }
            else
            {
                myState = Actions.interphase;            
            }
        }
        else
        {
            Debug.Log("bossisded");
            gameObject.SetActive(false);
            centerofmap.GetComponent<BossCameraLock>().EndEncounter();
        }
    }
    void flipX(bool flip)
    {
        gameObject.GetComponent<SpriteRenderer>().flipX = flip;
        if (flip == false)
        {
            weakbox.transform.localPosition = new Vector2(0.74f, weakbox.transform.localPosition.y);
            dedbox.transform.localPosition = new Vector2(-0.5f, dedbox.transform.localPosition.y);
            redVoid.transform.localPosition = new Vector2(-0.25f, redVoid.transform.localPosition.y);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-0.5f, -0.65f);

        }
        else
        {
            weakbox.transform.localPosition = new Vector2(-0.74f, weakbox.transform.localPosition.y);
            dedbox.transform.localPosition = new Vector2(0.5f, dedbox.transform.localPosition.y);
            redVoid.transform.localPosition = new Vector2(0.25f, redVoid.transform.localPosition.y);
            gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(0.5f, -0.65f);
        }
    }
    IEnumerator iframes()
    {
        Debug.Log("bossIFrames");
        yield return new WaitForSeconds(0.5f);
        weakbox.GetComponent<whenHit>().locked = false;
        gameObject.GetComponent<whenHit>().locked = false;
        redVoid.Stop();
        sparks.Stop();
    }
    IEnumerator wait(float waitime)
    {
        yield return new WaitForSeconds(waitime);
        wardust.Stop();
        gameObject.GetComponent<whenHit>().locked = false;
        myState = Actions.move;
    }
    IEnumerator Ripostattack()
    {
        yield return new WaitForSeconds(1f);
        body.velocity = new Vector2(-15 * pushSide, 0);
        myState = Actions.weakened;
        sparks.Play();
        StopAllCoroutines();
        StartCoroutine(iframes());

    }
    IEnumerator interphasage()
    {
        yield return new WaitForSeconds(2f);

        if (eventLauncher.GetComponent<DontDestroyInfo>().sidespawn == 0)
        {      
            if (player.transform.position.x < centerofmap.transform.position.x)
            {
                gameObject.transform.position = new Vector2(centerofmap.transform.position.x + 4, gameObject.transform.position.y);
                flipX(false);
                eventLauncher.GetComponent<DontDestroyInfo>().sidespawn = 1;
            }
            else
            {
                gameObject.transform.position = new Vector2(centerofmap.transform.position.x - 4, gameObject.transform.position.y);
                flipX(true);
                eventLauncher.GetComponent<DontDestroyInfo>().sidespawn = -1;
            }
        }
        else
        {
           if (eventLauncher.GetComponent<DontDestroyInfo>().sidespawn == 1)
            {
                gameObject.transform.position = new Vector2(centerofmap.transform.position.x + 4, gameObject.transform.position.y);
                flipX(false);
            }
            else
            {
                gameObject.transform.position = new Vector2(centerofmap.transform.position.x - 4, gameObject.transform.position.y);
                flipX(true);
            }
        }
                      
        body.velocity = new Vector2(0, 0);
        body.isKinematic = false;
        dedbox.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        gameObject.GetComponent<whenHit>().locked = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        myState = Actions.warcry;
    }

}
