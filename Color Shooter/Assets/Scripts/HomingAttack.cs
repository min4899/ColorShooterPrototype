using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Points the bottom of the object to the player.
public class HomingAttack : MonoBehaviour {

    //public Transform target;

    public float rotateSpeed;
    public float speed;

    [Tooltip("Enemy will track players for a set time if enabled.")]
    public bool limit;
    public float timer;

    private Transform target;
    private Rigidbody2D rb;
    private bool track = true;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody2D>();

        if (Player.instance != null)
        {
            target = Player.instance.GetComponent<Transform>();
        }
    }

    void Update()
    {
        if (limit)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                track = false;
            }
        }
    }
	
	void FixedUpdate()
    {
        if (track)
        {
            if (target == null)
            {
                rb.velocity = -transform.up * speed;
                return;
            }
            Vector2 direction = (Vector2)target.position - rb.position;
            direction.Normalize();
            float rotateAmount = Vector3.Cross(direction, transform.up).z;

            // different since front of enemy points down.
            rb.angularVelocity = rotateAmount * rotateSpeed;
            rb.velocity = -transform.up * speed;

            //rb.angularVelocity = -rotateAmount * rotateSpeed;
            //rb.velocity = transform.up * speed;
        }
        else
        {
            rb.angularVelocity = gameObject.transform.position.z;
            //rb.velocity = -transform.up * speed;
        }
    }
}
