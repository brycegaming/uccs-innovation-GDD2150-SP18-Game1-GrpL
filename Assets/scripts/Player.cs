using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions.Comparers;


public class Player : MonoBehaviour {
    private const float MAX_MOVEMENT_SPEED = 7.0f;
    private const float JUMP_VELOCITY = 9.0f;
    private const float ACCELERATION = 100;
    private Rigidbody2D body;
	AudioSource audio;

    private SpriteRenderer playerSprite;

    private  bool isJumping = false;

    private  GameObject[] lamps;

    //plays an animation for the player
    public SpriteAnimation animation;
    
    public Sprite[] rightAnimation;
    public Sprite[] leftAnimation;
    public float[] animationDurations;

	// Use this for initialization
	void Start () {
	    animation = new SpriteAnimation(rightAnimation, leftAnimation, animationDurations);
	    
        body = GetComponent<Rigidbody2D>();
        lamps = GameObject.FindGameObjectsWithTag("Lamp");
	    
	    //reset the level
	    for (int i = 0; i < lamps.Length; i++)
	    {
	        lamps[i].GetComponent<Lamp>().reset();
	    }
	    
	    GetComponent<LampMechanics>().reset();
	    
	    GameObject startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
	    transform.position = startPlatform.transform.position + new Vector3(0,startPlatform.transform.localScale.y/2+.1f,0);
		audio = GetComponent<AudioSource> ();
	}

    public void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
    }

    public void reset()
    {
        body.velocity = new Vector3(0,0,0);
        
        //reset the level
        for (int i = 0; i < lamps.Length; i++)
        {
            lamps[i].GetComponent<Lamp>().reset();
        }

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<EnemyAI>().reset();
        }

        GetComponent<LampMechanics>().reset();

        GameObject startPlatform = GameObject.FindGameObjectWithTag("StartPlatform");
        transform.position = startPlatform.transform.position + new Vector3(0,startPlatform.transform.localScale.y/2+.1f,0);
        
        GameObject.FindGameObjectWithTag("Timer").GetComponent<Timer>().reset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground"){
            reset();
        }

        if(collision.gameObject.tag == "Platform" || collision.gameObject.tag == "StartPlatform" || collision.gameObject.tag == "EndPlatform" )
            isJumping = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Platform" || collision.gameObject.tag == "StartPlatform" || collision.gameObject.tag == "EndPlatform") 
            isJumping = true;
    }

    // Update is called once per frame
    void Update ()
    {
        bool movement = false;
        
        if (Input.GetKey(KeyCode.A))
        {
            if (!isJumping)
            {
                body.velocity = new Vector3(-MAX_MOVEMENT_SPEED, body.velocity.y, 0);
                animation.turnLeft();
                movement = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!isJumping)
            {
                body.velocity = new Vector3(MAX_MOVEMENT_SPEED, body.velocity.y, 0);
                animation.turnRight();
                movement = true;
            }
        }
        else
        {
            if (!isJumping)
            {
                if (inRange(body.velocity.x, 1.0f, 1000000.0f))
                {
                    body.velocity = new Vector3((body.velocity.x) - Time.deltaTime * ACCELERATION, body.velocity.y, 0);
                }
                else if (inRange(body.velocity.x, -100000.0f, -1.0f))
                {
                    body.velocity = new Vector3((body.velocity.x) + Time.deltaTime * ACCELERATION, body.velocity.y, 0);
                }
                else
                {
                    body.velocity = new Vector3(0, body.velocity.y, 0);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(!isJumping)
                body.velocity = new Vector3(body.velocity.x, JUMP_VELOCITY, 0);
			audio.Play ();
            isJumping = true;
        }

        if (movement)
        {
            playerSprite.sprite = animation.progressAnimation();
        }
    }

    private bool inRange(float val, float low, float high)
    {
        return (val >= low) && (val <= high);
    }
    
    public void setJumping(bool jump)
    {
        this.isJumping = jump;
    }
    
    /**
     * placed here to avoid trigger confusions
     * I know there is a fix for that, but I am just going
     * to keep it simple
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" && other.GetType() == typeof(BoxCollider2D))
        {
            body.velocity = new Vector2(0, other.GetComponent<EnemyAI>().springVelocity);
        }
    }
}
