using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform t;
    Transform playerTransform;
    // Start is called before the first frame update
    public void Setup()
    {
        t = transform;
        playerTransform = PlayerManager.playerManager.playerObj.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform == null)
        {
            playerTransform = PlayerManager.playerManager.playerObj.transform;
            return;
        }

        t.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10) ;
    }
}
