using System.Collections.Generic;
using UnityEngine;

public class BallisticPreview : MonoBehaviour
{
	public float StepDistance = 0.2f;
	public int MaxSteps = 20;

	[HideInInspector]
	public ThrowObject StepPreviewPrefab;
	[HideInInspector]
	public Vector3 InitialRelativeVelocity;

	private List<ThrowObject> stepPreviews = new List<ThrowObject>();
	private int layerMask;

	private void Start()
	{
		layerMask = LayerMask.GetMask(new string[] { "BuildSurface" });
	}

	void Update()
	{
		Vector3 initialVelocity = transform.rotation * InitialRelativeVelocity;
		Vector3 velocity = initialVelocity;
		Vector3 position = transform.position;
		float time = 0;
		int steps = -1;

		time += StepDistance / velocity.magnitude;
		Vector3 nextPosition = (initialVelocity + 0.5f * Physics.gravity * time) * time + transform.position;

		while (!Physics.Raycast(position, nextPosition - position, (nextPosition - position).magnitude, layerMask)
			&& ++steps < MaxSteps)
		{
			position = nextPosition;
			velocity = initialVelocity + Physics.gravity * time;

			time += StepDistance / velocity.magnitude;
			nextPosition = (initialVelocity + 0.5f * Physics.gravity * time) * time + transform.position;

			if (steps < stepPreviews.Count)
			{
				// Reuse preview object
				stepPreviews[steps].transform.position = position;
				stepPreviews[steps].transform.LookAt(position + velocity);
			}
			else
			{
				// New preview object
				var newStepPreview = Instantiate(StepPreviewPrefab, transform.root);
				newStepPreview.Init();
				newStepPreview.IsPreview = true;
				newStepPreview.transform.position = position;
				newStepPreview.transform.LookAt(position + velocity);
				stepPreviews.Add(newStepPreview);
			}
		}

		// Remove unused preview objects
		if (steps + 1 < stepPreviews.Count)
		{
			int startIndex = steps + 1;
			for (int i = startIndex; i < stepPreviews.Count; ++i)
			{
				Destroy(stepPreviews[i].gameObject);
			}
			int count = stepPreviews.Count - startIndex;
			stepPreviews.RemoveRange(startIndex, count);
		}
	}
}
