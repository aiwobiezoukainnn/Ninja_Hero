using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Class in charge to update ui element colors
/// </summary>
public class UpdateColorEqualCamColor : MonoBehaviour 
{

	Image image;

	Camera cam;

	void Start()
	{
		cam = Camera.main;
		image = GetComponent<Image> ();
	}


	void OnEnable()
	{
		StartCoroutine (CoUpdate ());
	}

	void OnDisable()
	{
		StopAllCoroutines ();
	}

	IEnumerator CoUpdate () 
	{
		while (true) {
			

			if (image != null) {
				image.color = cam.backgroundColor;
			}
			yield return 0;
		}
	}
}
