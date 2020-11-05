using UnityEngine;
using System.Collections;
#if UNITY_5_3
using UnityEngine.SceneManagement;
#endif

public static class Util
{
	public static string COUNTGAMEOVER = "COUNTGAMEOVER";
	public static string LAST_LEVEL_PLAYED = "LEVEL_PLAYED";
	public static string LEVEL_UNLOCKED = "LEVEL";
	public static string SOUND_ON = "SOUND_ON";
	public static string NUMOFLEVELPLAYED = "NUMOFLEVELPLAYED";
    public static string IS_SHOW_GUIDE = "IS_SHOW_GUIDE";	// 是否显示引导
    public static string COIN = "COIN"; // 金币
    public static string AD_FREE = "AD_FREE"; //去广告
    public static string IS_DAY_SHARED = "IS_DAY_SHARED"; // 当天是否已经分享
    public static string DAY_SHARED_TIME = "DAY_SHARED_TIME"; // 当前分享时间，用作每天清空判断
    // 测试使用的配置参数
    public static bool IS_TEST_ENABLE = false;

	public static void SetCountGameOver(int count)
	{
		PlayerPrefs.SetInt(COUNTGAMEOVER, count);
		PlayerPrefs.Save();
	}

    public static bool GetIsShowGuide()
    {
        bool isShowGuide = PlayerPrefs.GetInt(IS_SHOW_GUIDE, 1) == 1 ? true : false;
        return isShowGuide;
    }

    public static void SetIsShowGuide(bool isShowGuide)
    {
        int bRet = isShowGuide == true ? 1 : 0;
        PlayerPrefs.SetInt(IS_SHOW_GUIDE, bRet);
        PlayerPrefs.Save();
     }

    public static int GetCoin()
    {
        int defaultValue = 100;
        if (IS_TEST_ENABLE)
        {
            defaultValue = 999;
        }       
        int coin = PlayerPrefs.GetInt(COIN, defaultValue);
        return coin;
    }

    public static void SetCoin(int coin)
    {
        PlayerPrefs.SetInt(COIN, coin);
        PlayerPrefs.Save();
    }


    /// <summary>
    /// 获取每个关卡复活次数
    /// </summary>
    /// <param name="leveName">关卡名</param>
    /// <returns></returns>
    public static int GetRevivalLevelTime(string leveName)
    {
       
        int revivalTime = PlayerPrefs.GetInt(leveName, 1);
        return revivalTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="levelName">关卡名</param>
    /// <param name="revivalTime">复活次数</param> 
    public static void SetRevivalLevelTime(string levelName, int revivalTime)
    {
        PlayerPrefs.SetInt(levelName, revivalTime);
        PlayerPrefs.Save();
    }



    public static bool IsAdFree()
    {
        bool bRet = PlayerPrefs.GetInt(AD_FREE, 0) == 1 ? true : false;
        return false;
    }

    public static void SetAdFree(bool isAdFree)
    {
        int bRet = isAdFree == true ? 1 : 0;
        PlayerPrefs.SetInt(AD_FREE, bRet);
        PlayerPrefs.Save();
    }



    public static bool IsDayShared()
    {
        bool bRet = PlayerPrefs.GetInt(IS_DAY_SHARED, 0) == 1 ? true : false;
        return bRet;
    }

    public static void SetDayShare(bool isDayShare)
    {
        int bRet = isDayShare == true ? 1 : 0;
        PlayerPrefs.SetInt(IS_DAY_SHARED, bRet);
        PlayerPrefs.Save();
    }


    public static string  GetDaySharedTime()
    {
        string dayShareTime = PlayerPrefs.GetString(DAY_SHARED_TIME, "yyyyMMdd");
        return dayShareTime;
    }

    public static void SetDayShareTime(string dayShareTime)
    {
        PlayerPrefs.SetString(DAY_SHARED_TIME, dayShareTime);
        PlayerPrefs.Save();
    }
  
    public static int GetCountGameOver()
	{
		return PlayerPrefs.GetInt(COUNTGAMEOVER, 0);
	}

	public static void SetMaxLevelUnlock(int num)
	{
		PlayerPrefs.SetInt(LEVEL_UNLOCKED, num);
		PlayerPrefs.Save();
	}

	public static int GetMaxLevelUnlock()
	{
        if(IS_TEST_ENABLE) // 测试，关卡全部打开
        {
            return PlayerPrefs.GetInt(LEVEL_UNLOCKED, Level.maxLevel);

        }
        return PlayerPrefs.GetInt(LEVEL_UNLOCKED, 1);
	}

	public static void SetLastLevelPlayed(int num)
	{
		PlayerPrefs.SetInt(LAST_LEVEL_PLAYED, num);
		PlayerPrefs.Save();
	}

	public static int GetLastLevelPlayed()
	{
		return PlayerPrefs.GetInt(LAST_LEVEL_PLAYED, 1);
	}

	public static void SetSound(bool ON)
	{
		if(ON)
			SetSoundOn();
		else
			SetSoundOff();
	}

	public static void SetSoundOn()
	{
		PlayerPrefs.SetInt(SOUND_ON, 1);
		PlayerPrefs.Save();
	}

	public static void SetSoundOff()
	{
		PlayerPrefs.SetInt(SOUND_ON, 0);
		PlayerPrefs.Save();
	}

	public static bool SoundIsOn()
	{
		return PlayerPrefs.GetInt(SOUND_ON,1) == 1;
	}

	public static void ReloadCurrentLevel()
	{
		#if UNITY_5_3
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		#else
		Application.LoadLevel (Application.loadedLevel);
		#endif
	}

	public static string GetCurrentLevelName()
	{
		#if UNITY_5_3
		return SceneManager.GetActiveScene().name;
		#else
		return Application.loadedLevelName;
		#endif
	}

	public static void SetNumberOfLevelPLayed(int num)
	{
		PlayerPrefs.SetInt(NUMOFLEVELPLAYED, num);
		PlayerPrefs.Save();
	}

	public static int GetNumberOfLevelPLayed()
	{
		return PlayerPrefs.GetInt(NUMOFLEVELPLAYED, 0);
	}

	public static bool ActivateButtonNext()
	{
		//int currentLevel = GetLastLevelPlayed();
        int currentLevel = CanvasManager.Instance.lastLevel;
		int max = GetMaxLevelUnlock();
	
		bool canUnlock = false;

		if(currentLevel < max)
			canUnlock = true;

//		Debug.Log("current = " + currentLevel + " - max = " + max + " ---> canUnlock = " + canUnlock);

		return canUnlock;
	}

	public static bool ActivateButtonLast()
	{
		//int currentLevel = GetLastLevelPlayed();
        int currentLevel = CanvasManager.Instance.lastLevel;

		bool canUnlock = false;

		if(currentLevel > 1)
			canUnlock = true;

		return canUnlock;
	}
}
