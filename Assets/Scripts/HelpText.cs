using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class HelpText : MonoBehaviour
{
	GUIText helpText;
	string helpText1 = "Time Displacement with Unity3D by Leon\nCode sources : github.com/leon196/TimeDisplacement\n\nSlices size : ";
	string helpText2 = "\nLEFT and RIGHT to set time slices size\n\nD to change direction\nV to flip webcam vertically\nH to flip webcam horizontally\n\nF1 or X to toggle this message\nESCAPE to quit";
	[HideInInspector] public List<string> helpTextList;
	[HideInInspector] public int sliceSize = 2;

	void Start ()
	{
		helpText = GameObject.FindObjectOfType<GUIText>();
		UpdateHelpText();
	}

	public void UpdateHelpText ()
	{
		if (helpText) {
			helpText.text = helpText1 + sliceSize + helpText2 + "\n\n" + string.Join("\n", helpTextList.ToArray());
		}
	}

	public void Toggle ()
	{
		if (helpText) {
			helpText.enabled = !helpText.enabled;
			UpdateHelpText();
		}
	}
}