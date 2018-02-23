using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LampMechanics : MonoBehaviour {
    private GameObject player;
    private bool attached = false;

    private  GameObject selectedLamp;
    private Rigidbody2D body;

    public void reset()
    {
        attached = false;
    }

    enum Click
    {
        LEFT_CLICK,
        RIGHT_CLICK,
        MIDDLE_CLICK
    }

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        body = GetComponent<Rigidbody2D>();

	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown((int)Click.LEFT_CLICK))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

            int lampIndex = 0;

            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.gameObject.tag == "Lamp")
                {
                    lampIndex = i;
                    break;
                }
            }
            
            if (hits.Length > 0 && hits[lampIndex] != null)
            {
                if (hits[lampIndex].collider.gameObject.tag == "Lamp")
                {
                    
					//if they click on a lamp, this is where they throw a rope around it and use it like a bungee cord
                    Vector3 relativeLoc = hits[lampIndex].collider.gameObject.transform.position - player.transform.position;
                    float dist = Mathf.Sqrt((relativeLoc.x * relativeLoc.x) + (relativeLoc.y * relativeLoc.y) + 0);

                    selectedLamp = hits[lampIndex].collider.gameObject;

                    if (dist <= selectedLamp.GetComponent<Lamp>().getLampRadius() && !selectedLamp.GetComponent<Lamp>().getUsed())
                    {
                        selectedLamp.GetComponent<Lamp>().use();

                        //attach them to the lamp and sling
                        attached = true;
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp((int)Click.LEFT_CLICK))
        {
            attached = false;
            if(selectedLamp != null)
                selectedLamp.GetComponent<Lamp>().detatch();
        }

        if (attached)
        {
            Vector3 relativeLoc = selectedLamp.transform.position - player.transform.position;

            //apply f = -kx (x-dir)
            Vector3 springForce = new Vector3(0, 0, 0);
            springForce = relativeLoc * selectedLamp.GetComponent<Lamp>().springConstant;

            body.AddForce(springForce * Time.deltaTime);
        }
    }
}
