using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public enum TipType
{
    Normal, // 正常情况
    ContainedButtonOK, // 包含确定按钮
    ContainedButtonShare,  // 包含分享按钮
}

public class GameTip : MonoBehaviourHelper
{
    public Button btnShare; // 分享
    public Button btnOK; // 确定
    public Button btnCancel; // 取消
    public Text txtDescription; // 提示信息
    public Text txtTitle; 
    private TipType tipType;
    private string description;
    public CanvasGroup popupCanvasGroup;
    private OnCallBack callBack; // 实现回调，当关闭的时候

    private void Awake()
   {
        popupCanvasGroup.gameObject.SetActive(false);
        txtTitle.text = GameDataLoader.Instance.GetLocaleString("tips"); 
    }

    private void AddButtonListeners()
    {
        btnShare.onClick.AddListener(OnClickedShare);
        btnOK.onClick.AddListener(OnClickedOK);
        btnCancel.onClick.AddListener(OnClickedCancel);
    }

    private void RemoveButtonListener()
    {
        btnShare.onClick.RemoveListener(OnClickedShare);
        btnOK.onClick.RemoveListener(OnClickedOK);
        btnCancel.onClick.RemoveListener(OnClickedCancel);
    }


    private void OnClickedShare()
    {

    }

    private void OnClickedOK()
    {
        int totalCoin = Util.GetCoin();
        int saveCostCoin = constant.getLevelSaveCostCoin(canvasManager.lastLevel); // 保存进度所消耗的金币
        if(totalCoin < saveCostCoin) // 不足支付,弹出商店
        {
            HidePopup();
            shop.PromptPopup(delegate ()
            {
                FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
            });
        }
        else
        {
            Util.SetLastLevelPlayed(canvasManager.lastLevel); // 保存当前进度
            totalCoin = totalCoin - saveCostCoin;
            Util.SetCoin(totalCoin);
            HidePopup();
        }
    }

    public void OnClickedCancel()
    {
        HidePopup();
    }


    public  void PromptPopup(TipType tipType, string description, OnCallBack callBack)
    {
        this.tipType = tipType;
        this.description = description;
        this.callBack = callBack;
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        popupCanvasGroup.gameObject.SetActive(true);
        AddButtonListeners();
        UpdateUI();
    }
    /// <summary>
    /// Method to hide the popup
    /// </summary>
    private void HidePopup()
    {
        popupCanvasGroup.gameObject.SetActive(false);
        RemoveButtonListener();
        FindObjectOfType<InputTouch>().BLOCK_INPUT = false;
        if (callBack != null)
        {
            callBack();
        }
    }

    private void UpdateUI()
    {
        if(this.tipType == TipType.Normal)
        {
            btnCancel.gameObject.SetActive(false);
            btnShare.gameObject.SetActive(false);
            btnOK.gameObject.SetActive(false);
            txtDescription.transform.localPosition = new Vector3(txtDescription.transform.localPosition.x, -20, 0);
        }
        else if(this.tipType == TipType.ContainedButtonOK)
        {
            btnCancel.gameObject.SetActive(true);
            btnShare.gameObject.SetActive(false);
            btnOK.gameObject.SetActive(true);
            txtDescription.transform.localPosition = new Vector3(txtDescription.transform.localPosition.x, -65, 0);
        }
        else if(this.tipType == TipType.ContainedButtonShare)
        {
            btnCancel.gameObject.SetActive(true);
            btnShare.gameObject.SetActive(true);
            btnOK.gameObject.SetActive(false);
            txtDescription.transform.localPosition = new Vector3(txtDescription.transform.localPosition.x, -1, 0);
        }

        txtDescription.text = this.description;
    }
}

