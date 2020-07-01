using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCameraLock : MonoBehaviour
{
    private Animator animator;
    private bool once1;
    private bool once2;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && once1 == false)
        {
            once1 = true;
           animator.Play("OpenBossRoom");
           other.GetComponent<CameraBehaviour>().fixCamera(gameObject.transform.position.x);
        }
    }
    public void EndEncounter()
    {
        if (once2 == false)
        {
            once2 = true;
            animator.Play("EndingBossRoom");
        }  
    }      
}
