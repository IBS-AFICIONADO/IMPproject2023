using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    public float speed;
    public float turnRate;
    public float groundDrag;
    public Transform Orientation;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;

    private Rigidbody playerRB;

    private float playerHeight;
    public LayerMask ground;
    private  bool grounded;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        playerRB.freezeRotation = true;
        playerHeight = GetComponent<CapsuleCollider>().height;
       
    
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, ground);
        physics();
        input();
        if (grounded)
        {
          //  Debug.Log();
        }
    }

    private void FixedUpdate()
    {
        move();
    }

    private void input()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void move()
    {
        //direction with respect to camera 
        moveDirection = Orientation.forward * verticalInput + Orientation.right * horizontalInput;
        playerRB.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

    private void physics()
    {
        if (grounded)
            playerRB.drag = groundDrag;
        else
            playerRB.drag = 0;

        Vector3 xzVelocity = new Vector3(playerRB.velocity.x, 0f, playerRB.velocity.z);

        if (xzVelocity.magnitude > speed)
        {
            Vector3 cappedVelocity = xzVelocity.normalized * speed;
            playerRB.velocity = new Vector3(cappedVelocity.x, playerRB.velocity.y, cappedVelocity.z);
        }
    }
}
