using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ShieldMobAI : MonoBehaviour
{

    Actions myState;
    public Rigidbody2D body;

    public int lasthit;
    public int lasthitSpot;
    private float bumpRecoverTime = 0.7f;

    public Light redLight;
    private Color red;
    private Color baseColor;
    public GameObject weakbox;
    public GameObject dedbox;
    public float lightforce;
    public ParticleSystem redVoid;
    public ParticleSystem sparks;
    private Animator animator;

    public float t;

    // Start is called before the first frame update
    void Start()
    {
        myState = Actions.wait;
        body = gameObject.GetComponent<Rigidbody2D>();

        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(baseColor.r, baseColor.g, baseColor.b);
        red = new Color(255, 100, 100);

        animator = gameObject.GetComponent<Animator>();
        lightforce = 3;
        redLight.intensity = lightforce;
        bumpRecoverTime = 0.7f;
        redVoid.Stop();
        sparks.Stop();
    }
    public enum Actions
    {       
        bumped,
        weakened,
        damaged,
        riposte,      
        wait
    }
    // Update is called once per frame
    void Update()
    {
        if (lasthit < gameObject.GetComponent<whenHit>().hit)
        {
            lasthit = gameObject.GetComponent<whenHit>().hit;

            if (lightforce < 20)
            {
                myState = Actions.bumped;
                gameObject.GetComponent<whenHit>().locked = true;
                StopAllCoroutines();
                StartCoroutine(iframes());
            }
            else
            {
                StopAllCoroutines();
                myState = Actions.riposte;
            }
           
        }
        if (lasthitSpot < weakbox.GetComponent<whenHit>().hit)
        {
            lasthitSpot = weakbox.GetComponent<whenHit>().hit;
            myState = Actions.damaged;
            StopAllCoroutines();
        }

        switch (myState)
        {               
            case Actions.bumped:
                // Debug.Log("shoot ongoing !");             
                animator.Play("ShieldMobBlock");
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
        }
    }
    void Wait()
    {
       
    }
    void Riposte()
    {
        myState = Actions.wait;
        weakbox.GetComponent<whenHit>().locked = true;
        gameObject.GetComponent<whenHit>().locked = true;
        dedbox.GetComponent<dedzone>().once = false;
        redVoid.Play();
        animator.Play("ShieldMobChargePower");
        StartCoroutine(Ripostattack());
        StartCoroutine(redvoibac());
    }
    void Weakened()
    {
        if (t < 1)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, baseColor, t);
            redLight.intensity = Mathf.Lerp(lightforce, 3, t);
            t += bumpRecoverTime * Time.deltaTime;
        }
        else
        {
            myState = Actions.wait;
            lightforce = 3;      
            dedbox.GetComponent<dedzone>().once = false;
            sparks.Stop();
        }
    }

    void Bumped()
    {
        gameObject.GetComponent<SpriteRenderer>().color = red;
        lightforce = lightforce + 10;
        redLight.intensity = lightforce;
        myState = Actions.weakened;
        dedbox.GetComponent<dedzone>().once = true;

        body.velocity = new Vector2(2, 0);

        t = 0;         
    }
    void Damaged()
    {
        Debug.Log("ilémaure le mob au bouclier");
        animator.Play("ShieldMobDed");
        myState = Actions.wait;

        weakbox.GetComponent<whenHit>().locked = true;
        gameObject.GetComponent<whenHit>().locked = true;
        dedbox.GetComponent<dedzone>().once = true;

        body.velocity = new Vector2(0,0);
        body.isKinematic = true;

        StopAllCoroutines();
        StartCoroutine(Death());

    }

    IEnumerator iframes()
    {       
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<whenHit>().locked = false;

    }
    IEnumerator Ripostattack()
    {
        yield return new WaitForSeconds(1f);
        body.velocity = new Vector2(-15 , 0);
        myState = Actions.weakened;
        weakbox.GetComponent<whenHit>().locked = false;
        gameObject.GetComponent<whenHit>().locked = false;
        sparks.Play();    
        StopAllCoroutines();
        StartCoroutine(iframes());
    }
    IEnumerator redvoibac()
    {
        yield return new WaitForSeconds(0.5f);
        redVoid.Stop();
    }
    IEnumerator Death()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }

}
