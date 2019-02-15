using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour {

    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 5f;
        public float rotateVel = 250f;
        public float jumpVel = 25;
        public float distToGrounded = 0.1f; // distance from target to ground
        public LayerMask ground; // can jump from ground
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public float inputDelay = 0.1f; //delay before input, purely preference choice
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;  // object that holds rotation in 4 axis, -1 to 1
    Rigidbody rBody;
    Animator anim;
    float forwardInput, turnInput, jumpInput;
    int animCondition = 0;
    bool isGrounded;

    public Quaternion TargetRotation    //for camera controller to use to get target rotation values
    {
        get { return targetRotation; }
    }

    /*bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }*/
	// Use this for initialization
	void Start () {
        targetRotation = transform.rotation;
        if (GetComponent<Animator>())
        {
            anim = GetComponent<Animator>();
        }
        else
        {
            Debug.LogError("The character needs an animator");
        }
        if (GetComponent<Rigidbody>())
        {
            rBody = GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("The character needs a rigidbody.");
        }
        forwardInput = turnInput = jumpInput = 0;   //initialise to 0
	}

    void getInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);   // value between -1 to 1
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);    // value between -1 to 1
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);   // only gives -1/0/1
    }
	
	// Update is called once per frame
	void Update () {
        getInput();
        turn();     //turning doesnt require any physics
        //Debug.Log(animCondition);
	}
     
    private void FixedUpdate()
    {

        jump();
        run();
        animCondition = !isGrounded ? 2 : (Mathf.Abs(forwardInput) > inputSetting.inputDelay) ? 1 : 0;
        anim.SetInteger("condition", animCondition);
        rBody.velocity = transform.TransformDirection(velocity);
    }

    void run()
    {
        if(Mathf.Abs(forwardInput) > inputSetting.inputDelay)
        {
            //move
            velocity.z = moveSetting.forwardVel * forwardInput;

        }
        else
        {
            // zero velocity
            velocity.z = 0;
        }
    }

    void turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay)
        {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void jump()
    {
        if (jumpInput > 0 && isGrounded)
        {
            //jump
            //anim.SetInteger("condition", 2);
            velocity.y = moveSetting.jumpVel;
        } else if (jumpInput == 0 && isGrounded)
        {
            //zero out velocity.y
            velocity.y = 0;
        }
        else
        {
            //decrease velocity.y
            velocity.y -= physSetting.downAccel;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("Platform"))
        {
            isGrounded = true;
        }else if (collision.collider.tag.Equals("Vanishing_Platform"))
        {
            isGrounded = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag.Equals("Platform"))
        {
            isGrounded = false;
        }
    }
}
