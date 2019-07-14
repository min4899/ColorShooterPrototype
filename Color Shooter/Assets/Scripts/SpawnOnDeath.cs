using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour {

    [Tooltip("If the object will spawn relative to parent object's position")]
    public bool spawnOnParent;
    [Tooltip("the object will follow the parent's original path and movement")]
    public bool followParentPath;
    public GameObject[] spawnItems;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void Spawn()
    {
        foreach (GameObject item in spawnItems)
        {
            if(spawnOnParent)
            {
                Vector2 parentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                GameObject newObject = Instantiate(item, parentPosition, gameObject.transform.rotation);
                if(followParentPath)
                {
                    FollowThePath parent = gameObject.GetComponent<FollowThePath>();
                    FollowThePath child = newObject.GetComponent<FollowThePath>();
                    child.nextMovement = parent.nextMovement;
                    child.nextMoveDelay = parent.nextMoveDelay;
                    child.nextMoveSpeed = parent.nextMoveSpeed;
                    child.scaleX = parent.scaleX;
                    child.scaleY = parent.scaleY;
                    child.speed = parent.speed;
                    child.rotationByPath = parent.rotationByPath;
                    child.loop = parent.loop;
                    child.currentPathPercent = parent.currentPathPercent;
                    child.pathPositions = parent.pathPositions;
                    child.movingIsActive = parent.movingIsActive;
                }
            }
            else
            {
                GameObject newObject = Instantiate(item);
                if(newObject.GetComponent<Wave>() != null) // if the object is a wave object
                {
                    newObject.GetComponent<Wave>().Activate();
                }
            }
        }
    }
}
