using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    //all elements that will be parallaxed
    public Transform[] parallaxElements;
    public float smoothing = 1;
    
    private Transform camera;
    private Vector3 prevCamPos;

    private void Awake()
    {
        camera = Camera.main.transform;
    }

    // Use this for initialization
	void Start ()
	{
	    prevCamPos = camera.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
	    for (int i = 0; i < parallaxElements.Length; i++)
	    {
	        float parallaxx = (prevCamPos.x - camera.position.x) * parallaxElements[i].position.z *  -1;
	        float parallaxy = (prevCamPos.y - camera.position.y) * parallaxElements[i].position.z * -1;
	        
	        //set the x position = currentPos + parralax
	        Vector3 newPosition = new Vector3(parallaxElements[i].position.x + parallaxx,
	            parallaxElements[i].position.y + parallaxy, parallaxElements[i].position.z);
	        
	        //lerp to new position
	        parallaxElements[i].position = Vector3.Lerp(parallaxElements[i].position, newPosition, smoothing);
	    }

	    prevCamPos = camera.position;
	}
}
