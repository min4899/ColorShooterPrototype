using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting2 : MonoBehaviour
{

    public GameObject shot;
    [Tooltip("Enemy will aim its shot at player's location.")]
    public bool aim;
    [Tooltip("The shotSpawn/parent of shotSpawns that represents the general direction.")]
    public Transform aimTransform;
    [Tooltip("Should use parent of multiple shotspawns for sequence shots. Children of a parent will shoot at the same time.")]
    public Transform[] shotSpawn;

    //public Transform shotSpawn;
    [Tooltip("Time that enemy won't fire between shots/burst shots")]
    public float fireRate;
    [Tooltip("Start shooting only after a delay time")]
    public float delay;

    [Tooltip("Time delay between each sequence shots")]
    public float sequenceDelay;

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
        if (aim)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Transform>();
            }
        }
        if (burstFire)
        {
            StartCoroutine(BurstFire());
        }
        else
        {
            //InvokeRepeating("Fire", delay, fireRate);
            StartCoroutine(Fire());
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

    /*
    void Fire()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < shotSpawn.Length; i++)
        {
            Instantiate(shot, shotSpawn[i].position, shotSpawn[i].rotation);
        }
    }
    */

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(delay);
        while(gameObject != null)
        {
            for (int i = 0; i < shotSpawn.Length; i++)
            {
                for (int child = 0; child < shotSpawn[i].childCount; child++) // children shotspawns all shoot at the same time.
                {
                    Instantiate(shot, shotSpawn[i].GetChild(child).position, shotSpawn[i].GetChild(child).rotation);
                }
                yield return new WaitForSeconds(sequenceDelay); // wait before moving on to the next parent shotspawn
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    IEnumerator BurstFire()
    {
        yield return new WaitForSeconds(delay);
        while (gameObject != null)
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
