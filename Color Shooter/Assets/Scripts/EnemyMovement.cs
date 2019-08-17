using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    public float delay = 0;
    public float verticalSpeed;
    public float horizontalSpeed;
    public bool straight = true;

    /*
    public float dodge;
    public float smoothing;
    public Vector2 startWait;
    public Vector2 maneuverTime;
    public Vector2 maneuverWait;
    public Boundary boundary;

    private float currentSpeed;
    private float targetManeuver;
    */
    private Rigidbody2D rb;
    public Boundary boundary;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * verticalSpeed; // use negative to down the screen

        //currentSpeed = rb.velocity.y;
        //StartCoroutine(Evade());

        //StartCoroutine(MovementProcess());
    }

    void Update()
    {
        //GetComponent<Rigidbody2D>().position = new Vector2
        rb.position = new Vector2
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
        );
    }

    /*
    void FixedUpdate()
    {
        // For boundaries
        rb.position = new Vector2
        (
            Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
            //Mathf.Clamp(rb.position.y, boundary.yMin, boundary.yMax)
            rb.position.y
        );

        float newManeuver = Mathf.MoveTowards(rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
        rb.velocity = new Vector2(newManeuver, currentSpeed);

    }

    IEnumerator Evade()
    {
        yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

        while (true)
        {
            targetManeuver = Random.Range(1, dodge) * -Mathf.Sign(transform.position.x);
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
            targetManeuver = 0;
            yield return new WaitForSeconds(Random.Range(maneuverTime.x, maneuverTime.y));
        }
    }
    */

    IEnumerator MovementProcess()
    {
        // wait 
        yield return new WaitForSeconds(delay);
        rb.velocity = transform.right * horizontalSpeed; // positive to go right, negative to go left
        rb.velocity = transform.up * verticalSpeed; // use negative to down the screen
    }
}
