using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GridSpace : MonoBehaviour {

	public Button button;//Public variable for the local Button component called "button".
	public Text buttonText;//Public variable for the Button’s associated Text component called "buttonText".

	private GameController gameController;

	public void SetGameControllerReference (GameController controller)
	{
		gameController = controller;
	}

	public void SetSpace ()
	{
		buttonText.text = gameController.GetPlayerSide();//text property to be "X" from playerSide.
		button.interactable = false;//make the button itself non-interactable.
		gameController.EndTurn();//line calling EndTurn.
	}
}

