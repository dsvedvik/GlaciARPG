using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    //keycodes for each direction
    public KeyCode moveUp;
    public KeyCode moveDown;
    public KeyCode moveLeft;
    public KeyCode moveRight;
    public KeyCode nAttack;

    public float speed = 10f;   //the speed when moving along just x or just y
    private float diagSpeed;    //stores the speed for x and y when traveling in a diagonal (calculated at Awake())

    private Rigidbody2D rigi;   //reference to the player's rigidbody2d component
    private Animator m_Anim;    //reference to the player's Animator compoinent

    private Vector2 movement;   //store movement from update for fixedupdate
	
    void Awake()
    {
        //high and low speeds, used to calculate diagonal speed
        float high = speed;
        float low = speed / 2;
        float target = (speed * speed) / 2;

        rigi = GetComponent<Rigidbody2D>();
        m_Anim = GetComponent<Animator>();

        //Use speed to find the right speed to use for diagonals.
        for (int i = 0; i < 3; i++)
        {
            diagSpeed = (high + low) / 2;
            if ((diagSpeed * diagSpeed) > target - 3 && (diagSpeed * diagSpeed) < target + 3)
            {
                i = 3;
            }
            else if ((diagSpeed * diagSpeed) < target)
            {
                low = diagSpeed;
                diagSpeed = high + low / 2;
            }
            else if ((diagSpeed * diagSpeed) > target)
            {
                high = diagSpeed;
                diagSpeed = high + low / 2;
            }
        }
        Debug.Log("diagSpeed is " + diagSpeed);

    }

	// Update is called once per frame
    //Checks for directional keypresses, and adds to/subtracts from the velocity as necessary. If a diagonal is detected, half each velocity.
	void Update () {
        float frameSpeedx = 0f;
        float frameSpeedy = 0f;

        //Check for directional keys and adjust new velocity accordingly
        if (Input.GetKey(moveUp))
        {
            frameSpeedy += speed;
        }
        if (Input.GetKey(moveDown))
        {
            frameSpeedy -= speed;
        }
        if (Input.GetKey(moveLeft))
        {
            frameSpeedx -= speed;
        }
        if (Input.GetKey(moveRight))
        {
            frameSpeedx += speed;
        }
        //If a diagonal is detected, half both speeds
        if ((frameSpeedx * frameSpeedx) == (speed * speed) && (frameSpeedy * frameSpeedy) == (speed * speed))
        {
            if (frameSpeedx > 0)
            {
                frameSpeedx = diagSpeed;
            }
            else
            {
                frameSpeedx = -diagSpeed;
            }
            if (frameSpeedy > 0)
            {
                frameSpeedy = diagSpeed;
            }
            else
            {
                frameSpeedy = -diagSpeed;
            }
        }
        //Set the new velocity
        /*
        rigi.velocity = new Vector3(frameSpeedx, frameSpeedy, 0f);
        m_Anim.SetFloat("xSpeed", frameSpeedx);
        m_Anim.SetFloat("ySpeed", frameSpeedy);
        */
        movement = new Vector2(frameSpeedx, frameSpeedy);

        if (Input.GetKey(nAttack))
        {
            m_Anim.SetBool("nAttack", true);
        }
        else
        {
            m_Anim.SetBool("nAttack", false);
        }
    }
    
    void FixedUpdate()
    {
        rigi.velocity = movement;
        m_Anim.SetFloat("xSpeed", movement[0]);
        m_Anim.SetFloat("ySpeed", movement[1]);
    }
}
