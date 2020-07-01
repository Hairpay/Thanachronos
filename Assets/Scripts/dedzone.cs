using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dedzone : MonoBehaviour
{
    // Start is called before the first frame update
    public bool once;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnTriggerEnter2D(Collider2D other)
    {     
        if (other.tag == "Player" && once == false)
        {
            once = true;
            other.GetComponent<PhantomGenerator>().ded();
            Debug.Log(gameObject.name + " a tué le joueur");
        }
        else
        {
            Debug.Log(other.tag);
        }
    }
}
