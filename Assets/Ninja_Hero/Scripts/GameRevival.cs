using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class GameRevival : MonoBehaviourHelper
{
    public Button btnShop;
	public Button btnContinue;
	public Text txtCostCoin;
    public Text txtTitle;
    public Text txtTotalCoin;
    public Image barFill;
    public CanvasGroup popupCanvasGroup;
    int revivalCostCoin; // 复活界面需要的金币
    protected float timer = -1.0f;
    protected float duration = 4.75f;
    private bool isPauseBarAnimation = true; // 是否停止播放进度

	private void Awake()
	{
        popupCanvasGroup.gameObject.SetActive(false);
        txtTitle.text = GameDataLoader.Instance.GetLocaleString("continue");
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        if (aspectRatio == 768f / 1024f ||
            aspectRatio == 1536f / 2048f)        

        {
            btnShop.transform.localPosition = new Vector3(btnShop.transform.localPosition.x,
                                                          btnShop.transform.localPosition.y - 10f,
                                                          btnShop.transform.localPosition.z);
        }
        else if (aspectRatio == 640f / 960f ||
                 aspectRatio == 320f / 640f)
        {
            btnShop.transform.localPosition = new Vector3(btnShop.transform.localPosition.x,
                                                          btnShop.transform.localPosition.y - 35f,
                                                          btnShop.transform.localPosition.z);
        }
    }

	private void AddButtonListeners()
	{
		btnShop.onClick.AddListener(OnClickedShop);
        btnContinue.onClick.AddListener(OnClickedContinue);
	}

	private void RemoveButtonListener()
	{
        btnShop.onClick.RemoveListener(OnClickedShop);
        btnContinue.onClick.RemoveListener(OnClickedContinue);       
	}


	public void OnClickedShop()
	{
      isPauseBarAnimation = true;
      HidePopup();
      shop.PromptPopup(new OnCallBack(this.OnCloseShopCallBack));
    }


    public  void OnCloseShopCallBack()
    {
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        isPauseBarAnimation = false;
        popupCanvasGroup.gameObject.SetActive(true);
        AddButtonListeners();
        UpdateUI();
    }



    private void GotoGameOver()
    {
        if (Mathf.Clamp01(timer / duration) >= 0.25f)
        {
            timer -= duration * 0.25f;
            Vector3 scale = barFill.transform.localScale - Vector3.right * 0.25f;
            scale.x = Mathf.Clamp01(scale.x);
            barFill.transform.localScale = scale;
        }
        HidePopup();
        gameOver.PromptPopup();
    }

    /// <summary>
    /// 点击屏幕，触发，每点击一次，进度条加快
    /// </summary>
    public void OnClickedScreen()
    {
        //if (Mathf.Clamp01(timer / duration) >= 0.25f)
        //{
            timer -= duration * 0.25f;
            float scale = barFill.fillAmount - 1 * 0.25f;
            scale = Mathf.Clamp01(scale);
            barFill.fillAmount = scale;
       // }
    }

    private void OnClickedContinue()
	{
        int totalCoin = Util.GetCoin();
        if(totalCoin < revivalCostCoin) // 金币不足
        {
            OnClickedShop(); 
        }
        else
        {
            String levelName = "LEVEL" + canvasManager.lastLevel;
            int revivalTime = Util.GetRevivalLevelTime(levelName);
            if (revivalTime < 5) // 只有前四次需要保存，第五次之后复活消耗的金币都一样
            {
                revivalTime++;
                Util.SetRevivalLevelTime(levelName, revivalTime); // 保存每个关卡的复活次数
            }

            totalCoin = totalCoin - revivalCostCoin;
            Util.SetCoin(totalCoin);
            UpdateUI();
            HidePopup();
            gameManager.SequenceLogic();
            gameManager.SequenceDOTLogic();
            gameManager.BackgroundTogglePause(); // 恢复背景旋转
            guyAnim.DORestart();
            if (Util.SoundIsOn()) // 如果当前声音播放，则恢复播放声音
            {
                soundManager.resumeAllMusic();
            }
        }
    }

    public  void PromptPopup()
    {
        if (Util.SoundIsOn()) // 如果当前声音播放，则暂停
        {
            soundManager.pauseAllMusic();
        }
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        popupCanvasGroup.gameObject.SetActive(true);
        AddButtonListeners();
        ResetProgressBar();
        UpdateUI();
    }
    /// <summary>
    /// Method to hide the popup
    /// </summary>
    private void HidePopup()
    {
        isPauseBarAnimation = true;
        popupCanvasGroup.gameObject.SetActive(false);
        RemoveButtonListener();
        FindObjectOfType<InputTouch>().BLOCK_INPUT = false;       
    }

    void Update()
    {
        if (isPauseBarAnimation) // 如果暂停播放，则返回
        {
            return;
        }
        float dt = Time.deltaTime;
        timer -= dt;
        float timePerc = Mathf.Clamp01(timer / duration);
        barFill.fillAmount = timePerc;
        if (timer < 0)
        {
            isPauseBarAnimation = true;
            GotoGameOver();
        }
   
    }

    private void UpdateUI()
    {      
        String levelName = "LEVEL" + canvasManager.lastLevel;
        int revivalTime = Util.GetRevivalLevelTime(levelName); // 获取当前关卡复活次数
        revivalCostCoin = constant.getLevelRevivalCostCoin(canvasManager.lastLevel, revivalTime);
        txtCostCoin.text = Convert.ToString(revivalCostCoin);
        txtTotalCoin.text = Convert.ToString(Util.GetCoin());
    }


    private void ResetProgressBar()
    {
        duration = 4.75f;
        timer = duration;
        barFill.fillAmount = 1;
        isPauseBarAnimation = false;
    }
}

