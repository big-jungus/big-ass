using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWall : MonoBehaviour
{
    [SerializeField] private float minVelocity;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (PlayerManager.playerManager.playerController.rb.velocity.magnitude >= minVelocity)
                DestroyWall();
        }
    }

    private void DestroyWall()
    {
        Destroy(this.gameObject);
    }
}
