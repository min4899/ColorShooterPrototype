using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For enemeies that have multiple different shot spawns that will shoot at different intervals
public class EnemyShooting2 : MonoBehaviour
{
    [Tooltip("Object to be used as shots, use parent of multiple shots to shoot different types of shots.")]
    public GameObject shot;
    public string fireSound = "Enemy_Fire";
    [Tooltip("Select to shoot multiple types of shots. Use parent of multiple types of shots in 'shot' param")]
    public bool multipleShots = false;
    [Tooltip("How many shots is to be fired before switching to the next shot type")]
    public int multipleShotsBetween = 1;
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

    /*
    [Tooltip("Projectiles to be shot out upon death, see Enemy script for activation.")]
    public Transform[] deathSpawn;
    public GameObject deathShot;
    */

    private Transform player; // used for player aiming
    private GameObject currentShot;
    private int shotIndex;
    private int multipleShotsBetweenCounter;
    //private string fireSound;

    // Use this for initialization
    void Start()
    {
        /*
        if (gameObject.GetComponent<Enemy>() != null)
        {
            fireSound = gameObject.GetComponent<Enemy>().fireSound;
        }
        */
        if (multipleShots) // if using multiple shot types
        {
            shotIndex = 0;
            multipleShotsBetweenCounter = multipleShotsBetween;
            currentShot = shot.transform.GetChild(shotIndex).gameObject;
        }
        else // for only 1 shot type
        {
            currentShot = shot;
        }
        /*
        if (aim)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.GetComponent<Transform>();
            }
        }
        */
        if (burstFire)
        {
            StartCoroutine(BurstFire());
        }
        else
        {
            StartCoroutine(Fire());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (aim && Player.instance != null) // constantly update reference to player's location
        {
            player = Player.instance.GetComponent<Transform>();
            aimTransform.up = player.position - aimTransform.position;
        }
    }

    IEnumerator Fire()
    {
        yield return new WaitForSeconds(delay);
        while(gameObject != null)
        {
            /*
            for (int i = 0; i < shotSpawn.Length; i++)
            {
                for (int child = 0; child < shotSpawn[i].childCount; child++) // children shotspawns all shoot at the same time.
                {
                    Instantiate(shot, shotSpawn[i].GetChild(child).position, shotSpawn[i].GetChild(child).rotation);
                }
                yield return new WaitForSeconds(sequenceDelay); // wait before moving on to the next parent shotspawn
            }
            */
            for (int i = 0; i < shotSpawn.Length; i++)
            {
                if(shotSpawn[i].childCount > 0) // has child, spawn shots from the children's position
                {
                    AudioManager.instance.PlaySound(fireSound);
                    for (int child = 0; child < shotSpawn[i].childCount; child++) // children shotspawns all shoot at the same time.
                    {
                        Instantiate(currentShot, shotSpawn[i].GetChild(child).position, shotSpawn[i].GetChild(child).rotation);
                    }
                }
                else // no children, just spawn shots on the object itself
                {
                    AudioManager.instance.PlaySound(fireSound);
                    Instantiate(currentShot, shotSpawn[i].position, shotSpawn[i].rotation);
                }
                if(multipleShots)
                {
                    NextShot();
                }
                //yield return new WaitForSeconds(sequenceDelay);               
                if (i < shotSpawn.Length - 1) // dont wait for sequence delay if its the last element
                {
                    yield return new WaitForSeconds(sequenceDelay); // wait before moving on to the next parent shotspawn
                }
                
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
                if (i < burstSize - 1) // dont add delay for last burst
                {
                    yield return new WaitForSeconds(burstDelay);
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    /*
    public void FireUponDeath()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < deathSpawn.Length; i++)
        {
            Instantiate(shot, deathSpawn[i].position, deathSpawn[i].rotation);
        }
    }
    */

    void NextShot()
    {
        multipleShotsBetweenCounter--;
        if (multipleShotsBetweenCounter <= 0)
        {
            multipleShotsBetweenCounter = multipleShotsBetween;
            shotIndex++;
            if (shotIndex >= shot.transform.childCount)
            {
                shotIndex = 0;
            }
            currentShot = shot.transform.GetChild(shotIndex).gameObject;
        }
    }
}
