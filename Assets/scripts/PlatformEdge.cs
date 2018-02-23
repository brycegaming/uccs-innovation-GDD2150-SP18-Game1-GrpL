using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformEdge : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyAI>().turn();
        }
    }
}
