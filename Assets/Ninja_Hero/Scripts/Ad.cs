using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using admob;

public class Ad : MonoBehaviourHelper{
    private bool isInited = false; // 是否已经初始化
    protected static Ad instance = null;
    public string bannerAdID = "ca-app-pub-6054433567181790/4265635661";
    public string interstitialAdID = "ca-app-pub-6054433567181790/7219102060";
    public string rewardedVideoID = "ca-app-pub-6054433567181790/8695835267";


    private void Awake()
   {
        instance = this;

    }

    private void Start()
    {
        if(Util.IS_TEST_ENABLE) // 测试开关打开
        {
            bannerAdID = "ca-app-pub-2400683233491217/1944087282";
            interstitialAdID = "ca-app-pub-2400683233491217/4897553689";
        }
        Admob.Instance().bannerEventHandler += onBannerEvent;
        Admob.Instance().interstitialEventHandler += onInterstitialEvent;
        Admob.Instance().rewardedVideoEventHandler += onRewardedVideoEvent;
        InitAd();       
    }

    public static Ad Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new Ad();
            }
            instance.InitAd();
            return instance;
        }
        set
        {
            instance = value;
        }
    }

    /// <summary>
    /// 初始化广告
    /// </summary>
    private void InitAd()
    {
        if (!isInited)
        {
            isInited = true;           
            Admob.Instance().initAdmob(bannerAdID, interstitialAdID);
            Admob.Instance().loadInterstitial();
            Admob.Instance().loadRewardedVideo(rewardedVideoID);
            ShowBannerAd(true);
        }
    }
  
    /// <summary>
    /// 显示插屏广告
    /// </summary>
    public void ShowInterstitialAd()
    {
        if (Admob.Instance().isInterstitialReady())
        {
            Admob.Instance().showInterstitial();
        }
        else
        {
            Admob.Instance().loadInterstitial();
        }


    } 

    /// <summary>
    /// 显示Banner广告
    /// </summary>
    /// <param name="isShow"></param>
    public void ShowBannerAd(bool isShow)
    {
        if (isShow)
        {
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);
        }
        else
        {
            Admob.Instance().removeBanner();
        }
    }

    /// <summary>
    /// 显示收入视频广告
    /// </summary>
    public void ShowRewardVideoAd()
    {
        if (Admob.Instance().isRewardedVideoReady())
        {
            Admob.Instance().showRewardedVideo();
        }else
        {
            Admob.Instance().loadRewardedVideo(rewardedVideoID);
        }
    }

    void onInterstitialEvent(string eventName, string msg)
    {
        if (eventName == AdmobEvent.onAdClosed)
        {
            Admob.Instance().loadInterstitial();
            constant.isShowInterstitialAdEnable = false;
            constant.isNeedRevokeTimer = true; // 启动触发器
        }
    }
    void onBannerEvent(string eventName, string msg)
    {
        if (eventName == AdmobEvent.onAdLoaded)
        {
            Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.BOTTOM_CENTER, 0);
        }
    }
    void onRewardedVideoEvent(string eventName, string msg)
    {
        if (eventName == AdmobEvent.onAdClosed)
        {
            Admob.Instance().loadRewardedVideo(rewardedVideoID);
        }
    }
}

