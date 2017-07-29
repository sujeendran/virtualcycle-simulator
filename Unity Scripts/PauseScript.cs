using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using WindowsInput;

public class PauseScript : MonoBehaviour {

	//SerialPort sp= new SerialPort("COM5",19200);
	//private int buttonWidth = 200;
	//private int buttonHeight = 50;
	//private int groupWidth = 200;
	//private int groupHeight = 170;

	void Start ()

	{
		//sCursor.lockState = CursorLockMode.Locked;

		Time.timeScale = 1;
		//sp.Open ();
			
	}


	bool paused = false;


	void OnGUI()
	{
		GUI.backgroundColor = Color.black;
		GUI.color = Color.white;
		//Cursor.lockState = CursorLockMode.Confined;
		//Screen.lockCursor = false;
		if (paused) 
		{

		//	sp.Write ("a");
			SceneManager.LoadScene ("Main Menu");
		/*GUI.BeginGroup (new Rect (((Screen.width / 2) - (groupWidth / 2)), ((Screen.height / 2) - (groupHeight / 2)), groupWidth, groupHeight));
			if(GUI.Button (new Rect(0,0,buttonWidth, buttonHeight),"Resume Game"))
			{
				//SceneManager.LoadScene (SceneManager.GetActiveScene().path);
				Time.timeScale = 1;
				paused = false;
			}

			if(GUI.Button (new Rect(0,60,buttonWidth,buttonHeight),"Main Menu"))
					{
				SceneManager.LoadScene ("Main Menu");
				//Application.LoadLevel(0);
					}
			//if (GUI.Button (new Rect (0, 120, buttonWidth, buttonHeight), "Quit Game")) {
			//	Application.Quit ();
			//}
			GUI.EndGroup ();*/
		}
	}

	void Update ()
	{
		if(Input.GetKeyUp (KeyCode.Escape))
			paused = togglePause();
	
	}

	bool togglePause()
	{
		if(Time.timeScale == 0)
		{	
			//Cursor.lockState = CursorLockMode.Confined;
			//Screen.lockCursor = false;
			Time.timeScale = 1;
			return(false);
		}
		else
		{
			Time.timeScale = 0;
			return(true);
		}
	}
}