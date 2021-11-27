using UnityEngine;

public class CastleCore : CastleComponent
{
    protected override void OnComponentDestroyed()
    {
        base.OnComponentDestroyed();

        //TODO: Loose the game for whoever owns this object.
        //owner.Loose();
        Debug.LogError($"Player {owner} has lost!");
    }
}
