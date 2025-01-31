using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Vector2 currentMovementConstraints;
    [SerializeField] private Transform target;
    [SerializeField] private float currentMovementSpeed;

    private Vector2 startingPosition;

    private void Start()
    {
        startingPosition = target.localPosition;
        StartCoroutine(ShakeAnimation());
    }

    public void LoadLevel(int levelIndex)
    {
        PlayerManager.playerManager.levelManager.LoadLevel(levelIndex);
    }

    private IEnumerator ShakeAnimation()
    {
        while (true)
        {
            yield return MovePosition();
        }
    }

    private IEnumerator MovePosition()
    {
        Vector2 randLocation = new Vector2(Random.Range(-currentMovementConstraints.x, currentMovementConstraints.x), Random.Range(-currentMovementConstraints.y, currentMovementConstraints.y)) + startingPosition;
        Vector2 currLocation = transform.localPosition;

        float currentTime = 0f;
        while (currentTime < 1f)
        {
            currentTime += Time.deltaTime * currentMovementSpeed;
            yield return null;

            target.localPosition = Vector2.Lerp(currLocation, randLocation, currentTime);
        }
    }
}
