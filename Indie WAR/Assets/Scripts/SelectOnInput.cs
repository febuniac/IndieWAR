using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectOnInput : MonoBehaviour {

	public EventSystem eventSystem;
	public GameObject selectedObject;

	private bool buttonSelected;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxisRaw ("Vertical") != 0 && buttonSelected == false) //#some movement detected in the vertical axis
		{
			eventSystem.SetSelectedGameObject(selectedObject);//caus ethe event system to select one of our buttons
			buttonSelected = true;
		}
	}

	private void OnDisable()
	{
		buttonSelected = false;
	}
}