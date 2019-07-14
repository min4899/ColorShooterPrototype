﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour {

    public GameObject shot;
    [Tooltip("Enemy will aim its shot at player's location.")]
    public bool aim;
    [Tooltip("The shotSpawn/parent of shotSpawns that represents the general direction.")]
    public Transform aimTransform;
    public Transform[] shotSpawn;

    //public Transform shotSpawn;
    [Tooltip("Time that enemy won't fire between shots/burst shots")]
    public float fireRate;
    [Tooltip("Start shooting only after a delay time")]
    public float delay;

    public bool burstFire;
    public int burstSize;
    public float burstDelay;

    [Tooltip("Projectiles to be shot out upon death, see Enemy script for activation.")]
    public Transform[] deathSpawn;
    public GameObject deathShot;

    private Transform player; // used for player aiming

    // Use this for initialization
    void Start()
    {
        if(aim)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Transform>();
            }
        }
        if(burstFire)
        {
            StartCoroutine(BurstFire());
        }
        else
        {
            InvokeRepeating("Fire", delay, fireRate);
        }
        //InvokeRepeating("Fire", delay, fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        if (aim && player != null)
        {
            aimTransform.up = player.position - aimTransform.position;
        }
    }

    void Fire()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for(int i = 0; i < shotSpawn.Length; i++)
        {
            Instantiate(shot, shotSpawn[i].position, shotSpawn[i].rotation);
        }
    }

    IEnumerator BurstFire()
    {
        yield return new WaitForSeconds(delay);
        while(gameObject != null)
        {
            for (int i = 0; i < burstSize; i++)
            {
                Fire();
                yield return new WaitForSeconds(burstDelay);
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    public void FireUponDeath()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < deathSpawn.Length; i++)
        {
            Instantiate(shot, deathSpawn[i].position, deathSpawn[i].rotation);
        }
    }
}