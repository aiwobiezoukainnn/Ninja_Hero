using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using AppAdvisory.social;
#if UNITY_5_3_OR_NEWER
using UnityEngine.SceneManagement;
#endif

public class ReloadScene : MonoBehaviour
{
	void Awake()
	{
		GetComponent<Button>().onClick.AddListener(OnClicked);
	}

	void OnClicked()
	{
		#if UNITY_5_3_OR_NEWER
		SceneManager.LoadScene(0);
		#else
		Application.LoadLevel(Application.loadedLevel);
		#endif
	}
}
