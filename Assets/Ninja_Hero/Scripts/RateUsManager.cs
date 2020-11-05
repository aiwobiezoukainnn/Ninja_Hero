using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// Class in charge to prompt a popup to ask the player to rate the game
///
/// Attached to the "RateUsManager" GameObject
/// </summary>
public class RateUsManager : MonoBehaviour 
{
	/// <summary>
	/// The number of level played (each win or lose count for 1 play) to prompt the popup
	/// </summary>
	public int NumberOfLevelPlayedToShowRateUs = 30;
	public string iOSURL = "https://itunes.apple.com/us/app/bie-peng-hei-kuai-er-dan-ji/id1124801492?l=zh&ls=1&mt=8";
	public string ANDROIDURL = "http://app-advisory.com";

	public Button btnYes;
	public Button btnLater;
	public Button btnNever;

	public CanvasGroup popupCanvasGroup;

	void Awake()
	{
		popupCanvasGroup.alpha = 0;
		popupCanvasGroup.gameObject.SetActive(false);

        Text rateUsDesc = popupCanvasGroup.transform.FindChild("Image/Text").GetComponent<Text>();
        rateUsDesc.text = GameDataLoader.Instance.GetLocaleString("rate_desc");

        Text btnYesTitle = btnYes.transform.FindChild("Text").GetComponent<Text>();
        btnYesTitle.text = GameDataLoader.Instance.GetLocaleString("ok");

        Text btnLaterTitle = btnLater.transform.FindChild("Text").GetComponent<Text>();
        btnLaterTitle.text = GameDataLoader.Instance.GetLocaleString("later");


        Text btnNeverTitle = btnNever.transform.FindChild("Text").GetComponent<Text>();
        btnNeverTitle.text = GameDataLoader.Instance.GetLocaleString("cancel");
    }

	void AddButtonListeners()
	{
		btnYes.onClick.AddListener(OnClickedYes);
		btnLater.onClick.AddListener(OnClickedLater);
		btnNever.onClick.AddListener(OnClickedNever);
	}

	void RemoveButtonListener()
	{
		btnYes.onClick.RemoveListener(OnClickedYes);
		btnLater.onClick.RemoveListener(OnClickedLater);
		btnNever.onClick.RemoveListener(OnClickedNever);
	}

	/// <summary>
	/// Method called if the player clicked on the YES button. If the player do that, we will never prompt again the popup
	/// </summary>
	void OnClickedYes()
	{
		#if UNITY_IPHONE
		Application.OpenURL(iOSURL);
		#endif

		#if UNITY_ANDROID
		Application.OpenURL(ANDROIDURL);
		#endif

		PlayerPrefs.SetInt("NUMOFLEVELPLAYED",-1);
		PlayerPrefs.Save();
		HidePopup();
	}

	/// <summary>
	/// Method called if the player clicked on the LATER button. If the player do that, we will ask again in "NumberOfLevelPlayedToShowRateUs"
	/// </summary>
	void OnClickedLater()
	{
		Util.SetNumberOfLevelPLayed(0);
		HidePopup();
	}
	/// <summary>
	/// Method called if the player clicked on the NEVER button. If the player do that, we will never prompt again the popup
	/// </summary>
	void OnClickedNever()
	{
		Util.SetNumberOfLevelPLayed(-1);
		HidePopup();
	}
	/// <summary>
	/// Check if we need to prompt the popup or not
	/// </summary>
	public void CheckIfPromptRateDialogue()
	{
		int count = PlayerPrefs.GetInt("NUMOFLEVELPLAYED",0);

		if(count == -1)
			return;

		count ++;

		if(count > NumberOfLevelPlayedToShowRateUs)
		{
			PromptPopup();
		}
		else
		{
			Util.SetNumberOfLevelPLayed(count);
		}

		PlayerPrefs.Save();
	}
	/// <summary>
	/// Method to prompt the popup
	/// </summary>
	public void PromptPopup()
	{
		Time.timeScale = 0;
		FindObjectOfType<InputTouch>().BLOCK_INPUT = true;

		popupCanvasGroup.alpha = 0;
		popupCanvasGroup.gameObject.SetActive(true);

		popupCanvasGroup.DOFade(1, 1)
			.SetUpdate(true)
			.OnComplete( () => {
				Time.timeScale = 0;
				AddButtonListeners();
			});
	}
	/// <summary>
	/// Method to hide the popup
	/// </summary>
	void HidePopup()
	{
		Time.timeScale = 0;
		popupCanvasGroup.alpha = 1;
		popupCanvasGroup.gameObject.SetActive(true);
		popupCanvasGroup.DOFade(0, 1)
			.SetUpdate(true)
			.OnComplete( () => {
				Time.timeScale = 1;
				popupCanvasGroup.gameObject.SetActive(false);
				RemoveButtonListener();
				FindObjectOfType<InputTouch>().BLOCK_INPUT = false;
			});
	}
}

