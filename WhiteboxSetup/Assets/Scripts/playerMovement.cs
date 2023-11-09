using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float turnRate;
    public float groundDrag;
    public float groundingForce;
    public Transform Orientation;
    public bool touch = false;

  
    [Header(" Layer of ground objects")]
    public LayerMask Groundlayer;


    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody playerRB;

    private float playerHeight;
    private  bool grounded;
    private bool playerInput;

    public AudioSource walksound;

    private Animator animator;
    private AnimatorController animatorController;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRB.freezeRotation = true;
        playerHeight = GetComponent<CapsuleCollider>().height;
        animator = GetComponent<Animator>();
        animatorController = GetComponent<AnimatorController>();    
   
        
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(GetComponent<CapsuleCollider>().transform.position, Vector3.down, playerHeight / 2 +0.1f, Groundlayer);
        Debug.DrawRay(GetComponent<CapsuleCollider>().transform.position, Vector3.down,Color.cyan);
        Debug.Log(grounded);
        physics();
        input();

    }

    private void FixedUpdate()
    {
        
        move();
    }

   

    private void input()
    {
        playerInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
      
           animator.SetBool("isMoving", playerInput);
          
         if (!walksound.isPlaying && playerInput)
        {
            walksound.Play();
        }

        
    }

    private void move()
    {
        //direction with respect to camera 
        moveDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
        playerRB.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        

    }

    private void physics()
    {
       
            playerRB.drag = groundDrag;
        

        if (!playerInput)
            playerRB.drag = groundDrag * 10;

        Vector3 xzVelocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);

        if (xzVelocity.magnitude > speed)
        {
            Vector3 cappedVelocity = xzVelocity.normalized * speed;
            playerRB.velocity = new Vector3(cappedVelocity.x, playerRB.velocity.y, cappedVelocity.z);
        }

        if (!grounded)
        {
            RaycastHit hit;
            Ray downRay = new Ray(transform.position, Vector3.down);
            if (Physics.Raycast(downRay, out hit))
            {
                float distanceToGround =  hit.distance - playerHeight * 0.5f;
                float pushDown = distanceToGround * groundingForce * 10 - playerRB.velocity.y *.5f ;
                playerRB.AddForce(pushDown * Vector3.down);
            }
        }

    }
    public void OnCollisionEnter(Collision collisionInfo)
    {

        if (collisionInfo.collider.CompareTag("Robot"))
        {
            touch = true;
            Debug.Log("Die");

        }
    }
}
