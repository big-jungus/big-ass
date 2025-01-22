using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthHeart : MonoBehaviour
{
    public void RemoveHeart()
    {
        Destroy(this.gameObject);
    }
}
