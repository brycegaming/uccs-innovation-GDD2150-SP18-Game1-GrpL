using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class InvisiblePlatform : MonoBehaviour
{
    private float startTransparency;
    private float endTransparency ;
    public float oscilationTime = .5f;
    private float currentTime;

    private const float MAX_TRANSPARENCY = 0;
    private const float MIN_TRANSPARENCY = 1;

    private SpriteRenderer renderer;
    
	// Use this for initialization
	void Start ()
	{
	    startTransparency = MIN_TRANSPARENCY;
	    endTransparency = MAX_TRANSPARENCY;
	    currentTime = 0;
	}

    private void Awake()
    {
        this.renderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
	void Update ()
	{
	    currentTime += Time.deltaTime;
	    
	    if (currentTime >= oscilationTime)
	    {
	        currentTime = 0;

	        if (endTransparency == MIN_TRANSPARENCY)
	        {
	            endTransparency = MAX_TRANSPARENCY;
	            startTransparency = MIN_TRANSPARENCY;
	        }
	        else
	        {
	            endTransparency = MIN_TRANSPARENCY;
	            startTransparency = MAX_TRANSPARENCY;
	        }
	    }

	    if (endTransparency == MAX_TRANSPARENCY)
	    {
	        startTransparency = Mathf.Lerp(MIN_TRANSPARENCY, MAX_TRANSPARENCY, (currentTime / oscilationTime));
	    }
	    else
	    {
	        startTransparency = Mathf.Lerp(MAX_TRANSPARENCY, MIN_TRANSPARENCY, (currentTime / oscilationTime));
	    }
	   
	    renderer.color = new Vector4(renderer.color.r, renderer.color.g, renderer.color.b, startTransparency);
	}
}
