using UnityEngine;

public class Spawner : MonoBehaviour
{
	public GameObject SpawnedObjectPrefab;
	public Vector3 InitialVelocity = 10f * Vector3.up;

	private void Start()
	{
		if(TryGetComponent(out BallisticPreview preview))
		{
			preview.InitialRelativeVelocity = InitialVelocity;
		}
	}

	void Update()
	{
		if (Time.frameCount % 50 == 0)
		{
			var spawnedObject = Instantiate(SpawnedObjectPrefab, transform.parent);
			spawnedObject.transform.position = transform.position;
			var rb = spawnedObject.GetComponent<Rigidbody>();
			rb.velocity = transform.rotation * InitialVelocity;
		}
	}
}
