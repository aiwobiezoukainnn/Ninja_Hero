using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public class Constant : MonoBehaviourHelper
{
	string shareMessage;
	public string url;
	public Texture2D smallIcon;
    public int startInterstitialAdTime = 2 * 60; // 显示插屏随机开始时间默认2分钟
    public int endInterstitialAdTime = 3 * 60; // 显示插屏随机结束时间默认3分钟
    public bool isShowInterstitialAdEnable = true; // 是否可以显示插屏广告
    public bool isNeedRevokeTimer = false; // 是否需要启动定时器
    public Color FailColor;
	public Color SuccessColor;
	public Color BackgroundColor;
	public List<Color> backgroundColors = new List<Color>();

    /// <summary>
    /// 全部关卡复活消耗的金币配置
    /// 第一行表示1到15关卡，1到5次复活消耗所需要的复活金币，其它类推
    /// </summary>
    public int[,] revival_cost_coins = 
    {
        {1, 2, 3, 4, 5 }, // 1-15
        {2, 3, 5, 8, 10 }, // 16-30
        {3, 4, 7, 11, 15 }, // 31-50
        {4, 5, 9, 14, 20 }, // 51-100
        {5, 6, 11, 18, 25 }, // 101-200
        {6, 7, 13, 21, 30 }, // 201-500
        {8, 10, 18, 28, 40 }, // 501-800
        {10, 12, 22, 35, 50 }, // 801-1200
    };

    /// <summary>
    /// 保存关卡所需要金币
    /// </summary>
    public int[] save_cost_coins =
   {
        5, // 1-15
        10, // 16-30
        15, // 31-50
        20, // 51-100
        25, // 101-200
        30, // 201-500
        35, // 501-800
        40, // 801-1200
     };

    /// <summary>
    ///  商店列表项购买所需要的价格
    /// </summary>
    public int []shop_items_price = { 6, 30, 68, 6};

    /// <summary>
    /// 购买商品赠送金币, 注:最后一项0是去广告，没有赠送金币
    /// </summary>
    public int[] shop_buy_coins = { 60, 388, 888, 0};


    /// <summary>
    /// 获取复活所需要的金币， 
    /// </summary>
    /// <param name="level"></param> 关卡 从1开始
    /// <param name="revivalIndex"></param> 第几次复活 从1开始
    /// <returns></returns>
    public int getLevelRevivalCostCoin(int level, int revivalIndex)
    {
        int rowIndex = 0; 
        int columIndex = 0;
        if(revivalIndex > 5)
        {
            revivalIndex = 5;
        }

        columIndex = revivalIndex - 1;
        if(level <= 15)
        {
            rowIndex = 0;
        }
        else if(level <= 30)
        {
            rowIndex = 1;
        }
        else if (level <= 50)
        {
            rowIndex = 2;
        }
        else if (level <= 100)
        {
            rowIndex = 3;
        }
        else if (level <= 200)
        {
            rowIndex = 4;
        }
        else if (level <= 500)
        {
            rowIndex = 5;
        }
        else if (level <= 800)
        {
            rowIndex = 6;
        }
        else if (level <= 1200)
        {
            rowIndex = 7;
        }

        return revival_cost_coins[rowIndex, columIndex];
    }

    /// <summary>
    /// 获取每个关卡保存所需要的金币
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public int getLevelSaveCostCoin(int level)
    {
        int rowIndex = 0;
        if (level <= 15)
        {
            rowIndex = 0;
        }
        else if (level <= 30)
        {
            rowIndex = 1;
        }
        else if (level <= 50)
        {
            rowIndex = 2;
        }
        else if (level <= 100)
        {
            rowIndex = 3;
        }
        else if (level <= 200)
        {
            rowIndex = 4;
        }
        else if (level <= 500)
        {
            rowIndex = 5;
        }
        else if (level <= 800)
        {
            rowIndex = 6;
        }
        else if (level <= 1200)
        {
            rowIndex = 7;
        }

        return save_cost_coins[rowIndex];
    }

    private  void Update()
    {     
        if(isNeedRevokeTimer)
        {
            isNeedRevokeTimer = false;
            int deltaSeconds = UnityEngine.Random.Range(startInterstitialAdTime, endInterstitialAdTime + 1);
            Invoke("resetShowInterstitialAdFlag", deltaSeconds);
        }
    }

    private void resetShowInterstitialAdFlag()
    {
        isShowInterstitialAdEnable = true;
    }

public string GetShareMessage(){

		shareMessage = 
			"I'm on level "
			+ Util.GetMaxLevelUnlock()
			+ "! #"
			+ "Ninja Hero"
			+ " by #appadvisory \n ";


		return shareMessage;
	}

	public Color RandomBrightColor()
	{
		if (backgroundColors == null || backgroundColors.Count == 0)
			return Color.white;

		return backgroundColors[UnityEngine.Random.Range(0,backgroundColors.Count)];
	}
}
