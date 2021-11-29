using UnityEngine;

public class ThrowObject : MonoBehaviour
{
	public GameObject SpawnedObjectPrefab;

	public int owner = 0;
	public System.Action OnDestroyed;

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

	public bool IsPickup
	{
		set
		{
			isPickup = value;
			rb.useGravity = !isPickup;
			foreach (var collider in GetComponents<Collider>())
			{
				collider.isTrigger = isPickup;
			}

			if (isPickup)
			{
				gameObject.layer = 0;
				rb.constraints = RigidbodyConstraints.FreezeRotation;
			}
			else
			{
				gameObject.layer = LayerMask.NameToLayer("ThrowObject");
				rb.constraints = RigidbodyConstraints.None;
			}
		}
		get
		{
			return isPickup;
		}
	}

	[SerializeField]
	private Rigidbody rb;
	private bool isPreview = false;
	private bool isPickup = false;

	private void OnDestroy()
	{
		if (OnDestroyed != null)
		{
			OnDestroyed.Invoke();
		}
	}

	public void Init()
	{
		rb = GetComponent<Rigidbody>();
		//IsPickup = true;
	}

	private void Start()
	{
		Init();

		//gameObject.layer = 0;
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
		if (isPreview || isPickup)
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
		if (spawnedObject.TryGetComponent(out CastleComponent castleComponent))
		{
			castleComponent.owner = owner;
		}
		Destroy(this.gameObject);
	}
}
