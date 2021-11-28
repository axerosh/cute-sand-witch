using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimerScript : MonoBehaviour
{
    private BallisticPreview preview;
    public GameObject aimObject;
    public PlayerMovement.InputMethod inputMethod;
    public Vector3 throwDirection = new Vector3(0, 10, 0); 
    private GameObject previewCube;
    public Vector3 throwDifference = new Vector3(0, 0, 5);
    public Vector3 throwDirectionShort = new Vector3(0, 7, 3); 
    public Vector3 throwDirectionMedium = new Vector3(0, 10, 5);
    public Vector3 throwDirectionLong = new Vector3(0, 15, 8);
    public ThrowObject SpawnedObjectPrefab;
    private Vector3 previewScale = new Vector3(0.5f, 0.5f, 0.5f) * 0.5f;
    public Transform witchTransform;
    public List<ThrowObject> throwables;

    void Start()
    {
        Debug.Log("player script started");
        preview = GetComponent<BallisticPreview>();

        preview.StepPreviewPrefab = aimObject;
        preview.InitialRelativeVelocity = throwDirectionMedium;
        throwDirection = throwDirectionMedium;

        SetPreviewCube();
    }

    public void SetNewAmmo(ThrowObject newAmmo)
    {
        SpawnedObjectPrefab = newAmmo;
        Destroy(previewCube);
        SetPreviewCube();
    }

    private void SetPreviewCube()
    {
        //SpawnedObjectPrefab.
        previewCube = Instantiate(SpawnedObjectPrefab.SpawnedObjectPrefab, transform);

        previewCube.layer = 0;
        // previewCube.GetComponent<Rigidbody>().isKinematic = false;
        previewCube.transform.localScale = previewScale;
    }

    // Update is called once per frame
    void Update()
    {
        previewCube.transform.rotation = witchTransform.rotation;
        
        //preview.InitialRelativeVelocity = throwDirection;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            // Debug.Log("Q is pressed");
            throwDirection = throwDirectionShort;

        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            // Debug.Log("Q is pressed");
            throwDirection = throwDirectionMedium;

        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            // Debug.Log("Q is pressed");
            throwDirection = throwDirectionLong;

        }
        if ((inputMethod == PlayerMovement.InputMethod.KeyboardMouse && Input.GetButtonDown("FireKey"))
            || (inputMethod == PlayerMovement.InputMethod.Controller && Input.GetButtonDown("FireJoy")))
        {
            var spawnedObject = Instantiate(SpawnedObjectPrefab, transform.root.parent);
            spawnedObject.transform.position = transform.position;
            var rb = spawnedObject.GetComponent<Rigidbody>();
            rb.velocity = transform.rotation * throwDirection;
        }
        preview.InitialRelativeVelocity = throwDirection;

    }
}
