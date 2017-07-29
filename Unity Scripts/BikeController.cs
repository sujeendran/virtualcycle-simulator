using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BikeController : MonoBehaviour {

	public WheelCollider Front;
	public WheelCollider Rear;
	public WheelCollider R1;
	public WheelCollider R2;
	
	public Text HUD;
	
	private Rigidbody rb;
	public Transform StartLoc;
	private Vector3 rLoc; 
	Quaternion rRot;
	
	float speed = 800.0f;
	public float braking = 400.0f;
	

	void Start () {
		rb = GetComponent<Rigidbody>();

		HUD.text= "Speed: 0 km/h";
		rLoc = StartLoc.position;
		rRot = transform.localRotation;
		R1.brakeTorque = 0;
		R2.brakeTorque = 0;
	}
	
	// Update is called once per frame
	void Update () {		
		//Respawn
		if(Input.GetKey(KeyCode.R)){
			transform.position=rLoc;
			transform.rotation=Quaternion.Slerp(transform.rotation,rRot,Time.deltaTime*40);
		}
	}

	void FixedUpdate()
	{
		Rear.motorTorque = speed * Input.GetAxis ("Vertical");
		Rear.brakeTorque = 0;
		
		float ang=Mathf.Round(Front.transform.position.y-Rear.transform.position.y);
		
		Front.steerAngle += Input.GetAxis ("Horizontal");//for steering
		rb.AddRelativeForce (Vector3.down * rb.velocity.magnitude);
		int s=(int)rb.velocity.magnitude*18/5;
		HUD.text= "Speed: "+ s +" km/hr\nBPM: 78";
		print (rb.rotation.eulerAngles+","+Front.steerAngle+","+ang);
		
		//Braking
		if (Input.GetKey (KeyCode.Space)) {
			Rear.brakeTorque = braking;
			print ("Braking "+ braking);
		}
	}

	void OnTriggerEnter (Collider col)
	{
		if(col.CompareTag("Checkpoint")){
			rLoc=col.transform.position;
			rRot=col.transform.rotation;
		}
	}
}