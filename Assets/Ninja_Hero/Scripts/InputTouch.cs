using UnityEngine;
using System.Collections;

/// <summary>
/// Class in of the input (mobile, web and desktop) in the game
/// </summary>
public class InputTouch : MonoBehaviour
{
	/// <summary>
	/// Delegate subscribe by the GameManager and trigger when the player make a touch/click
	/// </summary>
	public delegate void OnTouchDown(Vector3 pos);
	public static event OnTouchDown onTouchDown;

	/// <summary>
	/// Delegate subscribe by the GameManager and trigger when the player release the touch/click
	/// </summary>
	public delegate void OnTouchUp(Vector3 pos);
	public static event OnTouchDown onTouchUp;


	/// <summary>
	/// To block input when showing the rate us popup
	/// </summary>
	public bool BLOCK_INPUT = false;

	/// <summary>
	/// Listening for inputs
	/// </summary>
	void Update () 
	{
		if(BLOCK_INPUT)
			return;

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
