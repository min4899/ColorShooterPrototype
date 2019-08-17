using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Roomba enemy. Follows player if it is same color, otherwise move towards wall and move along wall in clockwise pattern.

public class Roomba : MonoBehaviour
{
    
    public Boundary boundary;
    public float speed;
    public float rotateSpeed;
    public float stayTime;

    //private Transform target;
    private Vector2 target;
    private Rigidbody2D rb;
    private bool track;

    //private Transform top;
    //private Transform right;
    //private Transform bottom;
    //private Transform left;

    private Vector2 top;
    private Vector2 right;
    private Vector2 bottom;
    private Vector2 left;
    private bool stay;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = -transform.up * speed; // use negative to down the screen
        track = false;
        stay = true;
        StartCoroutine(stayTimeCounter());
        //top = new GameObject().transform;
        //right = new GameObject().transform;
        //bottom = new GameObject().transform;
        //left = new GameObject().transform;

        //top.position = new Vector2(boundary.xMin, boundary.yMax);
        //right.position = new Vector2(boundary.xMax, boundary.yMax);
        //bottom.position = new Vector2(boundary.xMax, boundary.yMin);
        //left.position = new Vector2(boundary.xMin, boundary.yMin);

        top = new Vector2(boundary.xMin, boundary.yMax);
        right = new Vector2(boundary.xMax, boundary.yMax);
        bottom = new Vector2(boundary.xMax, boundary.yMin);
        left = new Vector2(boundary.xMin, boundary.yMin);
    }

    void Update()
    {
        if (stay)
        {
            rb.position = new Vector2
            (
                Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
            );
        }

        if(Player.instance != null) // player exists
        {
            // check if player is same color
            if(Player.instance.colorState == gameObject.GetComponent<Enemy>().colorState) // same color, set target to player
            {
                //target = Player.instance.GetComponent<Transform>();
                target = Player.instance.GetComponent<Transform>().position;
                track = true;
                //Debug.Log(target);
            }
            else // diff color, let roomba move foward until it hits boundary
            {
                // roomba is not at the edges, let it continue straight foward until it hits boundary
                if (rb.position.x != boundary.xMin && rb.position.x != boundary.xMax && rb.position.y != boundary.yMin && rb.position.y != boundary.yMax)
                {
                    track = false;
                    //rb.velocity = -transform.up * speed;
                }
                else // roomba is at the edges, set target to the edges
                {
                    track = true;
                    if(rb.position.x == boundary.xMin && rb.position.y == boundary.yMax) // top left corner
                    {
                        target = right;
                    }
                    else if(rb.position.x == boundary.xMax && rb.position.y == boundary.yMax) // top right corner
                    {
                        target = bottom;
                    }
                    else if (rb.position.x == boundary.xMax && rb.position.y == boundary.yMin) // bottom right corner
                    {
                        target = left;
                    }
                    else if (rb.position.x == boundary.xMin && rb.position.y == boundary.yMin) // bottom left corner
                    {
                        target = top;
                    }
                    else if(rb.position.x == boundary.xMin) // left eddge
                    {
                        target = top;
                    }
                    else if(rb.position.y == boundary.yMax) // top edge
                    {
                        target = right;
                    }
                    else if(rb.position.x == boundary.xMax) // right edge
                    {
                        target = bottom;
                    }
                    else if(rb.position.y == boundary.yMin) // bottom edge
                    {
                        target = left;
                    }
                    //Debug.Log(target);
                }
            }
        }
        else // player doesn't exist
        {
            // roomba is not at the edges, let it continue straight foward until it hits boundary
            if (rb.position.x != boundary.xMin && rb.position.x != boundary.xMax && rb.position.y != boundary.yMin && rb.position.y != boundary.yMax)
            {
                track = false;
            }
            else // roomba is at the edges, set target to the edges
            {
                track = true;
                if (rb.position.x == boundary.xMin && rb.position.y == boundary.yMax) // top left corner
                {
                    target = right;
                }
                else if (rb.position.x == boundary.xMax && rb.position.y == boundary.yMax) // top right corner
                {
                    target = bottom;
                }
                else if (rb.position.x == boundary.xMax && rb.position.y == boundary.yMin) // bottom right corner
                {
                    target = left;
                }
                else if (rb.position.x == boundary.xMin && rb.position.y == boundary.yMin) // bottom left corner
                {
                    target = top;
                }
                else if (rb.position.x == boundary.xMin) // left eddge
                {
                    target = top;
                }
                else if (rb.position.y == boundary.yMax) // top edge
                {
                    target = right;
                }
                else if (rb.position.x == boundary.xMax) // right edge
                {
                    target = bottom;
                }
                else if (rb.position.y == boundary.yMin) // bottom edge
                {
                    target = left;
                }
            }
        }
    }

    void FixedUpdate()
    {
        /*
        if (track)
        {
            if (target == null)
            {
                rb.velocity = transform.up * speed;
                return;
            }
            //Vector2 direction = (Vector2)target.position - rb.position;
            Vector2 direction = target - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            rb.angularVelocity = -rotateAmount * rotateSpeed;
            rb.velocity = transform.up * speed;
        }
        else
        {
            rb.angularVelocity = gameObject.transform.position.z;
            //rb.velocity = -transform.up * speed;
        }
        */
        if (track)
        {
            /*
            if (target == null)
            {
                rb.velocity = -transform.up * speed;
                return;
            }
            */
            Vector2 direction = target - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // different since front of enemy points down.
            rb.angularVelocity = rotateAmount * rotateSpeed;
            rb.velocity = -transform.up * speed;
        }
        else
        {
            rb.angularVelocity = gameObject.transform.position.z;
            rb.velocity = -transform.up * speed;
        }
    }

    IEnumerator stayTimeCounter()
    {
        yield return new WaitForSeconds(stayTime);
        stay = false;
    }

}
