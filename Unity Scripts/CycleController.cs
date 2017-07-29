using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using WindowsInput;


public class CycleController : MonoBehaviour {
	SerialPort sp= new SerialPort("COM5",9600);
	public WheelCollider Front;
	public WheelCollider Rear;
	
	public WheelCollider R1;
	public WheelCollider R2;
	
	public Text HUD;
	
	private Rigidbody rb;
	public Transform StartLoc;
	private Vector3 rLoc; 
	Quaternion rRot;
	
	
	float speed = 100.0f;
	float braking = 0;
	float bpm=0;
	float oHandle=0,nHandle=0;
	

	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 1;
		rb = GetComponent<Rigidbody>();
		HUD.text= "Speed: 0 km/h\nBPM: 0";
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
			//transform.Rotate(0,rRot.y,0);
		}	
	}

	void FixedUpdate(){
		float ang = Mathf.Round (Front.transform.position.y - Rear.transform.position.y);
		
		if (sp.IsOpen) {
			try {
				string x = sp.ReadLine ();
				string[] inp = x.Split (',');
				float y = Mathf.Round (float.Parse (inp [0])); //speed
				nHandle = -float.Parse (inp [1]);   //steering
				braking = float.Parse (inp [2]);	  //braking
				bpm = float.Parse (inp [3]);		//pulse
				
				if ((nHandle != oHandle) && (Mathf.Abs (nHandle - oHandle) <= 20)) {	
					Front.steerAngle = nHandle;
					oHandle = nHandle;
				}
				int s = (int)rb.velocity.magnitude * 18 / 5;
				rb.AddRelativeForce (Vector3.down * rb.velocity.magnitude);
				HUD.text = "Speed: " + s + " km/h\nBPM: " + bpm;
				
				Rear.motorTorque = y * speed;				//to go forward
				
				if (braking > 200) {	
					Rear.brakeTorque = braking + 6000;
					Rear.motorTorque = 0;
					InputSimulator.SimulateKeyDown (VirtualKeyCode.SPACE);
				} else {
					Rear.brakeTorque = 0;
					InputSimulator.SimulateKeyUp (VirtualKeyCode.SPACE);
				}
				
				print (y + "," + Front.steerAngle + "," + braking + "," + ang);
				
				if (Input.GetKey (KeyCode.A))
					sp.Write ("u");
				if (Input.GetKey (KeyCode.Z))
					sp.Write ("d");
				
				sp.BaseStream.Flush ();
				
				} catch (System.Exception) {
			}
		}

		if (Input.GetKey (KeyCode.Space)) {
			Rear.brakeTorque = braking;
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
