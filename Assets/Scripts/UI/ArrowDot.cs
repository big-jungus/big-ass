using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowDot : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Color> sprites = new List<Color>();
    [SerializeField] private float delay;
    [SerializeField] private float offset;
    private Coroutine flashRoutine;

    private void Start()
    {
        PlayerManager.playerManager.playerController.Charging += CheckForMaxCharge;
        PlayerManager.playerManager.playerController.ChargeEnded += Released;
    }

    private void OnDestroy()
    {
        PlayerManager.playerManager.playerController.Charging -= CheckForMaxCharge;
        PlayerManager.playerManager.playerController.ChargeEnded -= Released;
    }

    private void CheckForMaxCharge(float currentCharge)
    {
        if (currentCharge == PlayerManager.playerManager.playerStats.maxChargeDuration)
        {
            if (flashRoutine == null)
                flashRoutine = StartCoroutine(Animation());
        }
    }

    private void Released()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        spriteRenderer.color = sprites[0];
    }

    private IEnumerator Animation()
    {
        float currentTime = 0f;
        int spriteCounter = 0;

        yield return new WaitForSeconds(offset);

        while (true)
        {
            currentTime = 0f;
            while (currentTime < delay)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }

            spriteCounter++;
            if (spriteCounter >= sprites.Count)
                spriteCounter = 0;

            spriteRenderer.color = sprites[spriteCounter];
        }
    }
}
