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
    public int playerID = 0;

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

    public void SetNoAmmo()
    {
        SpawnedObjectPrefab = null;
        Destroy(previewCube);
    }

    private void SetPreviewCube()
    {
        //SpawnedObjectPrefab.
        previewCube = Instantiate(SpawnedObjectPrefab.SpawnedObjectPrefab, transform);
        var wc = previewCube.GetComponent<CastleComponent>();
        if(wc != null)
        {
            wc.enabled = false;
        }

        previewCube.layer = 0;
        // previewCube.GetComponent<Rigidbody>().isKinematic = false;
        previewCube.transform.localScale = previewScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(previewCube != null)
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
        if (SpawnedObjectPrefab != null && ((inputMethod == PlayerMovement.InputMethod.KeyboardMouse && Input.GetButtonDown("FireKey"))
            || (inputMethod == PlayerMovement.InputMethod.Controller && Input.GetButtonDown("FireJoy"))))
        {
            var spawnedObject = Instantiate(SpawnedObjectPrefab, transform.root.parent);
            spawnedObject.owner = playerID;
            var cc = spawnedObject.GetComponent<CastleComponent>();
            if (cc != null)
                spawnedObject.GetComponent<CastleComponent>().enabled = true;
            spawnedObject.transform.position = transform.position;
            var rb = spawnedObject.GetComponent<Rigidbody>();
            rb.velocity = transform.rotation * throwDirection;
            SetNoAmmo();
        }
        preview.InitialRelativeVelocity = throwDirection;

    }
}
