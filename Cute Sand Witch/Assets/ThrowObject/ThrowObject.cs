using UnityEngine;

public class ThrowObject : MonoBehaviour
{
	public GameObject SpawnedObjectPrefab;

	public bool IsPreview
	{
		set
		{
			if (value != isPreview)
			{
				isPreview = value;
				rb.isKinematic = isPreview;

				if (isPreview)
				{
					gameObject.layer = 0;
				}
				else
				{
					gameObject.layer = LayerMask.NameToLayer("ThrowObject");

				}
			}
		}
	}

	private Rigidbody rb;
	private bool isPreview = false;

	public void Init()
	{
		rb = GetComponent<Rigidbody>();
	}

	private void Start()
	{
		Init();
	}

	private void FixedUpdate()
	{
		if (isPreview)
			return;

		transform.LookAt(transform.position + rb.velocity);

		if (rb.IsSleeping())
			Destroy(this);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (isPreview)
			return;

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
