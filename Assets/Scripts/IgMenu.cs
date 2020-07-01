using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IgMenu : MonoBehaviour
{
    public GameObject Menu;
    public GameObject[] menuButtons;
    public GameObject ArrowBox;
    public GameObject fondu;

    public int currentPos;
    public float direction;
    public bool CD;
    private bool once;

    public bool menuActive;

    // Start is called before the first frame update
    void Start()
    {     
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        checkCursor();
       
        if (Input.GetButtonDown("Pause") && once == false)      
        {
            menuActive = !menuActive;
            Menu.SetActive(menuActive);
        }
        

        if (Input.GetButtonDown("Attack") && once == false && menuActive == true
            || Input.GetButtonDown("Jump") && once == false && menuActive == true
            || Input.GetButtonDown("TimeWarp") && once == false && menuActive == true)
        {
            if (currentPos != 0)
            {
                once = true;
                fondu.GetComponent<Animator>().Play("Fondu2");
                StartCoroutine(LoadNextScene());
            }
            else
            {
                menuActive = false;
                Menu.SetActive(menuActive);
            }
           
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

        ArrowBox.transform.localPosition = new Vector3(-9, menuButtons[currentPos].transform.position.y, 0);
    }

    IEnumerator returnCD()
    {
        yield return new WaitForSeconds(0.3f);
        CD = false;
    }
    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(1.5f);
        Application.Quit();
    }
}
