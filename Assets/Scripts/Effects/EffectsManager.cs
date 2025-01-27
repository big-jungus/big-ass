using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject hitSparkPrefab;

    private void Start()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
        PlayerManager.playerManager.playerController.CollectableCollected += CollectableCollected;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
        PlayerManager.playerManager.playerController.CollectableCollected -= CollectableCollected;
    }

    public void SpawnHitSpark(Vector3 position)
    {
        GameObject sparkObj = Instantiate(hitSparkPrefab);
        sparkObj.transform.position = position;
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        SpawnHitSpark(collision.GetContact(0).point);
    }

    public void EnemyKill(Vector3 position)
    {

    }

    public void CollectableCollected(Vector3 position, Collectable.CollectableTypes c)
    {

    }

    public void WallBreak(Vector3 position)
    {
        
    }
}
