using UnityEngine;
using System.Collections;

public class UserParameter : MonoBehaviour
{
	[HideInInspector] public int sliceSize = 2;
	[HideInInspector] public int sliceCount;
	[HideInInspector] public bool direction = false;
	[HideInInspector] public bool flipH = false;
	[HideInInspector] public bool flipV = true;
	[HideInInspector] public bool mode = true;
	TimeDisplacement timeDisplacement;
	HelpText helpText;

	void Awake ()
	{
		helpText = GameObject.FindObjectOfType<HelpText>();
		timeDisplacement = GameObject.FindObjectOfType<TimeDisplacement>();
		Shader.SetGlobalFloat("_Mode", mode ? 1f : 0f);
		Shader.SetGlobalFloat("_Vertical", flipV ? 1f : 0f);
		Shader.SetGlobalFloat("_Horizontal", flipH ? 1f : 0f);
		UpdateParameters();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			mode = !mode;
			Shader.SetGlobalFloat("_Mode", mode ? 1f : 0f);
		}

		if (Input.GetKeyDown(KeyCode.V)) {
			flipV = !flipV;
			Shader.SetGlobalFloat("_Vertical", flipV ? 1f : 0f);
		}

		if (Input.GetKeyDown(KeyCode.H)) {
			flipH = !flipH;
			Shader.SetGlobalFloat("_Horizontal", flipH ? 1f : 0f);
		}

		if (Input.GetKeyDown(KeyCode.D)) {
			direction = !direction;
			timeDisplacement.direction = direction;
		}

		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F1)) {
			helpText.Toggle();
		}

		if (Input.GetKeyDown(KeyCode.LeftArrow) && sliceSize > 1) {
			sliceSize = (int)Mathf.Clamp(sliceSize - 1, 1, 8);
			UpdateParameters();
		}

		if (Input.GetKeyDown(KeyCode.RightArrow) && sliceSize < 8) {
			sliceSize = (int)Mathf.Clamp(sliceSize + 1, 1, 8);
			UpdateParameters();
		}
	}

	void UpdateParameters ()
	{
		sliceCount = (int)Screen.height / sliceSize;
		timeDisplacement.ResizeSlices(sliceCount);
		helpText.sliceSize = sliceSize;
		helpText.UpdateHelpText();
	}
}