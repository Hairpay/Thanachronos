using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventLauncher : MonoBehaviour
{
    public Animator globalLightning;
    public GameObject player;
    private static GameObject launcherInstance;
    private GameObject fondu;
    public GameObject cristal;
    public GameObject bigVoid;
    private Vector2 bigVoidBasepos;
    public float speedVoid;
    public bool launched;

    public List<GameObject> Phantoms = new List<GameObject>();

    void Awake()
    {
        DontDestroyOnLoad(this);

        if (launcherInstance == null)
        {
            launcherInstance = this.gameObject;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void Start()
    {
        cristal.SetActive(false);
        bigVoidBasepos = bigVoid.transform.position;
    }

    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Char");         
        }   
        if (globalLightning == null)
        {
            globalLightning = gameObject.GetComponent<Animator>();
        }

        if (launched == true)
        {
            bigVoid.transform.position = new Vector2(bigVoid.transform.position.x + speedVoid * Time.deltaTime, 0);

        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {       
        if(other.tag == "Player" && player.transform.position.x > gameObject.transform.position.x)
        {
            SequenceLaunch();
            Debug.Log("yee");
            gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    void SequenceLaunch()
    {
        globalLightning.Play("go");
        launched = true;
        player.GetComponent<PhantomGenerator>().launch();
        for (int i = 0; i < Phantoms.Count; i++)
        {
            Phantoms[i].GetComponent<PhantomMover>().launch();
        }
    }
    public void Reset()
    {
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        globalLightning.Play("void");
        cristal.SetActive(true);
        launched = false;
        StopAllCoroutines();
        StartCoroutine(dedboi());
        fondu = GameObject.Find("Fondu");
        fondu.GetComponent<Animator>().Play("Fondu2");
        for (int i = 0; i < Phantoms.Count; i++)
        {
            Phantoms[i].GetComponent<PhantomMover>().activate = false;
            Phantoms[i].transform.position = new Vector2(0, 50);
        }
        if (Phantoms.Count > 255)
        {
            destroyPhantoms();
        }
    }
    public void destroyPhantoms()
    {
        for (int i = 0; i < Phantoms.Count; i++)
        {
            Destroy(Phantoms[i]);
        }
        Phantoms.Clear();
    }
    public void nextScene()
    {
        player.GetComponent<PhantomGenerator>().activate = false;
        fondu = GameObject.Find("Fondu");
        fondu.GetComponent<Animator>().Play("Fondu2");
        for (int i = 0; i < Phantoms.Count; i++)
        {
            Destroy(Phantoms[i]);
        }
        Destroy(gameObject);
    }
    IEnumerator dedboi()
    {
        yield return new WaitForSeconds(2f);
        gameObject.GetComponent<TimeWarper>().warping = false;
        Time.timeScale = 1;
        bigVoid.GetComponent<dedzone>().once = false;
        bigVoid.transform.position = bigVoidBasepos;
        int scene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }
}
