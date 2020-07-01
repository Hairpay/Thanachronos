using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomMover : MonoBehaviour
{
    public bool activate = false;
    public List<Vector2> PhantomPos = new List<Vector2>();
    public List<int> PhantomState = new List<int>();
    public int comptePos;

    public int time2Red = 200;

    public int rouge;
    private Color red;
    private Color baseColor;

    private Animator animator;
    public float t;
    public float launchDelay;
    private EventLauncher eventLauncher;

    // Start is called before the first frame update
    void Start()
    {
        eventLauncher = GameObject.Find("voidBarrier").GetComponent<EventLauncher>();
        DontDestroyOnLoad(this.gameObject);
        animator = gameObject.GetComponent<Animator>();
        baseColor = gameObject.GetComponent<SpriteRenderer>().color;
        baseColor = new Color(baseColor.r, baseColor.g, baseColor.b);
        red = new Color(255, 0, 0);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;      
    }

    // Update is called once per frame
    void Update()
    {
        if (activate == true)
        {
            if (gameObject.GetComponent<SpriteRenderer>().enabled == false)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                if (rouge > 0)
                {
                    gameObject.GetComponent<SpriteRenderer>().color = red;
                }
                t = 0;
            }
            if (eventLauncher.GetComponent<TimeWarper>().warping == false)
            {
                Phantomove();
                Prescience();
            }
            else
            {
                Phantomove();
                Prescience();
                Phantomove();
                Prescience();
                Phantomove();
                Prescience();

            }
                  
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
    void Prescience()
    {
        if (comptePos + time2Red < PhantomPos.Count)
        {            
            if (PhantomState[comptePos + time2Red] == 2 || PhantomState[comptePos + time2Red] == 12)
            {                
                rouge++;
                t = 0;
            }
        }

        if (rouge > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, red, t);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, baseColor, t);
        }
        t += 0.1f * Time.deltaTime;

    }

    void Phantomove()
    {
        if (comptePos < PhantomPos.Count)
        {
            if (PhantomState[comptePos] > 9)
            {                
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(-1, 1);
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
                gameObject.GetComponent<BoxCollider2D>().offset = new Vector2(1,1);
            }

            transform.position = PhantomPos[comptePos];
            if (PhantomState[comptePos] == 2 || PhantomState[comptePos] == 12)
            {
                animator.Play("Atacc");
                
                StartCoroutine(derouge());

            }
            else if (PhantomState[comptePos] == 0 || PhantomState[comptePos] == 10)
            {
                animator.SetBool("isWalking", false);
            }
            else if (PhantomState[comptePos] == 1 || PhantomState[comptePos] == 11)
            {
                animator.SetBool("isWalking", true);
            }

        }
        else
        {
            animator.Play("Death");
            StartCoroutine(ded());      
        }

        comptePos++;

    }
    public void launch()
    {
        StartCoroutine(launchRoutine());     
    }
    IEnumerator launchRoutine()
    {       
        yield return new WaitForSeconds(launchDelay);
        gameObject.GetComponent<dedzone>().once = false;
        activate = true;
        comptePos = 0;
        animator.Play("idle");
        StopAllCoroutines();
        rouge = 0;

        for (int i = 0; i < time2Red; i++)
        {
            if (i < PhantomState.Count && PhantomState[i] == 2 || i < PhantomState.Count && PhantomState[i] == 12)
            {
                rouge++;
            }
        }
    }
    IEnumerator derouge()
    {
        yield return new WaitForSeconds(0.5f);
        rouge--;
        t = 0;
    }
    IEnumerator ded()
    {
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        comptePos = 0;
        activate = false;
    }
}
