using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;
#if APPADVISORY_LEADERBOARD
using AppAdvisory.social;
#endif
#if APPADVISORY_ADS
using AppAdvisory.Ads;
#endif
/// <summary>
/// In Charge to display and managed all the UI elements in the game
/// </summary>
public class CanvasManager : MonoBehaviourHelper
{
	public int numberOfPlayToShowInterstitial = 5;

	public string VerySimpleAdsURL = "http://u3d.as/oWD";

	AudioSource _music;
	/// <summary>
	/// Reference to the music AudioSource
	/// </summary>
	public AudioSource music
	{
		get 
		{
			if (_music == null)
				_music = Camera.main.GetComponentInChildren<AudioSource> ();

			return _music;
		}
	}

	public Text levelText;
	public Button buttonNextLevel;
	public Button buttonLastLevel;
	public Button buttonSetting;
	public Button buttonUnlock;
	public Button buttonLike;
	public Button buttonLeaderboard;
	public Button buttonRate;
	public Button buttonShare;
	public Button buttonMoreGames;
	public Button buttonSound;
    public Image  panel;
    public GameObject buttonSlider;


    private GameObject guideTip;
    public int lastLevel = 1;
    public static CanvasManager Instance = null;

	/// <summary>
	/// Get the max level the player could play. A level is playable if the player unlock the previous level. for exemple: to player the level 10, the player have to cleared the level 
	/// </summary>
	public int maxLevel
	{
		get 
		{
			return Util.GetMaxLevelUnlock();
		}
	}
	/// <summary>
	/// Get the last level the player played
	/// </summary>
	/*
	int lastLevel
	{
		get 
		{
			return Util.GetLastLevelPlayed();
		}
	}
	*/

    public GameObject GuideTip
    {
        get
        {
            return guideTip;
        }

        set
        {
            guideTip = value;
        }
    }

	/// <summary>
	/// Set all the UI In Game Buttons
	/// </summary>
	void SetButtons()
	{

		buttonLastLevel.onClick.AddListener (() => {
			buttonUnlock.transform.DOKill();
			buttonUnlock.transform.DOScale(Vector3.zero,0.3f);
			Util.SetCountGameOver(0);
			//ButtonLogic ();
			OnClickedButtonPreviousLevel();
			//ButtonLogic ();
		});


		buttonNextLevel.onClick.AddListener (() => {
			//buttonNextLevel.transform.DOKill();
		//	buttonNextLevel.transform.DOScale(Vector3.zero,0.3f);
			Util.SetCountGameOver(0);
		//	ButtonLogic ();
			OnClickedButtonNextLevel();
			//ButtonLogic ();
		});


		foreach (Transform t in buttonSetting.transform.parent) 
		{
			if (t.GetComponent<Canvas> () != null)
				t.GetComponent<Canvas> ().sortingOrder = 10 - t.GetSiblingIndex ();
		}

		var gridLayoutGroup = buttonSetting.GetComponentInParent<GridLayoutGroup>();
		gridLayoutGroup.spacing = new Vector2(0,-43);

		buttonSetting.onClick.AddListener (() => {

			buttonSetting.enabled = false;

			float startvalue = 10;
			float endvalue = -43;

			if(gridLayoutGroup.spacing.y == -43)
			{
				startvalue = -43;
				endvalue = 10;

				buttonSetting.transform.DORotate ( new Vector3(0, 0, 360), 1, RotateMode.FastBeyond360);
			}
			else
			{
				buttonSetting.transform.DORotate ( new Vector3(0, 0, -360), 1, RotateMode.FastBeyond360);
			}



			DOVirtual.Float(startvalue, endvalue, 1, (float value) => {
				gridLayoutGroup.spacing = new Vector2(0,value);
			}).OnComplete(() => {
				buttonSetting.enabled = true;
			});
		});

		buttonUnlock.onClick.AddListener (() => {
			buttonUnlock.transform.DOScale(Vector3.zero,0.3f);
			ShowRewardedVideoGameOver();
		});

		buttonUnlock.transform.localScale = Vector3.zero;


		buttonLike.onClick.AddListener (() => {
			string facebookApp = "fb://profile/515431001924232" ;
			string facebookAddress = "https://www.facebook.com/appadvisory";

			float startTime;
			startTime = Time.timeSinceLevelLoad;

			Application.OpenURL(facebookApp);

			if (Time.timeSinceLevelLoad - startTime <= 1f)
			{
				Application.OpenURL(facebookAddress);
			}

		});
        buttonLike.gameObject.SetActive(false);

		buttonLeaderboard.onClick.AddListener (() => {

			OnClickedOpenLeaderboard();

		});
        buttonLeaderboard.gameObject.SetActive(false);


		buttonRate.onClick.AddListener (() => {
            Application.OpenURL(FindObjectOfType<RateUsManager>().iOSURL);             
		});

#if UNITY_IPHONE
       buttonRate.gameObject.SetActive(false); // 只在ios上显示评论功能
#else
       buttonRate.gameObject.SetActive(false); 
#endif


		buttonShare.onClick.AddListener (() => {
			Debug.LogWarning("PUT YOUR CODE HERE");
		});
        buttonShare.gameObject.SetActive(false);



		buttonMoreGames.onClick.AddListener (() => {
			//Application.OpenURL ("https://barouch.fr/moregames.php");
			Application.OpenURL ("http://112.74.128.108/ios/index.html");
		});
        buttonMoreGames.gameObject.SetActive(false);


        if (!Util.SoundIsOn()) 
		{
			music.Stop ();
			buttonSound.transform.GetChild (0).gameObject.SetActive (false);
			buttonSound.transform.GetChild (1).gameObject.SetActive (true);
		}
		else 
		{
			music.Play ();
			buttonSound.transform.GetChild (0).gameObject.SetActive (true);
			buttonSound.transform.GetChild (1).gameObject.SetActive (false);
		}

		buttonSound.onClick.AddListener (() => {
			TurnSound();
		});

        GuideTip = GameObject.Find("GuideTip");
        bool isShowGuide = Util.GetIsShowGuide(); // 获取是否显示引导
        if (isShowGuide)
        {
            GuideTip.SetActive(true);
            Text title = GuideTip.transform.FindChild("Bg/Title").GetComponent<Text>();
            title.text = GameDataLoader.Instance.GetLocaleString("tips");

            Text Description = GuideTip.transform.FindChild("Bg/Description").GetComponent<Text>();
            Description.text = GameDataLoader.Instance.GetLocaleString("tips_1");
        }
        else
        {
            GuideTip.SetActive(false);
        }
    }
	/// <summary>
	/// If player clics on the leaderbord button, we call this method. Works only on mobile (iOS & Android) if using Very Simple Leaderboard by App Advisory : http://u3d.as/qxf
	/// </summary>
	public void OnClickedOpenLeaderboard()
	{
		#if APPADVISORY_LEADERBOARD
		LeaderboardManager.ShowLeaderboardUI();
		#else
		Debug.LogWarning("OnClickedOpenLeaderboard : works only on mobile (iOS & Android), with Very Simple Leaderboard : http://u3d.as/qxf");
		#endif
	}
	/// <summary>
	/// Turn on/off the sounds in the game
	/// </summary>
	void TurnSound()
	{
		if (Util.SoundIsOn()) 
		{
			music.Stop ();
			Util.SetSoundOff();
			buttonSound.transform.GetChild (0).gameObject.SetActive (false);
			buttonSound.transform.GetChild (1).gameObject.SetActive (true);
		}
		else 
		{
			music.Play ();
			Util.SetSoundOn();
			buttonSound.transform.GetChild (0).gameObject.SetActive (true);
			buttonSound.transform.GetChild (1).gameObject.SetActive (false);
		}
			
		PlayerPrefs.Save();
	}

	void Awake()
	{
        Instance = this;
        lastLevel = Util.GetLastLevelPlayed();
        DOTween.Init ();


		SetButtons ();

		//ButtonLogic ();

        adjustUiForScreenSize();

    }

    /// <summary>
    /// 针对各个分辨率UI调整
    /// </summary>
    private void adjustUiForScreenSize()
    {
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        if (aspectRatio == 320f / 480f ||
            aspectRatio == 640f / 960f ||
            aspectRatio == 640f / 1136f ||
            aspectRatio == 750f / 1334f ||
            aspectRatio == 1242f / 2208f
            )
        {
            float offsetHeight = 20f;
            float offsetY = offsetHeight / 2f;          
            panel.rectTransform.sizeDelta = new Vector2(panel.rectTransform.rect.width,
                                                        panel.rectTransform.rect.height + offsetHeight);
            buttonNextLevel.transform.localScale = new Vector3(1, 1, 1);
            buttonNextLevel.transform.localPosition = new Vector3(buttonNextLevel.transform.localPosition.x,
                                                                  buttonNextLevel.transform.localPosition.y - offsetY,
                                                                  buttonNextLevel.transform.localPosition.z);
            levelText.transform.localPosition = new Vector3(levelText.transform.localPosition.x,
                                                            levelText.transform.localPosition.y - offsetY + 3,
                                                            levelText.transform.localPosition.z);
            buttonLastLevel.transform.localScale = new Vector3(1, 1, 1);
            buttonLastLevel.transform.localPosition = new Vector3(buttonLastLevel.transform.localPosition.x,
                                                                  buttonLastLevel.transform.localPosition.y - offsetY,
                                                                  buttonLastLevel.transform.localPosition.z);

            buttonSlider.transform.localScale = new Vector3(1.2f, 1.2f, 1);
            buttonSlider.transform.localPosition = new Vector3(buttonSlider.transform.localPosition.x,
                                                               buttonSlider.transform.localPosition.y - offsetY + 6,
                                                               buttonSlider.transform.localPosition.z);
        }

      

     
    }

    /// <summary>
    /// Show rewarded video at game over
    /// </summary>
    private void ShowRewardedVideoGameOver()
	{
		gameManager.success = false;
		gameManager.isGameOver = false;

		#if APPADVISORY_ADS
		if(AdsManager.instance.IsReadyRewardedVideo())
		{
			AdsManager.instance.ShowRewardedVideo ((bool success) => {
				if(success)
					PlayNextLevel ();
			});
		}
		#endif
	}


	/// <summary>
	/// Display the next and/or last button (the arrow around the level at the top of the screen)
	/// </summary>
	public void ButtonLogic()
	{
//		if (gameManager.isGameOver || gameManager.success) 
//		{
//			SetButtonActive(buttonLastLevel,false);
//			SetButtonActive(buttonNextLevel,false);
//			return;
//		}

		SetButtonActive(buttonLastLevel, Util.ActivateButtonLast());

	    SetButtonActive(buttonNextLevel, Util.ActivateButtonNext());

	}
	/// <summary>
	/// Activate and enable - or not - buttons
	/// </summary>
	void SetButtonActive(Button b,bool isActive)
	{
		if (isActive) 
		{
            b.gameObject.SetActive(true);

           // b.GetComponent<Image> ().color = new Color(b.GetComponent<Image> ().color.r,b.GetComponent<Image> ().color.g,b.GetComponent<Image> ().color.b,1);
			//b.interactable = true;
          
        }
		else 
		{
            b.gameObject.SetActive(false);

            //b.GetComponent<Image> ().color = new Color(b.GetComponent<Image> ().color.r,b.GetComponent<Image> ().color.g,b.GetComponent<Image> ().color.b,0);
            //b.interactable = false;
        }
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        if (aspectRatio == 320f / 480f ||
            aspectRatio == 640f / 960f ||
            aspectRatio == 640f / 1136f ||
            aspectRatio == 750f / 1334f ||
            aspectRatio == 1242f / 2208f
            )
        {
            b.transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            b.transform.localScale = new Vector3(0.7f, 0.7f, 1f);
        }

    }

	IEnumerator Start()
	{
		yield return new WaitForSeconds (0.1f);

		PlayLevel (lastLevel);
	}
	/// <summary>
	/// When the player failed, we show an unlock button ONLY IF there is a rewarded video available
	/// </summary>
	void ShowButtonUnlock()
	{
		#if APPADVISORY_ADS
		if (AdsManager.instance.IsReadyRewardedVideo()) 
		{
			if (buttonUnlock.transform.localScale.x == 1) 
			{
				buttonUnlock.transform.DOScale (Vector3.one * 1.5f, 0.3f).SetLoops (6, LoopType.Yoyo);
			}
			else
			{
				buttonUnlock.transform.DOScale (Vector3.one, 0.3f);
			}
		} 
		return;
		#endif
	}



	/// <summary>
	/// Animation when the player fails 
	/// </summary>
	public void AnimationCameraGameOver(Vector3 impactPosition)
	{

		// FindObjectOfType<RateUsManager>().CheckIfPromptRateDialogue();

		ShowAds();

		ShowButtonUnlock();

		ReplayCurrentLevel (lastLevel);
	}
	/// <summary>
	/// Animation when the player cleared a level 
	/// </summary>
	public void AnimationCameraSuccess()
	{
		Util.SetCountGameOver(0);

		FindObjectOfType<RateUsManager>().CheckIfPromptRateDialogue();

		ShowAds();

		buttonUnlock.transform.DOScale(Vector3.zero,0.3f);

		PlayNextLevel ();
	}
	/// <summary>
	/// Run the level logic on the UI side
	/// </summary>
	private void PlayLevel(int level)
	{
		ReportScoreToLeaderboard(level);

		levelText.text = GameDataLoader.Instance.GetLocaleString("level") + " " + level.ToString() /*+ " / 1200"*/;

        if (level > maxLevel)
        {
            if (!Util.IS_TEST_ENABLE) // 测试开关没有打开
            {
                Util.SetMaxLevelUnlock(level);
            }
        }

		Util.SetLastLevelPlayed(level); 

		//ButtonLogic ();

		gameManager.CreateGame (level);


	}
	/// <summary>
	/// Method called when the player clicked on the left arrow on the left of the level text on the top of the screen during the game
	/// </summary>
	private void OnClickedButtonPreviousLevel()
	{
		int last = lastLevel;

		last--;

		if (last < 1)
			last = 1;
        lastLevel = last;


		levelText.text = "Level " + last.ToString();
		//			levelTextMesh.text = last.ToString();
		PlayLevel (last);


	}
	/// <summary>
	/// Method called when the player clicked on the right arrow on the roght of the level text on the top of the screen during the game
	/// </summary>
	private void OnClickedButtonNextLevel()
	{

		PlayNextLevel ();



	}
	/// <summary>
	/// Method called when the player failed and so ... we replay the current level
	/// </summary>
	public void ReplayCurrentLevel(int level)
	{
		Camera.main.transform.DOMove (new Vector3 (0, Camera.main.transform.position.y, -10), 0.3f).OnComplete (() => {
			PlayLevel (level);
		});

	}
	/// <summary>
	/// Method called when the player have to play the next level (if the current level is cleared, or if the payer taps/Clicks on the next button or if the player see a rewarded video to unlock the current level
	/// </summary>
	private void PlayNextLevel()
	{
		int last = lastLevel;

		last++;
        lastLevel = last;


		levelText.text = "Level " + last.ToString();

		PlayLevel (last);

	}
	/// <summary>
	/// If using Very Simple Leaderboard by App Advisory, report the score : http://u3d.as/qxf
	/// </summary>
	void ReportScoreToLeaderboard(int p)
	{
		#if APPADVISORY_LEADERBOARD
		LeaderboardManager.ReportScore(p);
		#else
		print("Get very simple leaderboard to use it : http://u3d.as/qxf");
		#endif
	}
	/// <summary>
	/// If using Very Simple Ads by App Advisory, show an interstitial if number of play > numberOfPlayToShowInterstitial: http://u3d.as/oWD
	/// </summary>
	public void ShowAds()
	{
		int count = PlayerPrefs.GetInt("GAMEOVER_COUNT",0);
		count++;
		PlayerPrefs.SetInt("GAMEOVER_COUNT",count);
		PlayerPrefs.Save();

		#if APPADVISORY_ADS
		if(count > numberOfPlayToShowInterstitial)
		{
		#if UNITY_EDITOR
			print("count = " + count + " > numberOfPlayToShowINterstitial = " + numberOfPlayToShowInterstitial);
		#endif
			if(AdsManager.instance.IsReadyInterstitial())
			{
		#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == true ----> SO ====> set count = 0 AND show interstial");
		#endif
				PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
				AdsManager.instance.ShowInterstitial();
			}
			else
			{
		#if UNITY_EDITOR
				print("AdsManager.instance.IsReadyInterstitial() == false");
		#endif
			}

		}
		else
		{
			PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
		#else
		if(count >= numberOfPlayToShowInterstitial)
		{
		Debug.LogWarning("To show ads, please have a look to Very Simple Ad on the Asset Store, or go to this link: " + VerySimpleAdsURL);
		Debug.LogWarning("Very Simple Ad is already implemented in this asset");
		Debug.LogWarning("Just import the package and you are ready to use it and monetize your game!");
		Debug.LogWarning("Very Simple Ad : " + VerySimpleAdsURL);
		PlayerPrefs.SetInt("GAMEOVER_COUNT",0);
		}
		else
		{
		PlayerPrefs.SetInt("GAMEOVER_COUNT", count);
		}
		PlayerPrefs.Save();
		#endif
	}
}
