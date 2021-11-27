using UnityEngine;

public class ThrowObject : MonoBehaviour
{
	public GameObject SpawnedObjectPrefab;

	private void OnCollisionEnter(Collision collision)
	{
		// Dummy default values
		Vector3 closestPoint = transform.position;
		Vector3 closestNormal = collision.transform.position - closestPoint;
		float closestDistance = float.MaxValue;

		int contactCount = collision.contactCount;
		for (int i = 0; i < contactCount; ++i)
		{
			var contact = collision.GetContact(i);
			float contactDistance = (contact.point - transform.position).sqrMagnitude;

			if (contactDistance < closestDistance)
			{
				closestPoint = contact.point;
				closestNormal = contact.normal;
				closestDistance = contactDistance;
			}
		}

		OnHit(closestPoint, closestNormal);
	}

	private void OnHit(Vector3 globalHitPostion, Vector3 hitNormal)
	{
		var spawnedObject = Instantiate(SpawnedObjectPrefab, transform.parent);
		spawnedObject.transform.position = globalHitPostion;
		spawnedObject.transform.LookAt(globalHitPostion + hitNormal);
		Destroy(this);
	}
}
