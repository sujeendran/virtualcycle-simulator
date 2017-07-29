using UnityEngine;
using System.Collections;
using System.IO.Ports;

public class CycleSteer : MonoBehaviour {
//	float rotspeed=20.0f;
	public WheelCollider Front;
	Quaternion origRotation;
	float rotation;
	// Use this for initialization
	void Start () {
		origRotation = transform.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		//GameObject cycle = GameObject.Find ("3dCYCLEMODEL");
		//hand2=cycle.GetComponent<Acceleration>().;
		rotation = Front.steerAngle/20.0f;
		//print ("Steer Angle=" + Front.steerAngle);
		//float rotation= Input.GetAxis("Horizontal")*rotspeed* Time.deltaTime;
		//float rotation= hand2;//*rotspeed* Time.deltaTime;
		transform.Rotate(0,0,rotation);
		transform.localRotation = Quaternion.Slerp(transform.localRotation, origRotation, Time.deltaTime);
	}
}

