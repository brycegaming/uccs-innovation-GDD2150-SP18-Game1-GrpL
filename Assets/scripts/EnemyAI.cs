
using UnityEngine;

abstract class AnimationAction
{
    public abstract void performAction(GameObject animatedObject);
}
/**
 * implement this later, I dont have time
 * for now I will just stick with my current animation system
 */
class MovementAnimation
{
    private float[] animationDurrations;
    private AnimationAction[] actions;
    
    private float totalAnimationTime;
    private float localAnimationTime;
    private int animationProgress;
    private  GameObject gameObject;

    private bool inProgress = false;

    MovementAnimation(float[] animationDurrations, AnimationAction[] actions, GameObject gameObject)
    {
        this.animationDurrations = animationDurrations;
        this.actions = actions;
        inProgress = false;
        animationProgress = 0;
        this.gameObject = gameObject;
    }

    void animate()
    {
        totalAnimationTime += Time.deltaTime;
        localAnimationTime += Time.deltaTime;

        if (localAnimationTime >= animationDurrations[animationProgress])
        {
            localAnimationTime = 0;
            animationProgress++;
        }
        
        actions[animationProgress].performAction(gameObject);
    }
}

public class EnemyAI : MonoBehaviour
{
    public Sprite agroLeft;
    public Sprite agroRight;

    public Sprite idleLeft;
    public Sprite idleRight;

    /**
     * pointers to the left and right faces
     * to make it easier to switch between them
     */
    private Sprite faceLeft;
    private Sprite faceRight;
    
    private SpriteRenderer spriteRender;
    private PolygonCollider2D vision;
    private Rigidbody2D body;

    private bool agro = false;
    private bool playerInVision;

    private Vector3 startPos;

    private bool inAnimation = false;
    /**
     * first element is the wind up,
     * second is the time for the charge
     * third is the cooldown
     */
    public float[] chargeAnimationDurations = {.5f, .01f, 10.0f};
    private int animationProgress = 0;
    private float animationTime = 0;

    //when we turn the enemy around, we just multiply speed by -1
    public float speed = 2;
    public float chargeSpeedScale = 6;

    private Rigidbody2D playerRigidBody;
    private Player playerScript;
    
    public float springVelocity = 40;

    public bool getTurnedRight()
    {
        if (spriteRender.sprite == faceLeft)
        {
            return false;
        }

        return true;
    }

    private void turnRight()
    {
        spriteRender.sprite = faceRight;
        if (speed > 0)
        {
            speed *= -1;
        }
        
        vision.transform.rotation = Quaternion.Euler(new Vector3(0,180, 0));
    }

    private void turnLeft()
    {
        spriteRender.sprite = faceLeft;
        if (speed < 0)
        {
            speed *= -1;
        }
        
        vision.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void turn()
    {
        if (spriteRender.sprite == agroLeft || spriteRender.sprite == idleLeft)
        {
            spriteRender.sprite = faceRight;
            vision.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            spriteRender.sprite = faceLeft;
            vision.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        speed *= -1;
    }

    public bool getAgro()
    {
        return agro;
    }

    public void signalAgro()
    {
        this.agro = true;
    }

    public void signalCalm()
    {
        this.agro = false;
    }

    public void reset()
    {
        this.transform.position = startPos;

        turnRight();
        if (speed < 0)
            speed *= -1;
    }

    public bool isPlayerInVision()
    {
        return playerInVision;
    }

    public void setPlayerInVision(bool inVision)
    {
        playerInVision = inVision;
    }

    // Use this for initialization
    void Start()
    {
        //start facing right
        spriteRender.sprite = faceRight;
    }

    private void hitEnemy()
    {
        signalCalm();
        animationProgress = chargeAnimationDurations.Length - 1;
        animationTime = 0;
    }

    private void Awake()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        vision = transform.GetChild(0).GetComponent<PolygonCollider2D>();
        startPos = transform.position;
        
        playerRigidBody = Camera.main.transform.parent.GetComponent<Rigidbody2D>();
        playerScript = Camera.main.transform.parent.GetComponent<Player>();
        body = GetComponent<Rigidbody2D>();

        faceLeft = idleLeft;
        faceRight = idleRight;
    }

    public float getEnemySpeed()
    {
        if (inAnimation && animationProgress == 0 || !inAnimation)
        {
            return 0;
        }
        else
        {
            return speed * chargeSpeedScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float chargeSpeed = speed * chargeSpeedScale;

        if (!playerInVision)
        {
             body.velocity = new Vector3(speed, body.velocity.y, 0);
        }
        else
        {
            body.velocity = new Vector3(chargeSpeed, body.velocity.y, 0);
            inAnimation = true;
        }

        bool turnedRight = getTurnedRight();
        
        if (agro)
        {
            //spriteRender.color = agroColor;
            faceLeft = agroLeft;
            faceRight = agroRight;
        }
        else
        {
            //spriteRender.color = calmColor;
            faceLeft = idleLeft;
            faceRight = idleRight;
        }

        if (turnedRight)
        {
            spriteRender.sprite = faceRight;
        }
        else
        {
            spriteRender.sprite = faceLeft;
        }

        if (inAnimation)
        {
            animationTime += Time.deltaTime;
            
            if (animationTime >= chargeAnimationDurations[animationProgress])
            {
                animationProgress += 1;
                animationTime = 0;
            }

            if (animationProgress == 0)
            {
                //reverse the changes previously made to the position
                body.velocity = new Vector3(0,body.velocity.y, 0);
                agro = true;
            }
            else if (animationProgress == 1)
            {
                //let the enemy charge
            }
            else if (animationProgress == 2)
            {
                //let the animation cooldown
                body.velocity = new Vector3(0,body.velocity.y, 0);
                agro = false;
            }
            else
            {
                //animation is over!: reset the variables and enter calm mode
                animationProgress = 0;
                inAnimation = false;
                animationTime = 0;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerRigidBody.velocity = new Vector3(getEnemySpeed()*1.5f,  getEnemySpeed() * .1f, 0);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            hitEnemy();
        }
    }
}
