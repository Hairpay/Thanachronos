using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralaxinator : MonoBehaviour
{
    public GameObject mainCharacter;
    public float intensity;
    private float basePosZ;

    // Start is called before the first frame update
    void Start()
    {
        basePosZ = gameObject.transform.position.z;
        gameObject.transform.position = new Vector3(0, 0, basePosZ) ;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector3(mainCharacter.transform.position.x * intensity,0, basePosZ);
    }
}
