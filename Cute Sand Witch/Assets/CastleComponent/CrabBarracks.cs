using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CrabBarracks : CastleComponent
{
    public CrabArmy crabPrefab;

    public Transform crabSpawnPoint;

    public float cooldown;

    private float currentCooldown = Mathf.Infinity;

    private CastleCore opponentCore;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        currentCooldown = cooldown;

        opponentCore = FindObjectsOfType<CastleCore>().Where((CastleCore c) => { return c.owner != owner; }).FirstOrDefault();
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
        CrabArmy newArmy = Instantiate(crabPrefab, crabSpawnPoint.position, transform.rotation);

        newArmy.Init(owner, opponentCore);
    }
}
