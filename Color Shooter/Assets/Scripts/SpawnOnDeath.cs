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

                    if (GetComponent<FollowThePath2>() != null && parent.enabled == false) // if parent was using followthepath2
                    {
                        FollowThePath2 parent2 = gameObject.GetComponent<FollowThePath2>();
                        FollowThePath2 child2 = newObject.GetComponent<FollowThePath2>();
                        child.enabled = false;
                        child2.speed = parent2.speed;
                        child2.rotationByPath = parent2.rotationByPath;
                        child2.loop = parent2.loop;
                        child2.currentPathPercent = parent2.currentPathPercent;
                        child2.pathPositions = parent2.pathPositions;
                        child2.movingIsActive = parent2.movingIsActive;
                        //child2.delay = parent2.delay;
                        child2.delay = parent2.spawnOnDeathDelay;
                        return;
                    }

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
                    /*
                    if(GetComponent<FollowThePath2>() != null && parent.enabled == false) // if parent was using followthepath2
                    {
                        FollowThePath2 parent2 = gameObject.GetComponent<FollowThePath2>();
                        FollowThePath2 child2 = newObject.GetComponent<FollowThePath2>();
                        child.enabled = false;
                        child2.speed = parent2.speed;
                        child2.rotationByPath = parent2.rotationByPath;
                        child2.loop = parent2.loop;
                        child2.currentPathPercent = parent2.currentPathPercent;
                        child2.pathPositions = parent2.pathPositions;
                        child2.movingIsActive = parent2.movingIsActive;
                        child2.delay = parent2.delay;
                    }
                    */
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
