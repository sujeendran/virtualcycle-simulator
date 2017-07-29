using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using WindowsInput;

public class Acceleration : MonoBehaviour {

	SerialPort sp= new SerialPort("COM5",19200);
	public WheelCollider Front;
	public WheelCollider Rear;
	
	public WheelCollider R1;
	public WheelCollider R2;

	public Text HUD;

	private Rigidbody rb;
	public Transform StartLoc;
	private Vector3 rLoc; 
	Quaternion rRot;

	
	float speed = 50.0f;
	float braking = 0;
	//float turning = 20.0f;

//	int oldmin=-70;
//	int oldmax=70;
//	int newmin=1;
//	int newmax=5;
	//int oldrange,newrange;
	//int fSlope=0;

	float bpm=0;
	float oHandle=0,nHandle=0;
	float distance;
	float calories;
	
	// Use this for initialization
	void Start () {
		sp.Open ();
		sp.ReadTimeout = 1;
		distance = 0.0f;
		calories = 0.0f;
		rb = GetComponent<Rigidbody>();
		//oldrange=oldmax-oldmin; 
		//newrange=newmax-newmin;
		HUD.text= "Speed: 0 km/h\nDistance:\nBPM: 0\n";
		rLoc = StartLoc.position;
		rRot = transform.localRotation;
		R1.brakeTorque = 0;
		R2.brakeTorque = 0;
	}
	
	// Update is called once per frame
	void Update () {
//		RaycastHit hit;
//		Ray ray = new Ray(transform.position,Vector3.down);
//		if (Physics.Raycast (ray, out hit)) {
//			Vector3 hitn = hit.normal;    
//			int slope = (int)Vector3.Angle (hitn, Vector3.up);
//			Vector3 rf = tran11sform.forward;
//			int sign = (int)Mathf.Sign (Vector3.Dot (hitn, rf));
//			slope *= -sign;
//			fSlope = (((slope - oldmin) * newrange) / oldrange) + newmin;
//		}

		float ang=Mathf.Round(Front.transform.position.y-Rear.transform.position.y);

			if (sp.IsOpen) {
				try {
					string x = sp.ReadLine ();
					string[] inp= x.Split(',');
					
					float y = float.Parse (inp[0]); //speed
					
					nHandle = -float.Parse (inp[1]);   //steering

					braking=float.Parse (inp[2]);	  //braking

					bpm=float.Parse(inp[3]);		//pulse
					
					if((nHandle!=oHandle) && (Mathf.Abs(nHandle-oHandle)<=20))
					{	
					Front.steerAngle=nHandle;
					oHandle=nHandle;
					}
					int s=(int)rb.velocity.magnitude*18/5;
				distance+=s*Time.deltaTime/1000;
				calories=0.101f*distance*78f;
				HUD.text= "Speed: "+ s +" km/h\nDistance: "+distance.ToString("N1") +"km\nBPM: "+ bpm+"\nCalories: "+calories.ToString("F1");

					Rear.motorTorque = y * speed;				//to go forward

					if(braking>200){	
					Rear.brakeTorque = braking+6000;
					Rear.motorTorque=0;
					InputSimulator.SimulateKeyDown(VirtualKeyCode.SPACE);
					}
					else {
					Rear.brakeTorque=0;
					InputSimulator.SimulateKeyUp(VirtualKeyCode.SPACE);
					}
					
				print (y+","+Rear.motorTorque+","+Front.steerAngle+","+braking+","+ang);

				if(Input.GetKey(KeyCode.Q)|| ang > 0)
						sp.Write("q");
				if(Input.GetKey(KeyCode.Z)|| ang < 0)
					sp.Write("z");
				if(Input.GetKey(KeyCode.A)|| ang == 0)
					sp.Write("a");

				sp.BaseStream.Flush();

					} catch (System.Exception) {
				}
			}
			
			//Front.steerAngle = Input.GetAxis ("Horizontal") * turning;			//for steering
				
			//Braking
			if (Input.GetKey (KeyCode.Space)) {
				//print ("Braking");
				Rear.brakeTorque = braking;
			}	

			//Respawn
			if(Input.GetKey(KeyCode.R)){
				transform.position=rLoc;
				transform.rotation=Quaternion.Slerp(transform.rotation,rRot,Time.deltaTime*40);
				//transform.Rotate(0,rRot.y,0);
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