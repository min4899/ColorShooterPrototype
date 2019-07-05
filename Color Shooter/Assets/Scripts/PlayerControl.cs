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

    //for mobile control
    private float deltaX;
    private float deltaY;
    private int moveTouchID;

    void Start()
    {
        Input.simulateMouseWithTouches = false;
    }

    void Update()
    {
        /*
        // For PC keyboard controls
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(moveHorizontal, moveVertical);
        GetComponent<Rigidbody2D>().velocity = movement * speed;

        GetComponent<Rigidbody2D>().position = new Vector2
        (
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(GetComponent<Rigidbody2D>().position.y, boundary.yMin, boundary.yMax)
        );
        */

#if UNITY_STANDALONE || UNITY_EDITOR    //if the current platform is not mobile, setting mouse handling 
        // For Pc mouse controls
        if (Input.GetMouseButton(0)) //if mouse button was pressed       
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); //calculating mouse position in the worldspace
            mousePosition = new Vector2
            (
                Mathf.Clamp(mousePosition.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(mousePosition.y, boundary.yMin, boundary.yMax)
            );
            transform.position = Vector2.MoveTowards(transform.position, mousePosition, 15 * Time.deltaTime);
        }
#endif

#if UNITY_IOS || UNITY_ANDROID //if current platform is mobile, 
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
#endif
        }
    }
}
