using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttack : MonoBehaviour {

    //public Transform target;

    public float rotateSpeed;
    public float speed;

    private Transform target;
    private Rigidbody2D rb;

	// Use this for initialization
	void Start ()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            target = playerObject.GetComponent<Transform>();
        }
        else
        {
            Debug.Log("Cannot find 'PlayerControl' script");
        }

        rb = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate()
    {
        if(target == null)
        {
            rb.velocity = -transform.up * speed;
            return;
        }
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmouont = Vector3.Cross(direction, transform.up).z;

        // different since front of enemy points down.
        rb.angularVelocity = rotateAmouont * rotateSpeed;
        rb.velocity = -transform.up * speed;

        //rb.angularVelocity = -rotateAmouont * rotateSpeed;
        //rb.velocity = transform.up * speed;
    }
}
