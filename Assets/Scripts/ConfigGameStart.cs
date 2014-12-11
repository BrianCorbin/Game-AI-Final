using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

public class ConfigGameStart : MonoBehaviour {

	public GameObject innerTopLeft;
	public GameObject innerTopRight;
	public GameObject innerBotLeft;
	public GameObject innerBotRight;
	public GameObject innerVertical;
	public GameObject innerHorizontal;
	public GameObject empty;
	public GameObject Gate;
	public GameObject outerTopLeft;
	public GameObject outerTopRight;
	public GameObject outerBotLeft;
	public GameObject outerBotRight;
	public GameObject outerVertical;
	public GameObject outerHorizontal;

	public GameObject pellet;
	public GameObject PacMan;
	public GameObject Inky;
	public GameObject Blinky;
	public GameObject Pinky;
	public GameObject Clyde;

	public GameObject[,] tileObjects;
	public int[,] tileStates; 

	public string fileDir;

	// Use this for initialization
	void Start () 
	{
		StreamReader sr = new StreamReader (fileDir, Encoding.Default);
		string firstLine = sr.ReadLine ();

		//dimensions[0] == x
		//dimensions[1] == y
		string[] dimensions = firstLine.Split (' ');

		tileObjects = new GameObject[Convert.ToInt32 (dimensions [0]), Convert.ToInt32 (dimensions [1])];
		tileStates = new int[Convert.ToInt32 (dimensions [0]), Convert.ToInt32 (dimensions [1])];

		string[] lines = new string[Convert.ToInt32(dimensions [1])];

		int i = Convert.ToInt32(dimensions[1]) - 1;
		while(sr.Peek () >= 0)
		{
			lines[i] = sr.ReadLine();
			i--;
		}

		for(int y=0; y<Convert.ToInt32(dimensions[1]); y++)
		{
			int x = 0;
			string line = lines[y];

			foreach (char c in line)
			{
				GameObject newGameObject = new GameObject();
				bool isPassable = false;

				if(c == '1')
				{newGameObject = outerTopLeft;}
				else if(c == '2')
				{newGameObject = outerTopRight;}
				else if(c == '3')
				{newGameObject = outerBotLeft;}
				else if(c == '4')
				{newGameObject = outerBotRight;}
				else if(c == '5')
				{newGameObject = innerTopLeft;}
				else if(c == '6')
				{newGameObject = innerTopRight;}
				else if(c == '7')
				{newGameObject = innerBotLeft;}
				else if(c == '8')
				{newGameObject = innerBotRight;}
				else if(c == '|')
				{newGameObject = innerVertical;}
				else if(c == '#')
				{newGameObject = outerVertical;}
				else if(c == '-')
				{newGameObject = innerHorizontal;}
				else if(c == '=')
				{newGameObject = outerHorizontal;}
				else if(c == 'g')
				{newGameObject = Gate;}
				else if(c == '.' || c == '<' || c == 'i' || c == 'b' || 
				        c == 'p' || c == 'c' || c == ' ')
				{
					newGameObject = empty;
					isPassable = true;
					if(c == '.')
					{Instantiate(pellet, new Vector3(x, y, 0.1f), Quaternion.identity);}
					else if(c == '<')
					{
						PacMan = (GameObject)Instantiate(PacMan, new Vector3(x,y,0), Quaternion.identity);
						PacMan.GetComponent<PacManController>().gameController = gameObject;
						PacMan.GetComponent<PacManController>().locationX = x;
						PacMan.GetComponent<PacManController>().locationY = y;
						PacMan.GetComponent<PacManController>().startX = x;
						PacMan.GetComponent<PacManController>().startY = y;
					}
					else if(c == 'i')
					{
						Inky = (GameObject)Instantiate(Inky, new Vector3(x,y,0), Quaternion.identity);
						Inky.GetComponent<InkyController>().gameController = gameObject;
						Inky.GetComponent<InkyController>().locationX = x;
						Inky.GetComponent<InkyController>().locationY = y;
						Inky.GetComponent<InkyController>().startX = x;
						Inky.GetComponent<InkyController>().startY = y;
						Inky.GetComponent<InkyController>().isABitch = false;
					}
					else if(c == 'b')
					{
						Blinky = (GameObject)Instantiate(Blinky, new Vector3(x,y,0), Quaternion.identity);
						Blinky.GetComponent<BlinkyController>().gameController = gameObject;
						Blinky.GetComponent<BlinkyController>().locationX = x;
						Blinky.GetComponent<BlinkyController>().locationY = y;
						Blinky.GetComponent<BlinkyController>().isABitch = true;
						Blinky.GetComponent<BlinkyController>().startX = x;
						Blinky.GetComponent<BlinkyController>().startY = y;
					}
					else if(c == 'p')
					{
						Pinky = (GameObject)Instantiate(Pinky, new Vector3(x,y,0), Quaternion.identity);
						Pinky.GetComponent<PinkyController>().gameController = gameObject;
						Pinky.GetComponent<PinkyController>().locationX = x;
						Pinky.GetComponent<PinkyController>().locationY = y;
						Pinky.GetComponent<PinkyController>().width = Convert.ToInt32(dimensions[0]);
						Pinky.GetComponent<PinkyController>().height = Convert.ToInt32 (dimensions[1]);
						Pinky.GetComponent<PinkyController>().startX = x;
						Pinky.GetComponent<PinkyController>().startY = y;
						Pinky.GetComponent<PinkyController>().isABitch = true;
					}
					else if(c == 'c')
					{
						Clyde = (GameObject)Instantiate(Clyde, new Vector3(x,y,0), Quaternion.identity);
						Clyde.GetComponent<ClydeController>().gameController = gameObject;
						Clyde.GetComponent<ClydeController>().locationX = x;
						Clyde.GetComponent<ClydeController>().locationY = y;
						Clyde.GetComponent<ClydeController>().startX = x;
						Clyde.GetComponent<ClydeController>().startY = y;
						Clyde.GetComponent<ClydeController>().isABitch = true;
					}
				}

				tileObjects[x,y] = (GameObject)Instantiate(newGameObject, new Vector3(x,y,0), Quaternion.identity);

				tileObjects[x,y].GetComponent<TileController>().x = x;
				tileObjects[x,y].GetComponent<TileController>().y = y;

				if (isPassable) {tileStates[x,y] = 0;}
				else {tileStates[x,y] = 1;}
				x++;
			}
		}

		Clyde.GetComponent<ClydeController> ().PacMan = PacMan;
		Blinky.GetComponent<BlinkyController> ().PacMan = PacMan;
		Inky.GetComponent<InkyController> ().PacMan = PacMan;
		Inky.GetComponent<InkyController> ().Blinky = Blinky;
		Pinky.GetComponent<PinkyController>().PacMan = PacMan;

		Physics2D.IgnoreCollision (Clyde.transform.collider2D, Blinky.transform.collider2D);
		Physics2D.IgnoreCollision (Clyde.transform.collider2D, Inky.transform.collider2D);
		Physics2D.IgnoreCollision (Clyde.transform.collider2D, Pinky.transform.collider2D);
		Physics2D.IgnoreCollision (Blinky.transform.collider2D, Inky.transform.collider2D);
		Physics2D.IgnoreCollision (Blinky.transform.collider2D, Pinky.transform.collider2D);
		Physics2D.IgnoreCollision (Inky.transform.collider2D, Pinky.transform.collider2D);

	}
}
