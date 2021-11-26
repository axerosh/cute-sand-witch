using UnityEngine;

public class ThrowObject : MonoBehaviour
{
	public GameObject SpawnedObjectPrefab;

	private void OnCollisionEnter(Collision collision)
	{
		Vector3 closestPoint = transform.position; // Dummy default value
		float closestDistance = float.MaxValue;

		int contactCount = collision.contactCount;
		for (int i = 0; i < contactCount; ++i)
		{
			Vector3 contactPoint = collision.GetContact(i).point;
			float contactDistance = (contactPoint - transform.position).sqrMagnitude;

			if (contactDistance < closestDistance)
			{
				closestPoint = contactPoint;
				closestDistance = contactDistance;
			}
		}

		OnHit(closestPoint);
	}

	private void OnHit(Vector3 globalHitPostion)
	{
		var spawnedObject = Instantiate(SpawnedObjectPrefab, transform.parent);
		spawnedObject.transform.position = globalHitPostion;
		Destroy(this);
	}
}
