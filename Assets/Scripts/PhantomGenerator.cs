using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhantomGenerator : MonoBehaviour
{
    private List<Vector2> PhantomPos = new List<Vector2>();
    private List<int> PhantomState = new List<int>();
    public bool activate = false;
    public GameObject Phantom;
    private EventLauncher eventLauncher;
    

    // Start is called before the first frame update
    void Start()
    {      
        eventLauncher = GameObject.Find("voidBarrier").GetComponent<EventLauncher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (activate == true)
        {
            if (eventLauncher.GetComponent<TimeWarper>().warping == false)
            {
                AddStateNPos();
            }
            else
            {
                AddStateNPos();
                AddStateNPos();
                AddStateNPos();
            }            
        }
    }

    public void AddStateNPos()
    {
        PhantomPos.Add(transform.position);
        PhantomState.Add(gameObject.GetComponent<PlayerPlatformerController>().playerState);
    }
    public void launch()
    {
        activate = true;
    }
    public void ded()
    {
        if (activate == true)
        {
            Debug.Log("yésouimaure");
            activate = false;
            GameObject NouvoPhantom = Instantiate(Phantom);
            NouvoPhantom.transform.position = new Vector2(0, 50);
            NouvoPhantom.GetComponent<PhantomMover>().launchDelay = Random.Range(0.3f, 1f);
            for (int i = 0; i < PhantomPos.Count; i++)
            {
                NouvoPhantom.GetComponent<PhantomMover>().PhantomPos.Add(PhantomPos[i]);
                NouvoPhantom.GetComponent<PhantomMover>().PhantomState.Add(PhantomState[i]);
            }

            eventLauncher.GetComponent<EventLauncher>().Phantoms.Add(NouvoPhantom);
            eventLauncher.Reset();

            PhantomPos.Clear();
            PhantomState.Clear();

            gameObject.GetComponent<Animator>().Play("Death");
            gameObject.GetComponent<PlayerPlatformerController>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;

        }
    }    
}
