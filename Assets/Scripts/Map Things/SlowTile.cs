using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTile : EffectTile
{
    [SerializeField] private float slowAmount;

    protected override void Effect()
    {
        float slowedVelocity = PlayerManager.playerManager.playerController.rb.velocity.magnitude - slowAmount;
        Vector2 normalDir = PlayerManager.playerManager.playerController.rb.velocity.normalized;

        PlayerManager.playerManager.playerController.rb.velocity = normalDir * slowedVelocity;
    }
}
