using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public Text[] buttonList;

	private string playerSide;

	void Awake ()
	{
		SetGameControllerReferenceOnButtons();
		playerSide = "Indian";
	}

	void SetGameControllerReferenceOnButtons ()// code that loops through all of the buttons.
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			buttonList[i].GetComponentInParent<GridSpace>().SetGameControllerReference(this);//check each item in the Button List and set the GameController reference in the GridSpace component on the parent GameObject.
			//By passing in this to SetGameControllerReference we are passing in a reference to this instance of the class. Each GridSpace instance will use this to set their reference to the GameController.

		}
	}
	//buttons to do two things: Get the Player Side from the Game Controller before the buttons set the
	//Text value so the buttons know what symbol to place in their grid space and, once they’ve done that, 
	//inform the Game Controller that the turn is now over so that the Game Controller can check the win 
	//conditions and either end the game or change the side taking the turn.
	public string GetPlayerSide ()
	{
		return playerSide;//send the playerSide to GridSpace when it calls GetPlayerSide, and this sets us up for sending the current player's side when we alternate the sides.
	}
		public void EndTurn ()
	{

		if (buttonList [0].text == playerSide && buttonList [1].text == playerSide && buttonList [2].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [3].text == playerSide && buttonList [4].text == playerSide && buttonList [5].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [6].text == playerSide && buttonList [7].text == playerSide && buttonList [8].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [0].text == playerSide && buttonList [3].text == playerSide && buttonList [6].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [1].text == playerSide && buttonList [4].text == playerSide && buttonList [7].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [2].text == playerSide && buttonList [5].text == playerSide && buttonList [8].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [0].text == playerSide && buttonList [4].text == playerSide && buttonList [8].text == playerSide)
		{
			GameOver();
		}

		if (buttonList [2].text == playerSide && buttonList [4].text == playerSide && buttonList [6].text == playerSide)
		{
			GameOver();
		}

		ChangeSides();

	}

	void ChangeSides ()
	{
		playerSide = (playerSide == "Indian") ? "Portuguese" : "Indian";
	}

	void GameOver ()
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			buttonList[i].GetComponentInParent<Button>().interactable = false;
		}
	}
}