using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppAdvisory.social;

public class PostScore : MonoBehaviour 
{
	public int score = 10;

	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClicked);
	}

	void OnClicked()
	{
		LeaderboardManager.ReportScore(score);
	}
}
