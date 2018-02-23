using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text text = null;
    private float time = 0;
    
    void Start ()
    {
        text = GetComponent<Text>();
        //text.transform.position
    }

    public void reset()
    {
        time = 0;
    }
	
    void Update ()
    {
        time += Time.deltaTime;
        text.text = "Time: " + Math.Floor(time);
    }
}
