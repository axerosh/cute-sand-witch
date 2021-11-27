using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimerScript : MonoBehaviour
{
    private BallisticPreview preview;
    public ThrowObject aimObject;
    //public Vector3 throwDirection = new Vector3(0, 10, 0); 
    public Vector3 throwDifference = new Vector3(0, 0, 5);
    public Vector3 throwDirectionShort = new Vector3(0, 7, 3); 
    public Vector3 throwDirectionMedium = new Vector3(0, 10, 5);
    public Vector3 throwDirectionLong = new Vector3(0, 15, 8);

    void Start()
    {
        Debug.Log("player script started");
        preview = GetComponent<BallisticPreview>();

        preview.StepPreviewPrefab = aimObject;
        preview.InitialRelativeVelocity = throwDirectionMedium;
    }

    // Update is called once per frame
    void Update()
    {
      //preview.InitialRelativeVelocity = throwDirection;
        if (Input.GetKey(KeyCode.Alpha1))
        {
            // Debug.Log("Q is pressed");
            preview.InitialRelativeVelocity = throwDirectionShort;

        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            // Debug.Log("Q is pressed");
            preview.InitialRelativeVelocity = throwDirectionMedium;

        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            // Debug.Log("Q is pressed");
            preview.InitialRelativeVelocity = throwDirectionLong;

        }
    }
}
