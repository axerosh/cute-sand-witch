using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraOrigin;
    public AimerScript aimer;

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
    private Vector3 fallVelocity = new Vector3(0, 0, 0);

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

        Vector3 inputDirection = horizontal * transform.right + vertical * transform.forward;

        //begin with checking gravity
        if (!controller.isGrounded)
        {
            fallVelocity += fallSpeed *Time.deltaTime * Physics.gravity;
            //Debug.Log($"Fall");
        }
        else //reset falling
        {
            fallVelocity.y = -0.5f; // Since isGrounded is stupid
            //Debug.Log($"Grounded");

            if ((inputMethod == InputMethod.KeyboardMouse && Input.GetButtonDown("JumpKey"))
                || (inputMethod == InputMethod.Controller && Input.GetButtonDown("JumpJoy")))
            {
                Debug.Log($"Jump");
                fallVelocity += Vector3.up * jumpSpeed;
            }
        }

        //check for movement
        if (inputDirection.magnitude >= 0.1f)
        {
            inputDirection = inputDirection.normalized * speed;
        }

        controller.Move((fallVelocity + inputDirection) * Time.deltaTime);

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
                break;
            case InputMethod.Controller:
                rotationHorizontal = Input.GetAxisRaw("LookHorizontalJoy");
                rotationVertical = Input.GetAxisRaw("LookVerticalJoy");
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

	private void OnTriggerEnter(Collider collider)
    {
        Debug.Log("player on collision started" + collider.gameObject.name);
        //ThrowObject throwObject = collision.gameObject.GetComponent<ThrowObject>();
        switch(collider.gameObject.name)
        {
            case "ThrowSand(Clone)":
                aimer.SetNewAmmo(aimer.throwables[0]);
                Destroy(collider.gameObject);
                break;
            case "ThrowStone(Clone)":
                aimer.SetNewAmmo(aimer.throwables[1]);
                Destroy(collider.gameObject);
                break;
            case "ThrowCrab(Clone)":
                aimer.SetNewAmmo(aimer.throwables[2]);
                Destroy(collider.gameObject);
                break;
            case "ThrowTank(Clone)":
                aimer.SetNewAmmo(aimer.throwables[3]);
                Destroy(collider.gameObject);
                break;
            default:
                Debug.Log("Collided with not trowable!");
                return;
        }
    }

    /*
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("controller collider hit"+hit.gameObject.name);
        ThrowObject throwObject = hit.gameObject.GetComponent<ThrowObject>();
        if (throwObject != null)
        {
            Debug.Log("collision is throw object");
            if (throwObject.IsPickup)
            {
                Debug.Log("collision is pickup");
                Destroy(hit.gameObject);
            }
        }
    }
    */
}
