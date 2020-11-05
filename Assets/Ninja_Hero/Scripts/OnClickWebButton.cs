using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OnClickWebButton : MonoBehaviour 
{
	public bool isAppleStore;
	public bool isGoogleStore;
	public bool isAmazonStore;

	public string urlAppleStore = "https://itunes.apple.com/us/app/qq-tap-to-shoot-the-dots/id1048207034";
	public string urlGoogleStore;
	public string urlAmazonStore;

	void Start()
	{
		GetComponent<Button> ().onClick.AddListener (() => {
			if(isAppleStore)
				Application.OpenURL(urlAppleStore);
			else if(isGoogleStore)
				Application.OpenURL(urlGoogleStore);
			else if(isAmazonStore)
				Application.OpenURL(urlAmazonStore);
			else
				Application.OpenURL(urlAppleStore);
		});
	}

}
