using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CastleCore : CastleComponent
{
    public ParticleSystem shockwaveController;

    public int shockwaveDamage;

    public float shockwaveRadius;

    public List<GameObject> teamModels;

    protected override void Start()
    {
        base.Start();
        shockwaveController.Stop();

        //Make only the correct team color visible
        foreach (GameObject model in teamModels)
        {
            model.SetActive(false);
        }

        teamModels[owner].SetActive(true);
    }

    protected override void OnComponentDestroyed()
    {
        base.OnComponentDestroyed();

        //TODO: Loose the game for whoever owns this object.
        //owner.Loose();
        Debug.LogError($"Player {owner} has lost!");
    }

    protected override void OnDamageTaken()
    {
        base.OnDamageTaken();

        //Get nearby crabs
        Collider[] everything = Physics.OverlapSphere(transform.position, shockwaveRadius);

        List<CrabArmy> crabs = everything.Where((Collider c) =>
        {
            if (c.transform.parent != null)
            {
                CrabArmy army = c.transform.parent.GetComponent<CrabArmy>();
                if (army != null)
                {
                    return true;
                }
            }

            return false;
        }).Select((Collider c) => { return c.transform.parent.GetComponent<CrabArmy>(); }).ToList();

        //Shockwave takes a little while, wait a little to damage crabs
        StartCoroutine(DamageCrabs(crabs));

        //Emit Anti-Crab Wave
        shockwaveController.Play();
    }

    IEnumerator DamageCrabs(List<CrabArmy> crabs)
    {
        yield return new WaitForSeconds(0.5f);

        foreach(CrabArmy a in crabs)
        {
            if (a != null)
            {
                a.Damage(shockwaveDamage);
            }
        }
    }
}
