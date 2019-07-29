using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour {
    public float delay = 0f;

    void Start()
    {
        Destroy(gameObject, delay);
    }
}
