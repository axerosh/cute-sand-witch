using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraOrigin;
    public InputMethod inputMethod;

    public enum InputMethod
    {
        KeyboardMouse,
        Controller,
    }

    public float speed = 6f;
    public float fallSpeed = 1f;
    public float jumpSpeed = 1f;

    public float rotationSpeedHorizontal = 240f;
    public float rotationSpeedVertical = 120f;
    public float maxVerticalRotation = 30f;
    public float minVerticalRotation = -30f;

    private Vector3 startPosition;
    private Vector3 moveDir = new Vector3(0, 0, 0);

    void Start()
    {
        Debug.Log("player script started");
        startPosition = transform.position;
        SetMouseLocked(true);
    }

    // Update is called once per frame
    void Update()
    {
        // Movement

        float horizontal = 0f;
        float vertical = 0f;
        switch (inputMethod)
        {
            case InputMethod.KeyboardMouse:
                horizontal = Input.GetAxisRaw("HorizontalKey");
                vertical = Input.GetAxisRaw("VerticalKey");
                break;
            case InputMethod.Controller:
                horizontal = Input.GetAxisRaw("HorizontalJoy");
                vertical = Input.GetAxisRaw("VerticalJoy");
                break;
        }

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
       
        if ((inputMethod == InputMethod.KeyboardMouse && Input.GetButton("JumpKey"))
            || (inputMethod == InputMethod.Controller && Input.GetButton("JumpJoy")))
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

        float rotationHorizontal = 0f;
        float rotationVertical = 0f;
        switch (inputMethod)
        {
            case InputMethod.KeyboardMouse:
                rotationHorizontal = Input.GetAxisRaw("Mouse X");
                rotationVertical = -Input.GetAxisRaw("Mouse Y");
                Debug.Log($"Mouse: rotationHorizontal={rotationHorizontal}, rotationVertical={rotationVertical}");
                break;
            case InputMethod.Controller:
                rotationHorizontal = Input.GetAxisRaw("LookHorizontalJoy");
                rotationVertical = Input.GetAxisRaw("LookVerticalJoy");
                Debug.Log($"Joy: rotationHorizontal={rotationHorizontal}, rotationVertical={rotationVertical}");
                break;
        }

        rotationHorizontal = Mathf.Min(rotationHorizontal, 1f);
        rotationVertical = Mathf.Min(rotationVertical, 1f);

        if (rotationHorizontal > 0.1f || rotationHorizontal < -0.1f)
        {
            transform.Rotate(0f, rotationHorizontal * rotationSpeedHorizontal * Time.deltaTime, 0f);
        }

        if (rotationVertical > 0.1f || rotationVertical < -0.1f)
        {
            cameraOrigin.Rotate(rotationVertical * rotationSpeedVertical * Time.deltaTime, 0f, 0f);
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
