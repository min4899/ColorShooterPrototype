using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour {

    public GameObject[] BossComponents;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        int partsLeft = 0;
        for(int i = 0; i < BossComponents.Length; i++)
        {
            if (BossComponents[i] != null)
                partsLeft++;
        }
        if (partsLeft <= 0)
            Destroy(gameObject);
	}
}
