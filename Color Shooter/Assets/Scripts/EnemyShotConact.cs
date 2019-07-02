using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShotConact : MonoBehaviour {

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boundary") || other.CompareTag("Enemy") || other.CompareTag("EnemyRedShot") || other.CompareTag("EnemyYellowShot") || other.CompareTag("PlayerRedShot") || other.CompareTag("PlayerYellowShot"))
        {
            return;
        }

    }
}
