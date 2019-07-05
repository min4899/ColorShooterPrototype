using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour {

    [Tooltip("If the object will spawn relative to parent object's position")]
    public bool spawnOnParent;

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
                Instantiate(item, parentPosition, gameObject.transform.rotation);
            }
            else
            {
                Instantiate(item);
            }
        }
    }
}
