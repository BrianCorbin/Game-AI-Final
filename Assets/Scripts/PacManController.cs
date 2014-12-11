using UnityEngine;
using System.Collections;

public class PacManController : MonoBehaviour {

	public int currentInput = 0;
	public float moveSpeed;

	public int score = 0;
	public GameObject gameController;
	public int locationX;
	public int locationY;

	public int startX;
	public int startY;

	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		/* inputs
		 * 0:left
		 * 1:right
		 * 2:up
		 * 3:down
		 * 4:dead
		 */
		if (currentInput != 4)
		{

			if (Input.GetKey(KeyCode.LeftArrow))
			{
				currentInput = 0;
				anim.SetInteger("Action", 0);
			}
			else if (Input.GetKey(KeyCode.RightArrow))
			{
				currentInput = 1;
				anim.SetInteger("Action", 1);
			}
			else if (Input.GetKey(KeyCode.UpArrow))
			{
				currentInput = 2;
				anim.SetInteger("Action", 2);
			}
			else if (Input.GetKey(KeyCode.DownArrow))
			{
				currentInput = 3;
				anim.SetInteger("Action", 3);
			}

			if (currentInput == 0)
			{
				transform.Translate(Vector3.left *moveSpeed);
			}
			else if (currentInput == 1)
			{
				transform.Translate(Vector3.right * moveSpeed);
			}
			else if (currentInput == 2)
			{
				transform.Translate(Vector3.up * moveSpeed);
			}
			else if (currentInput == 3)
			{
				transform.Translate(Vector3.down * moveSpeed);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.tag == "pellet")
		{
			score++;
			Destroy(collider.gameObject);
			gameController.GetComponent<GameController>().updateScore(score);
		}

		if (collider.tag == "empty")
		{
			GameObject tileObject = collider.gameObject;
			locationX = tileObject.GetComponent<TileController>().x;
			locationY = tileObject.GetComponent<TileController>().y;
		}
	}

	public void reset()
	{
		transform.position = new Vector3 (startX, startY, 0);
	}

	public void killPacMan()
	{
		currentInput = 4;
		anim.SetInteger ("Action", 4);
	}

	public void destroyPacMan()
	{
		Destroy (gameObject);
	}
}
