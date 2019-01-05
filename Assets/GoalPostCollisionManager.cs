using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPostCollisionManager : MonoBehaviour {

    public GameObject BallCollision;
     BallCollisionChecker ballCollisionChecker;

	// Use this for initialization
	void Start () {

        ballCollisionChecker = BallCollision.GetComponent<BallCollisionChecker>();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Ball")
        {
            string LastTouchPlayer = ballCollisionChecker.LastTouchPlayer;
            Debug.Log("Goal by " + LastTouchPlayer);

        }
    }
}
