using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform t;
    [SerializeField] Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    void Update()
    {
        t.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10) ;
    }
}
