using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private void Start()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        
    }
}
