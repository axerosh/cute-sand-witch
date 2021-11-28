using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Castle component.
/// </summary>
public class CastleComponent : MonoBehaviour
{
    public int startingHealth;

    //TODO: Replace with Player object once it exists.
    public int owner;

    private int health;

    protected virtual void Start()
    {
        health = startingHealth;
    }

    public void Damage(int amount)
    {
        health -= amount;

        OnDamageTaken();

        if (health <= 0)
        {
            OnComponentDestroyed();

            Destroy(gameObject);
        }
    }

    protected virtual void OnComponentDestroyed()
    {

    }

    protected virtual void OnDamageTaken()
    {

    }
}
