using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour {
    private bool used = false;
    private bool attached = false;

    public float lampRadius;
    private SpriteRenderer render;
    private LineRenderer lineRender;
    private GameObject player;
    public float springConstant = 2000;

    public Sprite lampOn;
    public Sprite lampOff;

    public float getLampRadius()
    {
        return lampRadius * transform.localScale.x;
    }

    public bool getUsed()
    {
         return used;
    }

    public void use()
    {
        this.used = true;
        this.attached = true;
    }

    public void detatch()
    {
        this.attached = false;
    }

    public void reset()
    {
        //make sure the renderer is found
        render = GetComponent<SpriteRenderer>();

        this.used = false;
        transform.Find("bounding box").GetComponent<LampRange>().reset();

        render.sprite = lampOff;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
            render.sprite = lampOn;
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!used)
        {
            render.sprite = lampOff;
        }
    }

    // Use this for initialization
    void Start () {
        CircleCollider2D coll = GetComponent<CircleCollider2D>();
        coll.radius = lampRadius;

        lineRender = GetComponent<LineRenderer>();

        render = GetComponent<SpriteRenderer>();
        render.sprite = lampOff;

        player = GameObject.FindGameObjectWithTag("Player");
    }
	
	// Update is called once per frame
	void Update () {
        if (used)
        {
            transform.Find("bounding box").localScale = new Vector3(0, 0, 0);

            if (attached)
            {
                lineRender.SetPosition(0, new Vector3(player.transform.position.x, player.transform.position.y, -2));
                lineRender.SetPosition(1, new Vector3(transform.position.x, transform.position.y, -2));
            }
            else
            {
                lineRender.SetPosition(0, new Vector3(0, 0, -2));
                lineRender.SetPosition(1, new Vector3(0, 0, -2));
            }
        }
	}
}
