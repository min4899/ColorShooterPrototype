using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour {

    public float rotationSpeed;
	
	// Update is called once per frame
	void Update () {
        //transform.Rotate(Vector3.up * (rotationSpeed * Time.deltaTime));
        transform.Rotate(Vector3.forward * (rotationSpeed * Time.deltaTime));
    }
}
