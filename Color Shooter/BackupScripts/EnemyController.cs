using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float delay;

    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Fire", delay, fireRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Fire()
    {
        Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
    }
}
