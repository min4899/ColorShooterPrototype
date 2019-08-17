using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireOnDeath : MonoBehaviour {

    [Tooltip("Projectiles to be shot out upon death, see Enemy script for activation.")]
    public Transform[] deathSpawn;
    public GameObject deathShot;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FireUponDeath()
    {
        //Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
        //GetComponent<AudioSource>().Play();
        for (int i = 0; i < deathSpawn.Length; i++)
        {
            Instantiate(deathShot, deathSpawn[i].position, deathSpawn[i].rotation);
        }
    }
}
