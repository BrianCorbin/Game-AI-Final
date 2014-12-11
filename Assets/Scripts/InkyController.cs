using UnityEngine;
using System.Collections;

public class InkyController : MonoBehaviour {

	public float moveSpeed;

	public GameObject PacMan;
	public GameObject Blinky;
	
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
		moveInky ();
		if (isABitch)
		{
			anim.SetInteger("Action", 4);
		}
		if (count != 0)
		{
			count--;
		}
	}

	void moveInky()
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
			Debug.Log("pacman hit inky!");
			toggleMode();
			count = 10;
		}
		else if (collider.tag == "pacman" && count == 0 && !isABitch)
		{
			Debug.Log("inky hit pacman!");
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

	bool isIntersection(int x, int y)
	{
		int[,] tileStates = gameController.GetComponent<ConfigGameStart> ().tileStates;
		if ((tileStates[x,y-1] == 0 && tileStates[x-1,y] == 0) ||
		    (tileStates[x,y-1] == 0 && tileStates[x+1,y] == 0) ||
		    (tileStates[x,y+1] == 0 && tileStates[x-1,y] == 0) ||
		    (tileStates[x,y+1] == 0 && tileStates[x+1,y] == 0))
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
			//Distance from Blinky to PacMan
			int distBlinkyToPacMan = Mathf.Abs (Blinky.GetComponent<BlinkyController> ().locationX - PacMan.GetComponent<PacManController> ().locationX) +
				Mathf.Abs (Blinky.GetComponent<BlinkyController> ().locationY - PacMan.GetComponent<PacManController> ().locationY);
			int distFromPacManToTravel = distBlinkyToPacMan * 2 + 2;
			
			int currentX = PacMan.GetComponent<PacManController> ().locationX;
			int currentY = PacMan.GetComponent<PacManController> ().locationY;
			int currentDirection = PacMan.GetComponent<PacManController> ().currentInput;
			while(distFromPacManToTravel > 0)
			{
				if (isIntersection(currentX, currentY))
				{
					int rightSpace = 0;
					int leftSpace = 0;
					int upSpace = 0;
					int downSpace = 0;
					
					if (tileStates[currentX+1,currentY] == 0 && currentDirection != 0){rightSpace = Random.Range (1,1000);}
					if (tileStates[currentX-1,currentY] == 0 && currentDirection != 1){leftSpace = Random.Range (1,1000);}
					if (tileStates[currentX,currentY+1] == 0 && currentDirection != 3){upSpace = Random.Range (1,1000);}
					if (tileStates[currentX,currentY-1] == 0 && currentDirection != 2){downSpace = Random.Range (1,1000);}
					
					if (rightSpace >= leftSpace && rightSpace >= downSpace && rightSpace >= upSpace && rightSpace != 0) {currentDirection = 1; currentX++;}
					else if (leftSpace >= rightSpace && leftSpace >= downSpace && leftSpace >= upSpace && leftSpace != 0) {currentDirection = 0; currentX--;}
					else if (upSpace >= leftSpace && upSpace >= downSpace && upSpace >= rightSpace && upSpace != 0) {currentDirection = 2; currentY++;}
					else if (downSpace >= leftSpace && downSpace >= rightSpace && downSpace >=upSpace && downSpace != 0) {currentDirection = 3; currentY--;}
				}
				else
				{
					if (currentDirection == 0){currentX--;}
					else if (currentDirection == 1){currentX++;}
					else if (currentDirection == 2){currentY++;}
					else if (currentDirection == 3){currentY--;}
				}
				distFromPacManToTravel--;
			}
			
			int upScore = 2;
			int downScore = 2;
			int leftScore = 2;
			int rightScore = 2;
			
			int desiredSpaceX = currentX;
			int desiredSpaceY = currentY;
			int distanceToDesiredSpace = Mathf.Abs (locationX - desiredSpaceX) + Mathf.Abs (locationY - desiredSpaceY);
			
			if (tileStates[locationX+1,locationY] == 0 && currentAction != 0){
				int newDistToDesiredSpace = Mathf.Abs (locationX+1-desiredSpaceX)+Mathf.Abs(locationY-desiredSpaceY);
				rightScore = newDistToDesiredSpace - distanceToDesiredSpace;
			}
			if (tileStates[locationX-1,locationY] == 0 && currentAction != 1){
				int newDistToDesiredSpace = Mathf.Abs (locationX-1-desiredSpaceX)+Mathf.Abs(locationY-desiredSpaceY);
				leftScore = newDistToDesiredSpace - distanceToDesiredSpace;
			}
			if (tileStates[locationX,locationY+1] == 0 && currentAction != 3){
				int newDistToDesiredSpace = Mathf.Abs (locationX-desiredSpaceX)+Mathf.Abs(locationY+1-desiredSpaceY);
				upScore = newDistToDesiredSpace - distanceToDesiredSpace;
			}
			if (tileStates[locationX,locationY-1] == 0 && currentAction != 2){
				int newDistToDesiredSpace = Mathf.Abs (locationX-desiredSpaceX)+Mathf.Abs(locationY-1-desiredSpaceY);
				downScore = newDistToDesiredSpace - distanceToDesiredSpace;
			}
			
			if (rightScore <= leftScore && rightScore <= downScore && rightScore <= upScore && currentAction != 0) {currentAction = 1;}
			else if (leftScore <= rightScore && leftScore <= downScore && leftScore <= upScore && currentAction != 1) {currentAction = 0;}
			else if (upScore <= leftScore && upScore <= downScore && upScore <= rightScore && currentAction != 3) {currentAction = 2;}
			else if (downScore <= leftScore && downScore <= rightScore && downScore <=upScore && currentAction != 2) {currentAction = 3;}
			
			colorTile(desiredSpaceX, desiredSpaceY);
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
