using UnityEngine;
using System.Collections;

public class Webcam : MonoBehaviour
{
	public FilterMode filterMode;
	[HideInInspector] public WebCamTexture texture;
	[HideInInspector] public bool isReady = false;

	void Awake ()
	{
		if (WebCamTexture.devices.Length > 0)  
		{
			texture = new WebCamTexture();
			texture.Play();
			texture.filterMode = filterMode;

			isReady = texture.width * texture.height > 512;
		}
	}

	void Update ()
	{
		if (!isReady && texture) {
			isReady = texture.width * texture.height > 512;
		}
	}
}