using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public GameObject firstDigit;
	public GameObject secondDigit;
	public GameObject thirdDigit;
	public GameObject fourthDigit;

	private Animator anim1;
	private Animator anim2;
	private Animator anim3;
	private Animator anim4;

	// Use this for initialization
	void Start () {
		anim1 = firstDigit.GetComponent<Animator> ();
		anim2 = secondDigit.GetComponent<Animator> ();
		anim3 = thirdDigit.GetComponent<Animator> ();
		anim4 = fourthDigit.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void updateScore(int newScore)
	{
		anim1.SetInteger ("Number", newScore % 10);
		newScore /= 10;
		anim2.SetInteger ("Number", newScore % 10);
		newScore /= 10;
		anim3.SetInteger ("Number", newScore % 10);
		newScore /= 10;
		anim4.SetInteger ("Number", newScore % 10);
	}

	public void killPacMan()
	{
		Destroy (GetComponent<ConfigGameStart>().Inky);
		Destroy (GetComponent<ConfigGameStart>().Blinky);
		Destroy (GetComponent<ConfigGameStart>().Pinky);
		Destroy (GetComponent<ConfigGameStart>().Clyde);
		GetComponent<ConfigGameStart> ().PacMan.GetComponent<PacManController> ().killPacMan ();
	}
}
