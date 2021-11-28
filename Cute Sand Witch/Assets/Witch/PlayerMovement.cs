using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraOrigin;

    public float speed = 6f;
    public float fallSpeed = 1f;
    public float jumpSpeed = 1f;

    public float rotationSpeedHorizontal = 240f;
    public float rotationSpeedVertical = 120f;
    public float maxVerticalRotation = 30f;
    public float minVerticalRotation = -30f;

    private Vector3 startPosition = new Vector3();
    private Vector3 moveDir = new Vector3(0, 0, 0);

    void Start()
    {
        Debug.Log("player script started");
        SetMouseLocked(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = horizontal * transform.right + vertical * transform.forward;

        //begin with checking gravity
        if (!controller.isGrounded)
        {
            moveDir += Physics.gravity * fallSpeed * Time.deltaTime;
        }
        else //reset falling
        {
            moveDir = Vector3.zero;
        }

        //check for movement
        if (controller.isGrounded && direction.magnitude >= 0.1f)
        {
            //Debug.LogError($"{direction * speed * Time.deltaTime}");
            moveDir = direction;
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

        // Rotation

        float mouseDeltaX = Input.GetAxisRaw("Mouse X");
        float mouseDeltaY = Input.GetAxisRaw("Mouse Y");

        mouseDeltaX = Mathf.Min(mouseDeltaX, 1f);
        mouseDeltaY = Mathf.Min(mouseDeltaY, 1f);

        if (mouseDeltaX > 0.1f || mouseDeltaX < -0.1f)
        {
            transform.Rotate(0f, mouseDeltaX * rotationSpeedHorizontal * Time.deltaTime, 0f);
        }

        if (mouseDeltaY > 0.1f || mouseDeltaY < -0.1f)
        {
            cameraOrigin.Rotate(-mouseDeltaY * rotationSpeedVertical * Time.deltaTime, 0f, 0f);
            var rotation = cameraOrigin.localRotation;
            rotation.x = Mathf.Clamp(rotation.x,
                0.5f * Mathf.Deg2Rad * minVerticalRotation,
                0.5f * Mathf.Deg2Rad * maxVerticalRotation);
            rotation.y = 0f;
            rotation.z = 0f;
            cameraOrigin.localRotation = rotation;
        }
    }


    // Mouse locking

    private void OnApplicationFocus(bool hasFocus)
    {
        SetMouseLocked(hasFocus);
    }

    private void OnApplicationPause(bool isPaused)
    {
        SetMouseLocked(!isPaused);
    }

    private void SetMouseLocked(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}
