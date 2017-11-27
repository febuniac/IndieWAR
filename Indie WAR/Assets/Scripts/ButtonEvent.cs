using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonEvent : MonoBehaviour {
	public Button yourButton;
	// Use this for initialization
	void Start () {
		Button btn = yourButton.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		
	}
	
	void TaskOnClick()
	{
		Debug.Log("You have clicked the button!");
	}
}

