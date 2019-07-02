using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDeath : MonoBehaviour {

    public GameObject[] spawnItems;

    [Tooltip("If the object will spawn relative to parent object's position")]
    public bool spawnOnParent;

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
                Vector3 parentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                Instantiate(item, parentPosition, gameObject.transform.rotation);
            }
            else
            {
                Instantiate(item);
            }
        }
    }
}
