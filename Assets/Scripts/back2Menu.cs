using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class back2Menu : MonoBehaviour
{
    public bool once;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Attack") && once == false || Input.GetButtonDown("Jump") && once == false || Input.GetButtonDown("TimeWarp") && once == false)
        {
            once = true;
            gameObject.GetComponent<Animator>().Play("Fondu2");
            StartCoroutine(LoadMenu());
        }
    }
    IEnumerator LoadMenu()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("MainMenu");
    }
}
