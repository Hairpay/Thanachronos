using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class resetCristal : MonoBehaviour
{
    public EventLauncher eventLauncher;
    // Start is called before the first frame pdate
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Phantom")
        {
            deactivate();
        }
    }
    void deactivate()
    {
        eventLauncher.destroyPhantoms();
        gameObject.SetActive(false);
    }
}
