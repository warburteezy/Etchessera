using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMovementScript : MonoBehaviour {


	public Camera MainCamera;
	public GameObject TargetPosition;
	public int cameraSpeed = 1;
	bool camera_move_enabled = true;
	public GameObject boardState;
	public int cameraTargetIncrement = 0;  

	// Use this for initialization
	void Start () {
	//	MainCamera = (Camera)GameObject.FindObjectOfType(typeOf(Camera));
		moveCameraToActivePlayerColor(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (camera_move_enabled) {

			//MainCamera.transform.position = Vector3.Lerp (transform.position, TargetPosition.transform.position, cameraSpeed * Time.deltaTime);
			//MainCamera.transform.rotation = Quaternion.Lerp (transform.rotation, TargetPosition.transform.rotation, cameraSpeed * Time.deltaTime);

			moveCameraToActivePlayerColor(boardState.GetComponent<BoardManager>().getWhiteTurn());

		}
	}

	public void moveCameraToActivePlayerColor(bool whitesTurn){
		if(whitesTurn && cameraTargetIncrement > 0 ){
			cameraTargetIncrement -= cameraSpeed;
			//TargetPosition.transform.position = new Vector3(4,7,-3);
			//TargetPosition.transform.rotation = new Quaternion(0,0,0 , 0);
		}else if(!whitesTurn && cameraTargetIncrement < 180){
			cameraTargetIncrement+= cameraSpeed;
			//TargetPosition.transform.position = new Vector3(4,7,13);
			//TargetPosition.transform.rotation = new Quaternion(0,180, 0, 0);
		}
		MainCamera.transform.position = new Vector3 (4 - (7 * Mathf.Sin(cameraTargetIncrement * Mathf.Deg2Rad)), 7, -2.5f + cameraTargetIncrement * 13.0f / 180);
		MainCamera.transform.rotation =  Quaternion.Euler( new Vector3 (53, cameraTargetIncrement, 0));


	}

}
