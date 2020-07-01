
using System.Collections;
using UnityEngine;

public class MovingPlateforme : MonoBehaviour
{

    Actions myState;
    public GameObject pointA;
    public GameObject pointB;
    public Vector2 point1;
    public Vector2 point2;

    public Rigidbody2D body;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        point1 = pointA.transform.position;
        point2 = pointB.transform.position;
        gameObject.transform.position = new Vector2(Random.Range(point1.x, point2.x), Random.Range(point1.y, point2.y));

        myState = Actions.patroltoLeft;
        body = gameObject.GetComponent<Rigidbody2D>();
    }
    public enum Actions
    {
        patroltoLeft,
        patroltoRight,
        wait
  
    }
    // Update is called once per frame
    void Update()
    {
       
        switch (myState)
        {
            case Actions.patroltoLeft:
                // Debug.Log("Patrol ongoing !");              
                PatroltoLeft();
                break;
            case Actions.patroltoRight:
                // Debug.Log("Patrol ongoing !");              
                PatroltoRight();
                break;
            case Actions.wait:
                // Debug.Log("Patrol ongoing !");              
                Wait();
                break;
        }
    }

    void Wait()
    {

    }
    void PatroltoLeft()
    {
        body.velocity = new Vector2(speed * -1, body.velocity.y);
        if (gameObject.transform.position.x <= point1.x)
        {
            myState = Actions.wait;
            StartCoroutine(pause(Actions.patroltoRight));
        }
    }
    void PatroltoRight()
    {
        body.velocity = new Vector2(speed, body.velocity.y);
        if (gameObject.transform.position.x >= point2.x)
        {
            myState = Actions.wait;
            StartCoroutine(pause(Actions.patroltoLeft));
        }
    }
       

    IEnumerator pause(Actions nextodo)
    {
        yield return new WaitForSeconds(2f);
        myState = nextodo;
    }
}
