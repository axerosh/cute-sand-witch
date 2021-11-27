using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabArmy : MonoBehaviour
{
    public int owner;

    public int initialCrabs;

    public int startingHealth;

    public int minDamage;

    public int maxDamage;

    public float attackCooldown;

    public float speed;

    public Crab crabPrefab;

    public Transform hitBox;

    private int currentHealth;

    private float currentAttackCooldown = 0;

    private readonly List<Crab> conscriptedCrabs = new();

    private readonly List<CrabArmy> engagedCrabs = new();

    private CastleCore target;
    private CastleComponent currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        SpawnCrabs();
        target = FindObjectsOfType<CastleCore>().Where((CastleCore c) => c.owner != owner).First();
        transform.LookAt(target.transform, Vector3.up);

        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Are there targets?
        if (currentTarget != null || engagedCrabs.Count > 0)
        {
            //Can we attack?
            if (currentAttackCooldown <= 0)
            {
                //Attack building
                int damage = Random.Range(minDamage, maxDamage) * conscriptedCrabs.Count;

                if (currentTarget != null) currentTarget.Damage(damage);

                //Attack engaged crabs.
                if (engagedCrabs.Count > 0)
                {
                    foreach (CrabArmy army in engagedCrabs)
                    {
                        if (army != null) army.Damage(damage);
                    }

                    engagedCrabs.RemoveAll((CrabArmy a) => a == null);
                }

                currentAttackCooldown = attackCooldown;
            }
        }
        else if (target != null)
        {
            transform.Translate((target.transform.position - transform.position).normalized * speed * Time.deltaTime, Space.World);
        }

        currentAttackCooldown -= Time.deltaTime;
    }

    public void Damage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            float healthPercent = (float)currentHealth / startingHealth;
            int livingCrabs =  (int)Mathf.Ceil(initialCrabs * healthPercent);

            while(conscriptedCrabs.Count > livingCrabs)
            {
                Destroy(conscriptedCrabs[0].gameObject);
                conscriptedCrabs.RemoveAt(0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collided with " + collision.gameObject.name);
        if (collision.rigidbody != null)
        {
            CastleComponent hitComponent = collision.rigidbody.gameObject.GetComponent<CastleComponent>();
            if (hitComponent != null)
            {
                currentTarget = hitComponent;
            }

            CrabArmy hitArmy = collision.gameObject.GetComponent<CrabArmy>();
            if (hitArmy != null && hitArmy.owner != owner)
            {
                Debug.Log("Found enemy!");
                engagedCrabs.Add(hitArmy);
            }
        }
    }

    private void SpawnCrabs()
    {
        for (int i = 0; i < initialCrabs; ++i)
        {
            Vector2 spawnOffset = Random.insideUnitCircle * (hitBox.localScale.z / 2);

            Vector3 spawnPoint = hitBox.position;
            spawnPoint.x += spawnOffset.x;
            spawnPoint.z += spawnOffset.y; 

            Crab newCrab = Instantiate(crabPrefab, spawnPoint, Quaternion.identity, transform);

            newCrab.SetBobPhase(Random.value);

            conscriptedCrabs.Add(newCrab);
        }
    }
}
