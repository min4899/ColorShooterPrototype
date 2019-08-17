using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MopMovement : MonoBehaviour {
    public float speed;
    public float hangingTime;
    public float waitTime;
    public float exposedTime;

    public Transform[] shotSpawn;
    public GameObject shot;

    private bool tracking;
    private Vector2 coordinate;
    private float step;
    private bool attack;
    private float phase;

    private float phase1 = 4.75f;
    private float phase2 = -1.25f;

	// Use this for initialization
	void Start () {
        tracking = true;
        StartCoroutine(Attack());
	}
	
	// Update is called once per frame
	void Update () {
	}
    void FixedUpdate()
    {
        step = speed * Time.deltaTime;
        if (tracking && Player.instance != null) // track
        {
            //step = speed * Time.deltaTime;
            coordinate = Vector2.MoveTowards(transform.position, Player.instance.transform.position, step);
            coordinate = new Vector2(coordinate.x, 6f);
            transform.position = coordinate;
        }
        else if(attack) // slam down
        {
            Vector2 target = new Vector2(coordinate.x, phase);
            if(phase == phase1)
                transform.position = Vector2.MoveTowards(transform.position, target, step/2);
            else
                transform.position = Vector2.MoveTowards(transform.position, target, step);
        }
        else // move back up
        {
            Vector2 target = new Vector2(coordinate.x, 6f);
            transform.position = Vector2.MoveTowards(transform.position, target, step/4);
        }
    }

    IEnumerator Attack()
    {
        while (gameObject != null)
        {
            yield return new WaitForSeconds(hangingTime);
            tracking = false;
            attack = true;
            //phase = 4.75f;
            phase = phase1;
            yield return new WaitForSeconds(waitTime);
            //phase = -1.25f;
            phase = phase2;
            yield return new WaitForSeconds(0.75f);
            Fire();
            yield return new WaitForSeconds(exposedTime);
            attack = false;
            yield return new WaitForSeconds(3.5f);
            tracking = true;
        }
    }

    //slam down attack
    void Fire()
    {
        for (int i = 0; i < shotSpawn.Length; i++)
        {
            AudioManager.instance.PlaySound("Enemy_Explosion");
            Instantiate(shot, shotSpawn[i].position, shotSpawn[i].rotation);
        }
    }
}
