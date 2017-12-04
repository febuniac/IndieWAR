using UnityEngine;
using System.Collections;

public class QuitOnClick : MonoBehaviour {

	public void Quit()
	{
		#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false; // Exit play mode
		#else
			Application.Quit (); //Gray because we are running inthe editor.	
		#endif
	}

}