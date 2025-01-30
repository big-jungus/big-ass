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
    [SerializeField] GameObject pickupSparkleBig;
    [SerializeField] private GameObject hitSparkPrefab;

    private bool wasSetup;

    public void Setup()
    {
        PlayerManager.playerManager.playerController.CollisionOccured += WallCollision;
        PlayerManager.playerManager.playerController.CollectableCollected += CollectableCollected;
    }

    private void OnDestroy()
    {
        if (!wasSetup)
            return;

        PlayerManager.playerManager.playerController.CollisionOccured -= WallCollision;
        PlayerManager.playerManager.playerController.CollectableCollected -= CollectableCollected;
    }

    public void SpawnHitSpark(Vector3 position)
    {
        int index = Random.Range(0, hitCircleColors.Length);
        GameObject circle = Instantiate(hitCirclePrefab, position, Quaternion.identity);
        // circle.GetComponent<SpriteRenderer>().color = hitCircleColors[index];
        circle.GetComponent<Effect>().Spawn(transform.root, hitCircleColors[index]);

        GameObject shine = Instantiate(hitShinePrefab, position, Quaternion.identity);
        // shine.GetComponent<SpriteRenderer>().color = hitShineColors[index];
        shine.GetComponent<Effect>().Spawn(transform.root, hitShineColors[index]);
    }
    public void SpawnSpikeSpark(Vector3 position)
    {
        Instantiate(spikeEffectPrefab, position, Quaternion.identity).GetComponent<Effect>().Spawn(transform.root, RandomColor(spikeEffectColors));
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
        if(c == Collectable.CollectableTypes.SmallCoin)
            StartCoroutine(_SpawnSparkles(position));
        if(c == Collectable.CollectableTypes.BigCoin || c == Collectable.CollectableTypes.Win)
            StartCoroutine(_SpawnSparklesBig(position));
    }
    IEnumerator _SpawnSparkles(Vector3 origin, int numSparkles = 2){
        Vector2 pos = origin;
        float angle;
        int index;
        float radius = .3f;
        index = Random.Range(0, pickupColors.Length);
        Instantiate(pickupSparkle, pos, Quaternion.identity).GetComponent<Effect>().Spawn(transform.root, pickupColors[index]);
        yield return null;
        yield return null;
        yield return null;
        yield return null;
        for (int i = 0; i < numSparkles; i++)
        {
            angle = Mathf.Deg2Rad * Random.Range(0,360);
            pos = (Vector2)origin + new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            index = Random.Range(0, pickupColors.Length);
            Instantiate(pickupSparkle, pos, Quaternion.identity).GetComponent<Effect>().Spawn(transform.root, pickupColors[index]);
            yield return null;
            yield return null;
            yield return null;
            yield return null;
        }
    }
    IEnumerator _SpawnSparklesBig(Vector3 origin, int numSparkles = 3){
        Vector2 pos = origin;
        float angle;
        int index;
        float radius = .4f;
        index = Random.Range(0, pickupColors.Length);
        Instantiate(pickupSparkleBig, pos, Quaternion.identity).GetComponent<Effect>().Spawn(transform.root, pickupColors[index]);
        yield return null;
        yield return null;
        for (int i = 0; i < numSparkles; i++)
        {
            angle = Mathf.Deg2Rad * Random.Range(0,360);
            pos = (Vector2)origin + new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
            index = Random.Range(0, pickupColors.Length);
            Instantiate(pickupSparkleBig, pos, Quaternion.identity).GetComponent<Effect>().Spawn(transform.root, pickupColors[index]);
            yield return null;
            yield return null;
        }
    }

    public void WallBreak(Vector3 position)
    {
        
    }
}
