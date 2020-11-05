using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using AppAdvisory.social;

public class GameOver : MonoBehaviourHelper
{ 
    public Button btnRestart; // 重新开始   
    public Button btnLeaderboard; // 排行榜  
    public Text txtTitle;
    public CanvasGroup popupCanvasGroup;
   private void Awake()
   {
        popupCanvasGroup.gameObject.SetActive(false);
        txtTitle.text = GameDataLoader.Instance.GetLocaleString("game_over");
        //if (GameDataLoader.Instance.LocaleToUse == "en")
        //{
        //    txtTitle.fontStyle = FontStyle.Normal;
        //}
        //else if (GameDataLoader.Instance.LocaleToUse == "zh")
        //{
        //    txtTitle.fontStyle = FontStyle.Bold;
        //}
    }

    private void AddButtonListeners()
    {
        btnRestart.onClick.AddListener(OnClickedRestart);
        btnLeaderboard.onClick.AddListener(OnClickedLeaderboard);
    }

    private void RemoveButtonListener()
    {
        btnRestart.onClick.RemoveListener(OnClickedRestart);
        btnLeaderboard.onClick.RemoveListener(OnClickedLeaderboard);
    }

    private void OnClickedRestart()
    {      
        canvasManager.ReplayCurrentLevel(canvasManager.lastLevel);
        HidePopup();
    }
  
    private void OnClickedLeaderboard()
    {
        LeaderboardManager.ShowLeaderboardUI();
    }   

    public void PromptPopup()
    {
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        if (aspectRatio == 768f / 1024f || aspectRatio == 1536f / 2048f)
        {
            btnRestart.transform.localScale = new Vector3(0.8f, 0.8f, 1);
        }
        
        int maxLevel = Util.GetMaxLevelUnlock();
        LeaderboardManager.ReportScore(maxLevel);
        if (Util.SoundIsOn()) // 如果当前声音播放，则暂停
        {
            soundManager.pauseAllMusic();
        }
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        popupCanvasGroup.gameObject.SetActive(true);
        AddButtonListeners();
        if(constant.isShowInterstitialAdEnable)
        {
            Ad.Instance.ShowInterstitialAd();
        }
    }   

    /// <summary>
    /// Method to hide the popup
    /// </summary>
    void HidePopup()
    {
        popupCanvasGroup.gameObject.SetActive(false);
        RemoveButtonListener();
        FindObjectOfType<InputTouch>().BLOCK_INPUT = false;
        if(Util.SoundIsOn()) // 如果当前声音播放，则恢复播放声音
        {
            soundManager.resumeAllMusic();
        }
    }

    private void UpdateUI()
    {       
    }


}

