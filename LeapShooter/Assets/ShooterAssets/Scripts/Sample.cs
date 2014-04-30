using UnityEngine;
using System.Collections;
using Leap;

public class Sample : MonoBehaviour {

	Controller controller;

	// Use this for initialization
	void Start () {

		//create sample listener and controller
		controller = new Controller();
	
	}
	
	// Update is called once per frame
	void Update () {
	
		//get the most recent frame information
		Frame frame = controller.Frame();
		Debug.Log ("Frame id: " + frame.Id
		           + ", timestamp: " + frame.Timestamp
		           + ", hands: " + frame.Hands.Count
		           + ", fingers: " + frame.Fingers.Count
		           + ", tools: " + frame.Tools.Count);

		if (!frame.Hands.IsEmpty){
			//get the first hand
			Hand hand = frame.Hands[0];

			//check for fingers
			FingerList fingers = hand.Fingers;
			if (!fingers.IsEmpty){
				//calculate fingers average finger tip position position
				Vector avgPos = Vector.Zero;
				foreach (Finger finger in fingers){
					avgPos += finger.TipPosition;
				}

				avgPos /= fingers.Count;
				Debug.Log ("Hand has " + fingers.Count
				           + " fingers, average finger tip position: " + avgPos);
			}

			// Get the hand's sphere radius and palm position
			Debug.Log("Hand sphere radius: " + hand.SphereRadius.ToString("n2")
			              + " mm, palm position: " + hand.PalmPosition);

			//get the hand's Normal and Direction using Vector
			Vector normal = hand.PalmNormal;
			Vector direction = hand.Direction;

			// Calculate the hand's pitch, roll, and yaw angles
			Debug.Log("Hand pitch: " + direction.Pitch 
			              + "roll: " + normal.Roll 
			              + "yaw: " + direction.Yaw );

		}
	}

	public void OnConnect (Controller controller){

		Debug.Log ("is connected!");

		controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
		controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
		controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

	}
}
