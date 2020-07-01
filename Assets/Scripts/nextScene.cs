using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class nextScene : MonoBehaviour
{
    public string scenext;
    private EventLauncher eventLauncher;
    // Start is called before the first frame update
    void Start()
    {
        eventLauncher = GameObject.Find("voidBarrier").GetComponent<EventLauncher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" )
        {
            eventLauncher.nextScene();         
            StartCoroutine(next());
        }
    }
    IEnumerator next()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(scenext);
    }
}
