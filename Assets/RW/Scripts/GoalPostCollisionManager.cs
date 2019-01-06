﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalPostCollisionManager : MonoBehaviour {

    public GameObject BallCollision;
    public GameObject WinnerUI;
     BallCollisionChecker ballCollisionChecker;

	// Use this for initialization
	void Start () {
        WinnerUI.SetActive(false);
        ballCollisionChecker = BallCollision.GetComponent<BallCollisionChecker>();

    }
	
	

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Ball")
        {
            string LastTouchPlayer = ballCollisionChecker.LastTouchPlayer;
            Debug.Log("Goal by " + LastTouchPlayer);

            WinnerUI.SetActive(true);
            WinnerUI.transform.GetChild(0).GetComponent<Text>().text = LastTouchPlayer + " Scored!";

        }
    }
}