using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSwitch : MonoBehaviour
{    
    public GameObject weakbox;
    public int lasthit;
    public int lasthit2;
    public float t1;
    public float t2;


    private Color green;
    private Color baseColor;

    public bool activate1;
    public bool activate2;

    public GameObject animateOnActivation;

    // Start is called before the first frame update
    void Start()
    {
        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(baseColor.r, baseColor.g, baseColor.b);
        green = new Color(0, 255, 0);
    }
    private void Update()
    {
        if (lasthit < gameObject.GetComponent<whenHit>().hit)
        {
            lasthit = gameObject.GetComponent<whenHit>().hit;
            gameObject.GetComponent<SpriteRenderer>().color = green;
            activate1 = true;
            StopAllCoroutines();
            StartCoroutine(Return1());
        }

        if (lasthit2 < weakbox.GetComponent<whenHit>().hit)
        {
            lasthit2 = weakbox.GetComponent<whenHit>().hit;
            weakbox.GetComponent<SpriteRenderer>().color = green;
            activate2 = true;
            StopAllCoroutines();
            StartCoroutine(Return2());
        }

        if (activate1 == true && activate2 == true)
        {
            StopAllCoroutines();
            gameObject.GetComponent<whenHit>().locked = true;
            weakbox.GetComponent<whenHit>().locked = true;
            animateOnActivation.GetComponent<Animator>().Play("go");
        }
    }
    IEnumerator Return1()
    {      
        yield return new WaitForSeconds(2f);
        activate1 = false;
        gameObject.GetComponent<SpriteRenderer>().color = baseColor;

    }
    IEnumerator Return2()
    {
        yield return new WaitForSeconds(2f);
        activate2 = false;
        weakbox.GetComponent<SpriteRenderer>().color = baseColor;

    }
}
