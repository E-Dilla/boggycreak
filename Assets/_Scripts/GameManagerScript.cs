using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public bool isDayTime;
    public bool isNightTime;

    public float clockTime;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        clockTime += Time.deltaTime;
        
        if (clockTime >= 360)
        {
            clockTime = 0;
        }

        if (clockTime <= 180f)
        {
            isDayTime = true;
        }
        else
        {
            isDayTime = false;
        }

	}
}
