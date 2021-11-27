using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6f;
    public float fallSpeed = 1f;

    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Vector3 startPosition = new Vector3();
    public Vector3 moveDir = new Vector3(0, 0, 0);
    public float jumpSpeed = 1f;

    void Start()
    {
        Debug.Log("player script started");
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = -Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //begin with checking gravity
         if (!controller.isGrounded)
        {
                moveDir += Physics.gravity * fallSpeed * Time.deltaTime;
        }
        else //reset falling
        {
            moveDir = Vector3.zero;
        }
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //check for movement
        if (direction.magnitude >= 0.1f)
        {


            //Debug.LogError($"{direction * speed * Time.deltaTime}");
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            moveDir = moveDir.normalized * speed * Time.deltaTime;
        }

       
       
        if (Input.GetKey(KeyCode.Space))
        {
            if (controller.isGrounded)
            {
                moveDir += Vector3.up * jumpSpeed * Time.deltaTime;
            }
        }

        controller.Move(moveDir);

        //press r to reset position
        if (Input.GetKey(KeyCode.R))
        {
            this.transform.position = startPosition;
            Debug.Log("r is pressed");
        }
    }
}
