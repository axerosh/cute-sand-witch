using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CollectableType = ThrowObject;

public class CollectableSpawner : MonoBehaviour
{

	[System.Serializable]
	public struct SpawnObjectWithWeight
	{
		public CollectableType ObjectPrefab;
		public float Weight;
	}

	public List<SpawnObjectWithWeight> SpawnObjectsWithWeights = new List<SpawnObjectWithWeight>();
	public int MaxCollectableCount = 20;
	public float TimeBetweenSpawns = 1f;
	public float SpawnRadius = 5f;

	private readonly List<CollectableType> spawnedCollectables = new List<CollectableType>();
	private float totalWeight = 0f;
	private int layerMask;

	private void Start()
	{
		layerMask = LayerMask.GetMask(new string[] { "BuildSurface" });
		foreach (var objectWithWeight in SpawnObjectsWithWeights)
		{
			totalWeight += objectWithWeight.Weight;
		}
		StartCoroutine(SpawnObjectCoroutine());
	}

	IEnumerator SpawnObjectCoroutine()
	{
		while(true)
		{
			yield return new WaitForSecondsRealtime(TimeBetweenSpawns);
			if (spawnedCollectables.Count < MaxCollectableCount)
			{
				float pick = Random.value * totalWeight;
				float accumulatedWeight = 0f;
				foreach (var objectWithWeight in SpawnObjectsWithWeights)
				{
					accumulatedWeight += objectWithWeight.Weight;
					if (accumulatedWeight >= pick)
					{
						Vector2 circlePosition = Random.insideUnitCircle * SpawnRadius;
						Vector3 spawnPosition = transform.position + new Vector3(circlePosition.x, 0f, circlePosition.y);

						if (Physics.Raycast(spawnPosition + Vector3.up, Vector3.down, out var hit, 10f, layerMask)
							&& hit.transform.name.Contains("Terrain"))
						{
							var spawnedObject = Instantiate(objectWithWeight.ObjectPrefab, transform.root);
							spawnedObject.Init();

							spawnedCollectables.Add(spawnedObject);
							spawnedObject.OnDestroyed += () => { spawnedCollectables.Remove(spawnedObject); };

							spawnPosition.y = hit.point.y + 0.1f;
							spawnedObject.transform.position = spawnPosition;
							spawnedObject.transform.Rotate(0f, Random.Range(-180, 180), 0f);
							spawnedObject.IsPickup = true;
							spawnedObject.IsPreview = false;
						}
						break;
					}

				}
			}
		}
	}
}