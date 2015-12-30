using UnityEngine;
using System.Collections;

public class SlitScan : MonoBehaviour
{
	Webcam webcam;
	bool inited = false;
	Texture2D texture;
	Color[] colorList;
	int width, height, sliceCount;
	[HideInInspector] public bool direction;

	void Start ()
	{
		webcam = GameObject.FindObjectOfType<Webcam>();
		sliceCount = (int)Screen.height / 2;
		direction = false;
	}

	void Init ()
	{
		width = webcam.texture.width;
		height = webcam.texture.height;

		texture = new Texture2D(width, height);
		colorList = new Color[width * height];
		for (int c = 0; c < colorList.Length; ++c) {
			colorList[c] = Color.black;
		}
		texture.SetPixels(colorList);
		texture.Apply(false);

		Shader.SetGlobalTexture("_SlitTexture", texture);

		inited = true;
	}

	void Update ()
	{
		if (webcam.isReady)
		{
			if (inited)
			{
				Color[] webcamColors = webcam.texture.GetPixels();
				for (int i = colorList.Length - 1; i > 0; --i) {
					int x = (int)(i % width);
					int y = (int)Mathf.Floor(i / width);
					if (x > 1) {
						int index = (int)Mathf.Clamp(x - 1f, 0f, width - 1) + y * width;
					// if (y > 1) {
						// int index = (int)(x + Mathf.Clamp(y - 1f, 0f, height - 1) * width);
						colorList[i] = colorList[index];
					} else {
						colorList[i] = webcamColors[i];
					}
				}

				// Apply new colors to texture
				texture.SetPixels(colorList);
				texture.Apply(false);
			}
			else 
			{
				Init();
			}
		}
	}

	public void ResizeSlices (int newSliceCount)
	{
		if (sliceCount != newSliceCount) {
			sliceCount = newSliceCount;
		}
	}
}