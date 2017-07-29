﻿using UnityEngine;
using System.Collections;

public class WheelRot : MonoBehaviour {
	public WheelCollider Rear;
	float speed=5.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.right, speed * Rear.rpm * Time.deltaTime);
	}
}
