using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWarper : MonoBehaviour
{
    private float timeScaleMax = 3;
    private float maxbaseSpeed;
    private float maxbasejump;

    public GameObject player;
    public bool warping;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Char");
        warping = false;
        Time.timeScale = 1;
        maxbaseSpeed = player.GetComponent<PlayerPlatformerController>().maxSpeed;
        maxbasejump = player.GetComponent<PlayerPlatformerController>().jumpTakeOffSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            player = GameObject.Find("Char");
        }

        if (Input.GetButtonDown("TimeWarp") )
        {
            warping = true;
            Time.timeScale = timeScaleMax;
            player.GetComponent<PlayerPlatformerController>().maxSpeed = 0;
            player.GetComponent<PlayerPlatformerController>().jumpTakeOffSpeed = 0;
        }
        else if (Input.GetButtonUp("TimeWarp"))
        {
            warping = false;
            Time.timeScale = 1;
            player.GetComponent<PlayerPlatformerController>().maxSpeed = maxbaseSpeed;
            player.GetComponent<PlayerPlatformerController>().jumpTakeOffSpeed = maxbasejump;
        }
    }
}

