﻿using UnityEngine;
using System.Collections;

public class ClydeController : MonoBehaviour {

	public float moveSpeed;
	
	public GameObject PacMan;
	
	private int currentAction = -1;
	private Animator anim;
	
	public int locationX;
	public int locationY;

	public int startX;
	public int startY;
	
	public GameObject gameController;
	private GameObject currentSpace = null;

	public GameObject highlight;
	private GameObject previousHighlight = null;

	public bool isABitch;

	int count = 0;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		moveClyde ();
		if (isABitch)
		{
			anim.SetInteger("Action", 4);
		}
		if (count != 0)
		{
			count--;
		}
	}
	
	void moveClyde()
	{
		if (currentAction == 0)
		{
			transform.Translate(Vector3.left *moveSpeed);
		}
		else if (currentAction == 1)
		{
			transform.Translate(Vector3.right *moveSpeed);
		}
		else if (currentAction == 2)
		{
			transform.Translate(Vector3.up *moveSpeed);
		}
		else if (currentAction == 3)
		{
			transform.Translate(Vector3.down *moveSpeed);
		}
	}

	public void toggleMode()
	{
		if (isABitch)
		{
			transform.position = new Vector3(startX, startY, 0);
			isABitch = false;
			anim.SetInteger("Action", 0);
		}
		else
		{
			isABitch = true;
			anim.SetInteger("Action", 4);
		}
	}
	
	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "empty")
		{
			if (collider.gameObject != currentSpace)
			{
				currentSpace = collider.gameObject;
				locationX = collider.gameObject.GetComponent<TileController>().x;
				locationY = collider.gameObject.GetComponent<TileController>().y;
				if (atIntersection()){chooseDirection ();}
			}
		}

		if (collider.tag == "pacman" && count == 0 && isABitch)
		{
			Debug.Log("pacman hit clyde!");
			toggleMode();
			count = 10;
		}
		else if (collider.tag == "pacman" && count == 0 && !isABitch)
		{
			Debug.Log("clyde hit pacman!");
			toggleMode();
			collider.GetComponent<PacManController>().reset();
			count = 10;
		}
	}
	
	bool atIntersection()
	{
		int[,] tileStates = gameController.GetComponent<ConfigGameStart> ().tileStates;
		if ((tileStates[locationX,locationY-1] == 0 && tileStates[locationX-1,locationY] == 0) ||
		    (tileStates[locationX,locationY-1] == 0 && tileStates[locationX+1,locationY] == 0) ||
		    (tileStates[locationX,locationY+1] == 0 && tileStates[locationX-1,locationY] == 0) ||
		    (tileStates[locationX,locationY+1] == 0 && tileStates[locationX+1,locationY] == 0))
		{
			return true;
		}
		return false;
	}
	
	void chooseDirection()
	{
		int[,] tileStates = gameController.GetComponent<ConfigGameStart> ().tileStates;

		if (isABitch)
		{
			int upScore = 0;
			int downScore = 0;
			int leftScore = 0;
			int rightScore = 0;
			
			if (tileStates[locationX+1,locationY] == 0 && currentAction != 0){rightScore = Random.Range (1,1000);}
			if (tileStates[locationX-1,locationY] == 0 && currentAction != 1){leftScore = Random.Range (1,1000);}
			if (tileStates[locationX,locationY+1] == 0 && currentAction != 3){upScore = Random.Range (1,1000);}
			if (tileStates[locationX,locationY-1] == 0 && currentAction != 2){downScore = Random.Range (1,1000);}
			
			if (rightScore >= leftScore && rightScore >= downScore && rightScore >= upScore && rightScore != 0) {currentAction = 1;}
			else if (leftScore >= rightScore && leftScore >= downScore && leftScore >= upScore && leftScore != 0) {currentAction = 0;}
			else if (upScore >= leftScore && upScore >= downScore && upScore >= rightScore && upScore != 0) {currentAction = 2;}
			else if (downScore >= leftScore && downScore >= rightScore && downScore >=upScore && downScore != 0) {currentAction = 3;}
		}
		else
		{
			int upScore = 2;
			int downScore = 2;
			int leftScore = 2;
			int rightScore = 2;
			
			int desiredX = PacMan.GetComponent<PacManController> ().locationX;
			int desiredY = PacMan.GetComponent<PacManController> ().locationY;
			int distanceToPacMan = Mathf.Abs (locationX - desiredX) + Mathf.Abs (locationY - desiredY);
			
			if (distanceToPacMan < 8)
			{
				desiredX = startX;
				desiredY = startY;
				distanceToPacMan = Mathf.Abs (locationX-desiredX) + Mathf.Abs(locationY-desiredY);
			}
			
			if (tileStates[locationX+1,locationY] == 0 && currentAction != 0){
				int newDistToPacMan = Mathf.Abs (locationX+1-desiredX)+Mathf.Abs(locationY-desiredY);
				rightScore = newDistToPacMan - distanceToPacMan;
			}
			if (tileStates[locationX-1,locationY] == 0 && currentAction != 1){
				int newDistToPacMan = Mathf.Abs (locationX-1-desiredX)+Mathf.Abs(locationY-desiredY);
				leftScore = newDistToPacMan - distanceToPacMan;
			}
			if (tileStates[locationX,locationY+1] == 0 && currentAction != 3){
				int newDistToPacMan = Mathf.Abs (locationX-desiredX)+Mathf.Abs(locationY+1-desiredY);
				upScore = newDistToPacMan - distanceToPacMan;
			}
			if (tileStates[locationX,locationY-1] == 0 && currentAction != 2){
				int newDistToPacMan = Mathf.Abs (locationX-desiredX)+Mathf.Abs(locationY-1-desiredY);
				downScore = newDistToPacMan - distanceToPacMan;
			}
			
			if (rightScore <= leftScore && rightScore <= downScore && rightScore <= upScore && currentAction != 0) {currentAction = 1;}
			else if (leftScore <= rightScore && leftScore <= downScore && leftScore <= upScore && currentAction != 1) {currentAction = 0;}
			else if (upScore <= leftScore && upScore <= downScore && upScore <= rightScore && currentAction != 3) {currentAction = 2;}
			else if (downScore <= leftScore && downScore <= rightScore && downScore <=upScore && currentAction != 2) {currentAction = 3;}
			
			colorTile (desiredX, desiredY);
		}
		
		if(isABitch)
		{
			anim.SetInteger("Action", 4);
		}
		else
		{
			anim.SetInteger("Action", currentAction);
		}
	}
	
	void colorTile(int x, int y)
	{
		Destroy (previousHighlight);
		previousHighlight = (GameObject)Instantiate(highlight, new Vector3 (x, y, 0), Quaternion.identity);
	}
}
