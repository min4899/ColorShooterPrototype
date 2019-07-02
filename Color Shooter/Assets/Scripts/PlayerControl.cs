using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerControl : MonoBehaviour {
    /*
    public float speed;
    public Boundary boundary;
    private GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    public float invincibleTime;
    public float vulernableTime;
    [Tooltip("no color = 0, red = 1, yellow = 2, green = 3")]
    [Range(0, 3)]
    public int colorState;

    public Sprite red;
    public Sprite yellow;
    public Sprite green;
    public GameObject redShot;
    public GameObject yellowShot;
    public GameObject greenShot;
    public GameObject shield;
    public Material whiteMaterial;

    private bool shieldOn;
    private float currInvincibleTime;
    private float currVulnerableTime;
    private float nextFire;    
    private Material defaultMaterial;
    */

    public float speed;
    public Boundary boundary;
    //for mobile control
    private float deltaX;
    private float deltaY;
    private int moveTouchID;

    /*
    void Start()
    {
        shieldOn = true;
        defaultMaterial = gameObject.GetComponent<SpriteRenderer>().material;

        // for testing purposes
        if (colorState == 1)
        {
            GetComponent<SpriteRenderer>().sprite = red;
            shot = redShot;
        }
        else if(colorState == 2)
        {
            GetComponent<SpriteRenderer>().sprite = yellow;
            shot = yellowShot;
        }
        else if(colorState == 3)
        {
            GetComponent<SpriteRenderer>().sprite = green;
            shot = greenShot;
        }
    }
    */

    void Update()
    {
        /*
        if (shot != null && Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            //GetComponent<AudioSource>().Play();
        }
        */
        
        // For PC controls
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        GetComponent<Rigidbody2D>().position = new Vector2
        (
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
        );
        
        // For mobile controls
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            //Debug.Log("ID:" + touch.fingerId);
            if (touch.fingerId == 0)
            {
                Vector2 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        deltaX = touchPosition.x - transform.position.x;
                        deltaY = touchPosition.y - transform.position.y;
                        break;

                    case TouchPhase.Moved:
                        Vector2 newPosition = new Vector2(touchPosition.x - deltaX, touchPosition.y - deltaY);
                        newPosition = new Vector2
                        (
                            Mathf.Clamp(newPosition.x, boundary.xMin, boundary.xMax),
                            Mathf.Clamp(newPosition.y, boundary.yMin, boundary.yMax)
                        );
                        GetComponent<Rigidbody2D>().MovePosition(newPosition);

                        // if player position is at edge, readjust the delta
                        if (transform.position.x <= boundary.xMin || transform.position.x >= boundary.xMax || transform.position.y <= boundary.yMin || transform.position.y >= boundary.yMax)
                        {
                            deltaX = touchPosition.x - transform.position.x;
                            deltaY = touchPosition.y - transform.position.y;
                        }
                        break;

                    case TouchPhase.Ended:
                        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                        break;
                }
            }
            //Debug.Log(transform.position);
        }
    }

    /*
    // Short Period of Invincibility after getting hit with shield on.
    IEnumerator Invincible()
    {
        // Invincibility is on.
        GetComponent<CapsuleCollider2D>().enabled = false;
        //shieldOn = false;
        shield.SetActive(false);
        //Debug.Log("Invincibility on.");
        currInvincibleTime = invincibleTime * 10; // use 10 * seconds of invincible time to get "tenth of a second" unit.
        bool activeSprite = true;
        while(currInvincibleTime > 0)
        {
            // flicker affect
            if(activeSprite) // sprite is on
            {
                //gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.3f);
                gameObject.GetComponent<SpriteRenderer>().material = whiteMaterial;
                activeSprite = false;
            }
            else // sprite is off
            {
                //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;
                activeSprite = true;
            }
            yield return new WaitForSeconds(0.1f); // Each loops is 0.1 seconds (tenth of a second).           
            currInvincibleTime--;
        }
        //Debug.Log("Invincibility off.");
        GetComponent<CapsuleCollider2D>().enabled = true;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<SpriteRenderer>().material = defaultMaterial;

        // Invincibility off. Must wait additional seconds before shield can get back up.
        shieldOn = false;
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
        //StartCoroutine(Invincible());
        GetComponent<SpriteRenderer>().sprite = red;
        shot = redShot;
        colorState = 1;
        StartCoroutine(Invincible());
    }

    public void ChangeToYellow()
    {
        //StartCoroutine(Invincible());
        GetComponent<SpriteRenderer>().sprite = yellow;
        shot = yellowShot;
        colorState = 2;
        StartCoroutine(Invincible());
    }

    public void ChangeToGreen()
    {
        //StartCoroutine(Invincible());
        GetComponent<SpriteRenderer>().sprite = green;
        shot = greenShot;
        colorState = 3;
        StartCoroutine(Invincible());
    }

    public bool CheckShield()
    {
        return shieldOn;
    }
    */
}
