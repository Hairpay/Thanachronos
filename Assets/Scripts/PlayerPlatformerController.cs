using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPlatformerController : PhysicsObject
{

    public float maxSpeed = 7;
    public float jumpTakeOffSpeed = 7;
    private float edgeDelay = 1;

    public int playerState;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public bool isJumping;

    public GameObject hachebox;
    public Vector2 hachepos;
    public LayerMask layerMask;
    public bool atacCD;
    private bool atacForm;
    public bool inoffensive;

    // Use this for initialization
    void Awake()
    {      
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hachepos = hachebox.transform.localPosition;
        layerMask = LayerMask.GetMask("LevelHitbox","Environment");
    }

    protected override void ComputeVelocity()
    {
       
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal") * edgeDelay;
        if (Mathf.Abs(move.x) > 0.2f)
        {
            EdgeCheck(move.x);
        }  

        if(grounded == true)
        {
            if (isJumping == true)
            {
                isJumping = false;
                animator.SetBool("landing", true);
            }      
        }
        else
        {
            isJumping = true;
            animator.SetBool("landing", false);
        }

        if (Input.GetButtonDown("Jump") && canJump > 0)
        {
            velocity.y = jumpTakeOffSpeed;         
            animator.Play("Jump");         
        }
        else if (Input.GetButtonUp("Jump"))
        {
            if (velocity.y > 0)
            {
                velocity.y = velocity.y * 0.5f;
            }         
        }
        

        // set animation state

        if (Mathf.Abs(velocity.x) > 0.5f)
        {
            animator.SetBool("isWalking", true);
            playerState = 1;

        }
        else
        {
            animator.SetBool("isWalking", false);
            playerState = 0;
        }

        targetVelocity = move * maxSpeed;

        if (Input.GetButtonDown("Attack") && inoffensive == false && atacCD == false)
        {        
            atacCD = true;
            playerState = 2;
            StopCoroutine(atacReset());
            StartCoroutine(atacReset());

            if (atacForm == false)
            {
                animator.Play("Atacc");
                atacForm = true;
            }
            else
            {
                animator.Play("Atacc2");
                atacForm = false;
            }
             
        }
       


        //flipsprite  here

        bool flipSprite = (spriteRenderer.flipX ? (move.x > 0.01f) : (move.x < -0.01f));
        if (flipSprite)
        {
            spriteRenderer.flipX = !spriteRenderer.flipX;
        }

        if (spriteRenderer.flipX == true)
        {
            hachebox.transform.localPosition = new Vector2(-hachepos.x, hachepos.y);
            playerState = playerState + 10;
        }
        else
        {
            hachebox.transform.localPosition = new Vector2(hachepos.x, hachepos.y);
        }       
    }
    public void EdgeCheck(float moveX)
    {
        Vector2 vectorpos = new Vector2(transform.position.x + 0.05f, transform.position.y + 1.4f);
        RaycastHit2D hit = Physics2D.Raycast(vectorpos, Vector2.right * Mathf.Sign(moveX), 0.4f, layerMask);
        Debug.DrawRay(vectorpos, Vector2.right * Mathf.Sign(moveX) * 0.4f);

        Vector2 vectorposBot = new Vector2(transform.position.x + 0.05f, transform.position.y + 1f);
        RaycastHit2D hitBot = Physics2D.Raycast(vectorposBot, Vector2.right * Mathf.Sign(moveX), 0.4f, layerMask);
        Debug.DrawRay(vectorposBot, Vector2.right * Mathf.Sign(moveX) * 0.4f);

        if (hit.collider == null && hitBot.collider != null)
        {
            Debug.Log("EdgegrabTime");
            gameObject.transform.position = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + 1.2f);
            velocity.y = 0;
            edgeDelay = 0.2f;
            animator.Play("EdgeGrab");

            StopCoroutine(DelayReset());
            StartCoroutine(DelayReset());

        }    
    }
    IEnumerator DelayReset()
    {
        yield return new WaitForSeconds(0.4f);
        edgeDelay = 1f;
    }
    IEnumerator atacReset()
    {
        yield return new WaitForSeconds(0.4f);
        atacCD = false;
    }
}
