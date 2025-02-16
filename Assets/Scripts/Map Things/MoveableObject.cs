using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveableObject : MonoBehaviour
{
    [SerializeField] private List<Vector3> pathNodes = new List<Vector3>();
    [SerializeField] private float timePerNode;
    private int currentNode;
    private bool direction;

    [SerializeField] private bool isActive = true;
    private Coroutine movementRoutine;

    private void Start()
    {
        currentNode = 1; // Should start moving towards 2nd node bc first node is starting point
        direction = true;

        if (isActive)
            movementRoutine = StartCoroutine(MovementRoutine());
    }

    private IEnumerator MovementRoutine()
    {
        Vector3 startingPosition;
        //float segmentTime = totalTravelDuration / pathNodes.Count;
        float currentTime;

        while (true)
        {
            startingPosition = transform.position;
            currentTime = 0;

            while (currentTime < timePerNode)
            {
                yield return null;
                currentTime += Time.deltaTime;

                transform.position = Vector3.Lerp(startingPosition, pathNodes[currentNode], currentTime / timePerNode);
            }

            if (direction)
            {
                currentNode++;
                if (currentNode >= pathNodes.Count)
                {
                    currentNode = pathNodes.Count - 1;
                    direction = false;
                }
            }
            else
            {
                currentNode--;
                if (currentNode < 0)
                {
                    currentNode = 0;
                    direction = true;
                }
            }
        }
    }

    public void SwitchActivated()
    {
        if (isActive)
        {
            if (movementRoutine != null)
                StopCoroutine(movementRoutine);
        }
        else
        {
            movementRoutine = StartCoroutine(MovementRoutine());
        }

        isActive = !isActive;
    }
}
