using UnityEngine;
using System.Collections;
using Leap;

public class CrossHairScript : MonoBehaviour {
	public float chDist = 10;

	// Declare variables here
	Controller controller;
	private Camera mainCam;
	private LeapManager leapManager;

	// Use this for initialization
	void Start () {
		mainCam = (GameObject.FindGameObjectWithTag("MainCamera")as GameObject).GetComponent(typeof(Camera)) as Camera;
		leapManager = (GameObject.Find("LeapManager")as GameObject).GetComponent(typeof(LeapManager)) as LeapManager;
		controller = new Controller();
	}
	
	// Update is called once per frame
	void Update () {
	
		if (leapManager != null){

			if (leapManager.pointerAvailible){

				leapManager.screenToWorldDistance = chDist;
				this.transform.position = leapManager.pointerPositionScreenToWorld;
				if (!renderer.enabled) renderer.enabled = true;

			}
			else
				renderer.enabled = false;

		}
				
	}
	
	//leap manager has a leap controller called _leapController
	
	
	
	
}
