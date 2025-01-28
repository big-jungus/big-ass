using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] Color[] hitCircleColors;
    [SerializeField] GameObject hitCirclePrefab;
    [SerializeField] Color[] hitShineColors;
    [SerializeField] GameObject hitShinePrefab;
    [Space(10)]
    [SerializeField] Color[] spikeEffectColors;
    [SerializeField] GameObject spikeEffectPrefab;
    [Space(10)]
    [SerializeField] Color[] pickupColors;
    [SerializeField] GameObject pickupSparkle;
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
        int index = Random.Range(0, hitCircleColors.Length);
        Instantiate(hitCirclePrefab, position, Quaternion.identity).GetComponent<SpriteRenderer>().color = hitCircleColors[index];
        Instantiate(hitShinePrefab, position, Quaternion.identity).GetComponent<SpriteRenderer>().color = hitShineColors[index];
        // GameObject sparkObj = Instantiate(hitSparkPrefab);
        // sparkObj.transform.position = position;
    }
    public void SpawnSpikeSpark(Vector3 position)
    {
        Instantiate(spikeEffectPrefab, position, Quaternion.identity).GetComponent<SpriteRenderer>().color = RandomColor(spikeEffectColors);
    }
    Color RandomColor(Color[] colors){
        int index = Random.Range(0, colors.Length);
        return colors[index];
    }

    private void WallCollision(Vector2 dir, Collision2D collision)
    {
        SpawnHitSpark(collision.GetContact(0).point);
    }
    private void SpikeCollision()
    {
        // SpawnSpikeSpark();
    }

    public void EnemyKill(Vector3 position)
    {

    }

    public void CollectableCollected(Vector3 position, Collectable.CollectableTypes c)
    {
        StartCoroutine(_SpawnSparkles(position));
    }
    IEnumerator _SpawnSparkles(Vector3 origin, int numSparkles = 2){
        Vector2 pos = origin;
        float angle;
        int index;
        float radius = .3f;
        index = Random.Range(0, pickupColors.Length);
        Instantiate(pickupSparkle, pos, Quaternion.identity).GetComponent<SpriteRenderer>().color = pickupColors[index];
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        for (int i = 0; i < numSparkles; i++)
        {
            angle = Mathf.Deg2Rad * Random.Range(0,360);
            pos = (Vector2)origin + new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            index = Random.Range(0, pickupColors.Length);
            Instantiate(pickupSparkle, pos, Quaternion.identity).GetComponent<SpriteRenderer>().color = pickupColors[index];
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }

    public void WallBreak(Vector3 position)
    {
        
    }
}
