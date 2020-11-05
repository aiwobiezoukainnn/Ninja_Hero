using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetText : MonoBehaviour {

	public string iOSText = "Open Leaderboard";
	public string AndroidText = "Open Leaderboard";

	void Awake()
	{
		#if UNITY_IOS
		GetComponent<Text>().text = iOSText;
		#elif UNITY_ANDROID
		GetComponent<Text>().text = AndroidText;
		#else
		GetComponent<Text>().text = "PLEASE CHANGE YOUR PLATFORM FOR iOS OR ANDROID";
		#endif
	}
}
