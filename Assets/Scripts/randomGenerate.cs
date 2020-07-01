using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomGenerate : MonoBehaviour
{
    public GameObject[] objects;
    // Start is called before the first frame update
    private void Awake()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
        objects[Random.Range(0, objects.Length)].SetActive(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
