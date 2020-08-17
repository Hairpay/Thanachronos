using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whenEntered : MonoBehaviour
{
    public GameObject plate;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Body")
        {
            deactivate();
        }   
    }

    void deactivate()
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        gameObject.GetComponent<Animator>().Play("go");
        plate.GetComponent<whenEnter>().numberOfActivations = 100;
        GameObject player = GameObject.Find("Char");
        player.GetComponent<PlayerPlatformerController>().inoffensive = false;
        player.GetComponent<PlayerPlatformerController>().Axevisuel.SetActive(true);
    }
}
