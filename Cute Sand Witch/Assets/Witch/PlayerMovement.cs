using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cameraOrigin;
    public AimerScript aimer;

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
        if (controller.isGrounded && direction.magnitude >= 0.1f)     // This prevents flying
        //if (direction.magnitude >= 0.1f)                          // this enables flying
        {
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("player on  collision started" + collision.gameObject.name);
        //ThrowObject throwObject = collision.gameObject.GetComponent<ThrowObject>();
        switch(collision.gameObject.name)
        {
            case "ThrowSand(Clone)":
                aimer.SetNewAmmo(aimer.throwables[0]);
                Destroy(collision.gameObject);
                break;
            case "ThrowStone(Clone)":
                aimer.SetNewAmmo(aimer.throwables[1]);
                Destroy(collision.gameObject);
                break;
            case "ThrowCrab(Clone)":
                aimer.SetNewAmmo(aimer.throwables[2]);
                Destroy(collision.gameObject);
                break;
            case "ThrowTank(Clone)":
                aimer.SetNewAmmo(aimer.throwables[3]);
                Destroy(collision.gameObject);
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
