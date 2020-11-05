using UnityEngine;
using System.Collections;
using DG.Tweening;

/// <summary>
/// In Charge to animate the color of backgrounds elements, smoothly
/// </summary>
public class ChangeMaterialColor : MonoBehaviourHelper
{
	public Renderer r;
	public bool isAngle;

	public void Start()
	{
		r = GetComponent<Renderer> ();

		AnimColor1 ();
		AnimColor2 ();
		AnimeOffset ();
		AnimeUVOffsetX ();
		AnimeUVOffsetY();
	}

	void AnimColor1()
	{
		Color c = constant.RandomBrightColor ();

		if(Random.Range(0,2) == 0)

		c = new Color (c.r, c.g, c.b, Random.Range(0.4f,0.6f));

		r.material.DOColor (c, "_Color", Random.Range(3,10)).OnComplete(AnimColor1);
	}

	void AnimColor2()
	{
		Color c = constant.RandomBrightColor ();

		c = new Color (c.r, c.g, c.b, Random.Range(0.4f,0.6f));

		r.material.DOColor (c, "_Color2", Random.Range(3,10)).OnComplete(AnimColor2);
	}

	void AnimeOffset()
	{
		if (isAngle) 
			r.material.DOFloat (Random.Range (-360.00f, +360.00f), "_Angle", Random.Range (5, 20)).OnComplete (AnimeOffset);
		else 
			r.material.DOFloat (Random.Range (-1.00f, +1.00f), "_Offset", Random.Range (5, 20)).OnComplete (AnimeOffset);
	}

	void AnimeUVOffsetX()
	{
		r.material.DOFloat (Random.Range (-0.50f, +0.50f), "_UVXOffset", Random.Range (5, 20)).OnComplete (AnimeUVOffsetX);
	}

	void AnimeUVOffsetY()
	{
		r.material.DOFloat (Random.Range (-0.50f, +0.50f), "_UVYOffset", Random.Range (5, 20)).OnComplete (AnimeUVOffsetY);

	}
}
