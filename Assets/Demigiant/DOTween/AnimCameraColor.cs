using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// In Charge to change the camera background color
/// </summary>
public class AnimCameraColor : MonoBehaviourHelper 
{
	void Start ()
	{
		AnimColor ();
	}
	
	void AnimColor()
	{
		Color c = constant.RandomBrightColor ();

		Camera.main.DOColor (c, Random.Range (3, 10)).OnComplete (AnimColor);
	}
}
