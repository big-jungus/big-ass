using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushTile : EffectTile
{
    [SerializeField] private Vector2 pushDir;
    [SerializeField] private float pushForce;

    protected override void Effect()
    {
        PlayerManager.playerManager.playerController.OnPushTile(pushDir.normalized * pushForce);
    }
}
