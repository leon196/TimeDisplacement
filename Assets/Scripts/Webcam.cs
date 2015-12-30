using UnityEngine;
using System.Collections;

public class Webcam : MonoBehaviour
{
	public FilterMode filterMode;
	[HideInInspector] public WebCamTexture texture;
	[HideInInspector] public bool isReady = false;
	HelpText helpText;

	void Awake () 
	{
		helpText = GameObject.FindObjectOfType<HelpText>();

#if UNITY_WEBGL
    StartCoroutine(RequestAuthorization());
#else
    Init();
#endif
	}

	void Init ()
	{
		if (WebCamTexture.devices.Length > 0)  
		{
			texture = new WebCamTexture(640, 480, 60);
			texture.Play();
			texture.filterMode = filterMode;

			isReady = texture.width * texture.height > 512;

			helpText.helpTextList.Add("camera name : " + WebCamTexture.devices[0].name);
			helpText.helpTextList.Add("resolution : " + texture.width + " x " + texture.height);
		}
	}

	IEnumerator RequestAuthorization ()
	{
    yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
    	Init();
    }
	}

	void Update ()
	{
		if (!isReady && texture) {
			isReady = texture.width * texture.height > 512;
		}
	}
}