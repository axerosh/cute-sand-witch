using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabBarracks : CastleComponent
{
    public CrabArmy crabPrefab;

    Transform crabSpawnPoint;

    public float cooldown;

    private float currentCooldown = Mathf.Infinity;

    private CastleCore opponentCore;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentCooldown = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentCooldown <= 0)
        {
            SpawnCrabs();
            currentCooldown = cooldown;
        }
        else
        {
            currentCooldown -= Time.deltaTime;
        }
    }

    private void SpawnCrabs()
    {
        Instantiate(crabPrefab, crabSpawnPoint.position, Quaternion.identity);
    }
}
