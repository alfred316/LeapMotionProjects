using UnityEngine;
using System.Collections;
using Leap;

public class shooterObject : MonoBehaviour {

	//create the variables that will be used
	Controller controller;
	public Transform bullet;
	public AudioSource reloadSound;
	public AudioSource shootSound;

	public static int bulletCount = 6;

	// Use this for initialization
	void Start () {

		//create an instance of the controller object
		controller = new Controller();


		//enable the gestures that will be used
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);
		controller.EnableGesture(Gesture.GestureType.TYPEINVALID);
	}
	
	// Update is called once per frame
	void Update () {

		//this was for testing the shooter without the motion
		/*
		if(Input.GetKeyDown("space"))
		{
			shootBullet();
		}
		*/

		//create an instance of the frame object
		//this is used to grab data from the motion sensor frame by frame
		//which is why it's in our update function
		//for example, the gesture data
		Frame frame = controller.Frame();
		foreach (Gesture gesture in frame.Gestures())
		{
			switch(gesture.Type)
			{
		
			case(Gesture.GestureType.TYPEINVALID):
			{
				Debug.Log("Invalid gesture recognized.");
				break;
			}
		
			//a simple motion of tapping at th screen
			case(Gesture.GestureType.TYPESCREENTAP):
			{
				Debug.Log("Screen tap gesture recognized.");
				if (bulletCount > 0){
				shootBullet();
				}
				break;
			}
			
			//a swipe of the finger or pointer in any direction
			case(Gesture.GestureType.TYPESWIPE):
			{
				Debug.Log("Swipe gesture recognized.");
				reloadSound.Play();
				bulletCount = 6;
				break;
			}
	
			default:
			{
				break;
			}
			}
		}

	}

	//function used to instantiate the bullet object and shoot it out of the crosshairs
	public void shootBullet(){
		
		Instantiate(bullet, transform.position, transform.rotation);
		shootSound.Play();
		bulletCount -= 1;
		
	}

	//GUI to display data such as ammo and score
	//also some game instructions
	void OnGUI () {

		//convert the count of bullets to a string so as to be able to display it
		string bulletName = bulletCount.ToString();

		// Make a background box
		GUI.Box(new Rect(45,30,50,25), "Ammo");
		GUI.Box(new Rect(10,10,100,40), bulletName);
		
		//game instructions
		GUI.Box(new Rect(900,40,300,25), "Shoot the thingies!");
		GUI.Box(new Rect(900,60,300,25), "Screen Tap to shoot");
		GUI.Box(new Rect(900,20,300,25), "Swipe to reload");
		GUI.Box(new Rect(900,80,300,25), "Have fun!");
	}
}
