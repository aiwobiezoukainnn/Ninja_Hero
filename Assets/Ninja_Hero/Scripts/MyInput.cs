using UnityEngine;
using System.Collections;

public class MyInput : MonoBehaviour 
{
	public delegate void OnTouchDown(Vector3 pos);
	public static event OnTouchDown onTouchDown;

	public delegate void OnTouchUp(Vector3 pos);
	public static event OnTouchDown onTouchUp;


	void Update()
	{
		if (Application.isMobilePlatform) 
		{
			int nbTouches = Input.touchCount;

			if (nbTouches > 0) 
			{
				for (int i = 0; i < nbTouches; i++)
				{
					Touch touch = Input.GetTouch (i);

					TouchPhase phase = touch.phase;

					if (phase == TouchPhase.Began && onTouchDown != null) 
					{
						onTouchDown (touch.position);
						break;
					}

					if (phase == TouchPhase.Ended && onTouchUp != null)
					{
						onTouchUp (touch.position);
						break;

					}
				}
			}
		}

		if (!Application.isMobilePlatform) 
		{
			if (Input.GetMouseButtonDown (0) && onTouchDown != null)
				onTouchDown (Input.mousePosition);

			if (Input.GetMouseButtonUp (0) && onTouchUp != null)
				onTouchUp (Input.mousePosition);
		}

	}
}
