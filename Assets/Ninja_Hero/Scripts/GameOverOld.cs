using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GameOverOld : MonoBehaviourHelper
{
    public Button btnShop; // 跳转商店
    public Button btnRestart; // 重新开始
    public Button btnAdFree; // 去广告
    public Button btnLeaderboard; // 排行榜
    public Button btnSave; // 保存进度
    public Button btnAdVideo; // 打开视频广告

    public Text txtNew;
    public Text txtScroe;
    public Text txtTitleScroe;
    public Text txtRecord;
    public Text txtTitleRecord;
    public Text txtTotalCoin;
    public Text txtTitle;
    public Text txtAdVideoTip;    
    public CanvasGroup popupCanvasGroup;
   private void Awake()
   {
        popupCanvasGroup.gameObject.SetActive(false);
        txtTitle.text = GameDataLoader.Instance.GetLocaleString("game_over");
        txtTitleScroe.text = GameDataLoader.Instance.GetLocaleString("score");
        txtTitleRecord.text = GameDataLoader.Instance.GetLocaleString("save_record");
        txtAdVideoTip.text = GameDataLoader.Instance.GetLocaleString("tips_2");
        txtNew.gameObject.SetActive(false);

    }

    private void AddButtonListeners()
    {
        btnShop.onClick.AddListener(OnClickedShop);
        btnRestart.onClick.AddListener(OnClickedRestart);
        btnAdFree.onClick.AddListener(OnClickedAdFree);
        btnLeaderboard.onClick.AddListener(OnClickedLeaderboard);
        btnSave.onClick.AddListener(OnClickedSave);
        btnAdVideo.onClick.AddListener(OnClickedAdVideo);
    }

    private void RemoveButtonListener()
    {
        btnShop.onClick.RemoveListener(OnClickedShop);
        btnRestart.onClick.RemoveListener(OnClickedRestart);
        btnAdFree.onClick.RemoveListener(OnClickedAdFree);
        btnLeaderboard.onClick.RemoveListener(OnClickedLeaderboard);
        btnSave.onClick.RemoveListener(OnClickedSave);
        btnAdVideo.onClick.RemoveListener(OnClickedAdVideo);
    }


    public void OnClickedShop()
    {
        shop.PromptPopup(new OnCallBack(this.OnCloseShopCallBack));
    }


    public void OnCloseShopCallBack()
    {
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
    }


    private void OnClickedRestart()
    {
        canvasManager.ReplayCurrentLevel(canvasManager.lastLevel);
        HidePopup();
    }

    /// <summary>
    /// 观看广告视频，获取奖励
    /// </summary>
    /// <returns></returns>
    private int getRewardForWatchAdVideo()
    {
        int coin = UnityEngine.Random.Range(1, 10);
        return coin;
    }

    private void OnClickedAdFree()
    {
    }
    private void OnClickedLeaderboard()
    {
#if APPADVISORY_LEADERBOARD
		//LeaderboardManager.ShowLeaderboardUI();
#else
        Debug.LogWarning("OnClickedOpenLeaderboard : works only on mobile (iOS & Android), with Very Simple Leaderboard : http://u3d.as/qxf");
#endif
    }
    private void OnClickedSave()
    {      
        string descripton = ""; // 提示描述语
        bool isNeedSave = isNeedSaveLevel();
        if(!isNeedSave) // 不需要保存
        {
            descripton = GameDataLoader.Instance.GetLocaleString("tips_5");
            gameTip.PromptPopup(TipType.Normal, descripton, delegate ()
            {
                FindObjectOfType<InputTouch>().BLOCK_INPUT = true;

            });

        }
        else
        {
            bool isDayShared =/* Util.IsDayShared();*/ true;
            if (isDayShared) // 当前已经分享过了
            {
                descripton = GetSaveCostCoinTip();
                gameTip.PromptPopup(TipType.ContainedButtonOK, descripton, delegate ()
                {
                    FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
                    UpdateUI();

                });
            }
            else // 还没分享
            {
                descripton = GameDataLoader.Instance.GetLocaleString("tips_3");
                gameTip.PromptPopup(TipType.ContainedButtonShare, descripton, delegate ()
                {
                    FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
                    UpdateUI();

                });
            }
        }   
    }

    /// <summary>
    /// 是否需要保存进度
    /// </summary>
    /// <returns></returns>
    private bool isNeedSaveLevel()
    {
        int savedLevel = Util.GetLastLevelPlayed(); // 获取已经保存的关卡
        int curLevel = canvasManager.lastLevel; // 当前的关卡
        return savedLevel != curLevel ? true : false;
    }

    /// <summary>
    /// 获取保存进度消耗金币提示语
    /// </summary>
    /// <returns></returns>
    private string GetSaveCostCoinTip()
    {
        string tip = GameDataLoader.Instance.GetLocaleString("tips_4");
        int costCoin = constant.getLevelSaveCostCoin(canvasManager.lastLevel);
        tip = tip.Replace(@"N", costCoin + "");
        return tip;
    }

    private void OnClickedAdVideo()
    {
        if(isPlayAdVideoEnable())
        {

        }
        else
        {
            string descripton = GameDataLoader.Instance.GetLocaleString("tips_6");
            gameTip.PromptPopup(TipType.Normal, descripton, delegate ()
            {
                FindObjectOfType<InputTouch>().BLOCK_INPUT = true;  

            });
        }
    }

    private bool isPlayAdVideoEnable()
    {
        return false;
    }

    public void PromptPopup()
    {
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        popupCanvasGroup.gameObject.SetActive(true);
        AddButtonListeners();
        UpdateUI();
        checkIsDayShareEnable();
    }

    /// <summary>
    /// 检查当天是否可以分享
    /// </summary>
    public  void checkIsDayShareEnable()
    {
        string curDateTime = System.DateTime.Now.ToShortDateString(); // 当前的年月日
        string lastDateTime = Util.GetDaySharedTime();
        if (curDateTime != lastDateTime) // 每天都有一次分享机会
        {
            lastDateTime = curDateTime;
            Util.SetDayShareTime(lastDateTime);
            Util.SetDayShare(false);
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
    }

    private void UpdateUI()
    {
        if(canvasManager.lastLevel < canvasManager.maxLevel) // 不是最新成绩
        {
            txtNew.gameObject.SetActive(false);
        }
        else
        {
            txtNew.gameObject.SetActive(true);
        }
        txtScroe.text = Convert.ToString(canvasManager.lastLevel); //  当前分数
        txtRecord.text = Convert.ToString(Util.GetLastLevelPlayed()); //  当前分数
        bool isAdFree =/* Util.IsAdFree();*/ true;
        if(isAdFree) // 去广告
        {
            btnAdFree.gameObject.SetActive(false);
        }
        else
        {
            btnAdFree.gameObject.SetActive(true);
        }
        txtTotalCoin.text = Convert.ToString(Util.GetCoin());
    }


}

