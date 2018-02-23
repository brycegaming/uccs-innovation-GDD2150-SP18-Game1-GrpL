using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour {
    private Vector3 direction;
    public float springStrength;
    private Rigidbody2D playerRigid;

	// Use this for initialization
	void Start () {
        Vector3 eulerAngles = transform.rotation.eulerAngles;

        //find direction vector from euler angles
        float yaw, pitch, roll;
        yaw = Mathf.Deg2Rad * eulerAngles.x;
        pitch = Mathf.Deg2Rad * eulerAngles.y;
        roll = Mathf.Deg2Rad * eulerAngles.z;

        ////no need for a z axis
        ////direction.z = ((-Mathf.Cos(yaw)) * Mathf.Sin(pitch) * 
        ////                 Mathf.Sin(roll)) - (Mathf.Sin(yaw) * 
        ////                 Mathf.Cos(roll));

        direction.y = ((-Mathf.Sin(yaw)) * Mathf.Sin(pitch) *
                         Mathf.Sin(roll)) + (Mathf.Cos(yaw) *
                         Mathf.Cos(roll));

        direction.x = -Mathf.Cos(pitch)*Mathf.Sin(roll);

        playerRigid = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
    }

    /**
     * so it only adds the force once
     * */
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag =="Player")
        {
            //add force in the direction
            playerRigid.AddForce(direction * springStrength);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
