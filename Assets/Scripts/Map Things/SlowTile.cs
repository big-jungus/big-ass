using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTile : EffectTile
{
    [SerializeField] private float slowAmount;

    protected override void Effect()
    {
        Vector2 vel = PlayerManager.playerManager.playerController.rb.velocity;
        PlayerManager.playerManager.playerController.rb.AddForce(new Vector2(-vel.x, -vel.y).normalized * vel.magnitude * vel.magnitude * slowAmount, ForceMode2D.Force);

        /*float slowedVelocity = PlayerManager.playerManager.playerController.rb.velocity.magnitude - slowAmount;
        Vector2 normalDir = PlayerManager.playerManager.playerController.rb.velocity.normalized;

        PlayerManager.playerManager.playerController.rb.velocity = normalDir * slowedVelocity;*/
    }
}
