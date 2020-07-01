using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class whenEnter : MonoBehaviour
{
    public GameObject[] toActivate;
    public GameObject[] toDeactivate;

    public int numberOfActivations;
    public bool isActivated;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < toActivate.Length; i++)
        {
            toActivate[i].SetActive(false);
        }
        for (int i = 0; i < toDeactivate.Length; i++)
        {
            toDeactivate[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Body")
        {
            numberOfActivations++;

            if (numberOfActivations == 1 && isActivated == false)
            {              
                for (int i = 0; i < toActivate.Length; i++)
                {
                    toActivate[i].SetActive(true);
                }
                for (int i = 0; i < toDeactivate.Length; i++)
                {
                    toDeactivate[i].SetActive(false);
                }

                isActivated = true;
            }         
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" || other.tag == "Body")
        {
            numberOfActivations--;

            if (numberOfActivations == 0 && isActivated == true)
            {
                for (int i = 0; i < toActivate.Length; i++)
                {                   
                    toActivate[i].SetActive(false);
                }
                for (int i = 0; i < toDeactivate.Length; i++)
                {
                    toDeactivate[i].SetActive(true);
                }

                isActivated = false;
            }          
        }
    }
}
