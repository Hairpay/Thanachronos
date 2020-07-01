using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class MenuArrow : MonoBehaviour
{
    public GameObject[] menuButtons;
    public string[] ActionOfButton;
    public GameObject ArrowBox;
    public GameObject fondu;

    public int currentPos;
    public float direction;
    public bool CD;
    private bool once;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        checkCursor();
        if (Input.GetButtonDown("Attack") && once == false || Input.GetButtonDown("Jump") && once == false || Input.GetButtonDown("TimeWarp") && once == false)
        {
            once = true;
            fondu.GetComponent<Animator>().Play("Fondu2");
            StartCoroutine(LoadNextScene());
        }
    }
   
    private void checkCursor()
    {
        direction = Input.GetAxis("Vertical");

        if (direction > 0.05f && CD == false)
        {
            currentPos = currentPos - 1;
            CD = true;
            StopAllCoroutines();
            StartCoroutine(returnCD());
        }
        else if (direction < -0.05f && CD == false)
        {
            currentPos = currentPos + 1;
            CD = true;
            StopAllCoroutines();
            StartCoroutine(returnCD());
        }

        if (currentPos >= menuButtons.Length)
        {
            currentPos = 0;
        }
        else if (currentPos < 0)
        {
            currentPos = menuButtons.Length - 1;
        }

        ArrowBox.transform.position = new Vector3(-2, menuButtons[currentPos].transform.position.y, 0);
    }

    IEnumerator returnCD()
    {
        yield return new WaitForSeconds(0.3f);
        CD = false;
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(ActionOfButton[currentPos]);
    }
}
