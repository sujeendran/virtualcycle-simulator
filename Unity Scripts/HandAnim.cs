using UnityEngine;
using System.Collections;

public class HandAnim : MonoBehaviour {

	public WheelCollider Rear;
	private bool isBraking = false;
	private float rTimer = 0.7f;
			
			
			// Use this for initialization
			void Start () {
				//GetComponent<Animation>().Play("RightHand_Idle");
			}
			
			// Update is called once per frame
			void Update () {

				//If we aren't braking, then we idle!
				if(isBraking == false)
				{
					GetComponent<Animation>().Play("LeftHand_Idle");
			        GetComponent<Animation>()["LeftHand_Idle"].wrapMode = WrapMode.Loop;
					//GetComponent<Animation>()["LeftHand_Idle"].speed = 0.5f;
				}
				//Braking SECTION
				if(Input.GetKeyDown(KeyCode.Space))
				{
					isBraking=true;
					GetComponent<Animation>().Play("LeftHand_IdleToBrake");
					GetComponent<Animation>()["LeftHand_IdleToBrake"].speed = 2.2f;	
				}
				
				if(Input.GetKeyUp(KeyCode.Space))
				{
					GetComponent<Animation>().Play("LeftHand_BrakeToIdle");
					GetComponent<Animation>()["LeftHand_BrakeToIdle"].speed = 1.5f;
					while(rTimer>0)
						{
						rTimer -= Time.deltaTime;
						}
				}			
				if(rTimer <= 0)
				{
					rTimer = 0.7f;  
					isBraking=false;
				}
	}
}