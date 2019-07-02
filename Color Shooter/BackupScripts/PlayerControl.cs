using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerControl : MonoBehaviour {

    public float speed;
    public Boundary boundary;
    public Sprite red;
    public Sprite yellow;
    public GameObject redShot;
    public GameObject yellowShot;
    public GameObject shield;

    private GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float invincibleTime;
    public float vulernableTime;

    private bool shieldOn;
    private float currInvincibleTime;
    private float currVulnerableTime;
    private float nextFire;

    void Start()
    {
        shieldOn = true;
    }

    void Update()
    {
        if (shot != null && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play();
        }
    }

    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        GetComponent<Rigidbody2D>().position = new Vector2
        (
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
        );
    }

    // Short Period of Invincibility after getting hit with shield on.
    IEnumerator Invincible()
    {
        // Invincibility is on.
        GetComponent<CapsuleCollider2D>().enabled = false;
        shield.SetActive(false);
        shieldOn = false;
        //Debug.Log("Invincibility on.");
        currInvincibleTime = invincibleTime * 10; // use 10 * seconds of invincible time to get "tenth of a second" unit.
        bool activeSprite = true;
        while(currInvincibleTime > 0)
        {
            // flicker affect
            if(activeSprite) // sprite is on
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                activeSprite = false;
            }
            else // sprite is off
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                activeSprite = true;
            }
            yield return new WaitForSeconds(0.1f); // Each loops is 0.1 seconds (tenth of a second).
            currInvincibleTime--;
        }
        //Debug.Log("Invincibility off.");
        GetComponent<CapsuleCollider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // Invincibility off. Must wait additional seconds before shield can get back up.
        currVulnerableTime = vulernableTime; // use regular seconds
        while (currVulnerableTime > 0)
        {
            yield return new WaitForSeconds(1f); // each second
            currVulnerableTime--;
        }
        shield.SetActive(true);
        shieldOn = true;
    }

    public void ChangeToRed()
    {
        GetComponent<SpriteRenderer>().sprite = red;
        shot = redShot;
        StartCoroutine(Invincible());
    }

    public void ChangeToYellow()
    {
        GetComponent<SpriteRenderer>().sprite = yellow;
        shot = yellowShot;
        StartCoroutine(Invincible());
    }

    public bool CheckShield()
    {
        return shieldOn;
    }
}
