using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{

    public ThrowObject ToSpawn;
    private ThrowObject pickUp;

    // Start is called before the first frame update
    void Start()
    {
        pickUp = Instantiate(ToSpawn, this.transform);
        pickUp.transform.position = new Vector3(pickUp.transform.position.x, 0, pickUp.transform.position.z);
        pickUp.IsPickup = true;
        pickUp.IsPreview = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
