using UnityEngine;
using System.Collections;

public class TimeDisplacement : MonoBehaviour
{
	Webcam webcam;
	Texture2D texture;
	Color[][] colorList;
	int width, height, sliceCount;
	[HideInInspector] public bool direction;

	void Start ()
	{
		webcam = GameObject.FindObjectOfType<Webcam>();

		sliceCount = (int)Screen.height / 2;
		direction = false;

		width = webcam.texture.width;
		height = webcam.texture.height;

		texture = new Texture2D(width, height);
		colorList = new Color[sliceCount][];
		for (int i = 0; i < sliceCount; ++i) {
			Color[] color = new Color[width * height];
			for (int c = 0; c < width * height; ++c) {
				color[c] = Color.black;
			}
			colorList[i] = color;
		}
		texture.SetPixels(colorList[0]);
		texture.Apply(false);

		Shader.SetGlobalTexture("_TimeTexture", texture);
	}

	void Update ()
	{
		if (webcam.isReady)
		{
			// Shift colors
			for (int i = sliceCount - 1; i > 0; --i) {
				colorList[i] = colorList[i - 1];
			}

			// Refresh first color list
			Color[] webcamColors = webcam.texture.GetPixels();
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
			texture.SetPixels(newColors);
			texture.Apply(false);
		}
	}

	public void ResizeSlices (int newSliceCount)
	{
		if (sliceCount != newSliceCount) {
			sliceCount = newSliceCount;
			colorList = new Color[sliceCount][];
			for (int i = 0; i < sliceCount; ++i) {
				Color[] color = new Color[width * height];
				for (int c = 0; c < width * height; ++c) {
					color[c] = Color.black;
				}
				colorList[i] = color;
			}
		}
	}
}