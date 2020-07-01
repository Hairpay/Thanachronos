using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caginator : MonoBehaviour
{
    public GameObject[] cagesPrefabs;
    public GameObject pointB;

    public float timeBetweenSpawn = 3f;


    // Start is called before the first frame update
    void Start()
    {
        Cagenerate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Cagenerate()
    {
        GameObject newCage = Instantiate(cagesPrefabs[Random.Range(0,cagesPrefabs.Length)]);
        newCage.transform.parent = gameObject.transform;
        newCage.transform.position = gameObject.transform.position;
        newCage.GetComponent<CageMover>().pointA = gameObject.transform.position;
        newCage.GetComponent<CageMover>().pointB = pointB.transform.position;

        StopAllCoroutines();
        StartCoroutine(Cooldown());
    }
    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        Cagenerate();
    }
}
