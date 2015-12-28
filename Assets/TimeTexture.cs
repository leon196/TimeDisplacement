using UnityEngine;
using System.Collections;

public class TimeTexture : MonoBehaviour
{
	WebCamTexture webcamTexture;
	Texture2D timeTexture;
	Color[][] colorList;
	int width, height, sliceCount;
	int userSliceCount = 2;
	bool direction = false;
	bool flipH = false;
	bool flipV = true;

	GUIText helpText;
	string helpText1 = "Time Displacement with Unity3D by Leon\nCode sources : github.com/leon196/TimeDisplacement\n\nSlices size : ";
	string helpText2 = "\nLEFT and RIGHT to set time slices size\n\nD to change direction\nV to flip webcam vertically\nH to flip webcam horizontally\n\nF1 or X to toggle this message\nESCAPE to quit";

	void SetupWebcam ()
	{
		if (WebCamTexture.devices.Length > 0)  
		{
			// Setup webcam texture
			webcamTexture = new WebCamTexture();
			webcamTexture.Play();
			webcamTexture.filterMode = FilterMode.Point;

			// Setup dimensions
			width = webcamTexture.width;
			height = webcamTexture.height;

			// Setup how many slices for time displacement
			sliceCount = height;
		}
	}

	void SetupTimeTexture ()
	{
		timeTexture = new Texture2D(width, height);
		colorList = new Color[sliceCount][];
		for (int i = 0; i < sliceCount; ++i) {
			Color[] color = new Color[width * height];
			for (int c = 0; c < width * height; ++c) {
				color[c] = Color.black;
			}
			colorList[i] = color;
		}
		timeTexture.SetPixels(colorList[0]);
		timeTexture.Apply(false);

		// Assign texture to global uniform
		Shader.SetGlobalTexture("_TimeTexture", timeTexture);
	}

	void Start ()
	{
		SetupWebcam();
		SetupTimeTexture();
		helpText = GameObject.FindObjectOfType<GUIText>();
		UpdateHelpText();
		Shader.SetGlobalFloat("_Vertical", flipV ? 1f : 0f);
		Shader.SetGlobalFloat("_Horizontal", flipH ? 1f : 0f);
	}

	void Update ()
	{
		UpdateInputs();

		if (webcamTexture)
		{
			// Shift colors
			for (int i = sliceCount - 1; i > 0; --i) {
				colorList[i] = colorList[i - 1];
			}

			// Refresh first color list
			Color[] webcamColors = webcamTexture.GetPixels();
			colorList[0] = webcamColors;

			// Map colors into one texture
			Color[] newColors = new Color[webcamColors.Length];
			for (int c = 0; c < webcamColors.Length; ++c) {
				float ratio;
				if (direction) {
					ratio = (webcamColors.Length - c - 1) / (float)webcamColors.Length;
				}
				else {
					ratio = c / (float)webcamColors.Length;
				}
				int index = (int)Mathf.Floor(ratio * sliceCount);
				newColors[c] = colorList[index][c];
			}

			// Apply new colors to texture
			timeTexture.SetPixels(newColors);
			timeTexture.Apply(false);

			// Adjust Slice Count with horizontal arrows
			if (Input.GetKeyDown(KeyCode.LeftArrow) && userSliceCount > 1) {
				userSliceCount = (int)Mathf.Clamp(userSliceCount - 1, 1, 8);
				ResizeSlices();
			} else if (Input.GetKeyDown(KeyCode.RightArrow) && userSliceCount < 8) {
				userSliceCount = (int)Mathf.Clamp(userSliceCount + 1, 1, 8);
				ResizeSlices();
			}
		}
	}

	void ResizeSlices ()
	{
		sliceCount = (int)Screen.height / userSliceCount;
		colorList = new Color[sliceCount][];
		for (int i = 0; i < sliceCount; ++i) {
			Color[] color = new Color[width * height];
			for (int c = 0; c < width * height; ++c) {
				color[c] = Color.black;
			}
			colorList[i] = color;
		}
		UpdateHelpText();
	}

	void UpdateHelpText ()
	{
		helpText.text = helpText1 + userSliceCount + helpText2;
	}

	void UpdateInputs ()
	{
		if (Input.GetKeyDown(KeyCode.Escape)) {
			Application.Quit();
		}

		if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.F1)) {
			helpText.enabled = !helpText.enabled;
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
		}
	}
}