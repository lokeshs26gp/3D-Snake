using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    public Transform rotationPoint;
    [Range(0, 100)]
    public float speed = 1.0f;
    public Vector3 rotationAxis;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //transform.Rotate(Vector3.forward*speed);
        transform.RotateAround(rotationPoint.position, rotationAxis, speed);
	}
}
