using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampRange : MonoBehaviour {

    public void reset()
    {
        transform.localScale = new Vector3(
            GetComponentInParent<Lamp>().lampRadius * 2,
            GetComponentInParent<Lamp>().lampRadius * 2,
            0
        );
    }

	// Use this for initialization
	void Start () {
        reset();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
