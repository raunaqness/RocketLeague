using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallCollisionChecker : MonoBehaviour {

    public string LastTouchPlayer = "";
    public Text LastTouch;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            LastTouchPlayer = other.name;
            LastTouch.text = other.name;
        }

    }


}
