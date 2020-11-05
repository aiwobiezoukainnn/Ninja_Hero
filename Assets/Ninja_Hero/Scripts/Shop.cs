using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class ShopItem
{
    public static  string GoldCoin60 = "coin60"; // 60金币

    public static  string GoldCoin388 = "coin388"; // 388金币
    public static  string GoldCoin888 = "coin888"; // 888金币
}
public class Shop : MonoBehaviourHelper
{
    public Text[] txtShopItemPrice; // 商品项价格
    public Text[] txtBuyCoin; // 赠送的金币
    public Text txtTotalCoin;
    public Text txtTitle;
    public Text txtBack;
    public Button btnShop;
    public Button btnAdFree; // 去广告
    public CanvasGroup popupCanvasGroup;
    private OnCallBack callBack; // 实现回调，当关闭商店的时候
    protected static Shop instance = null;
    public static Shop Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new Shop();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private void Awake()
   {
       instance = this;
       popupCanvasGroup.gameObject.SetActive(false);
       for (int i = 0; i < txtShopItemPrice.Length; i++)
       {
            txtShopItemPrice[i].text = Convert.ToString(constant.shop_items_price[i]);
       }
        
       for (int i = 0; i < txtBuyCoin.Length; i++)
        {
            txtBuyCoin[i].text = Convert.ToString(constant.shop_buy_coins[i]);
        }
        txtBuyCoin[txtBuyCoin.Length - 1].text = GameDataLoader.Instance.GetLocaleString("remove_ads");
        txtTitle.text = GameDataLoader.Instance.GetLocaleString("shop");
        txtBack.text = GameDataLoader.Instance.GetLocaleString("back");
        float aspectRatio = Screen.width * 1.0f / Screen.height;
        if (aspectRatio == 768f / 1024f ||
            aspectRatio == 1536f / 2048f        
          )
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

    public void OnClickedShopItem0()
    {
        GameChargeHandle.Instance.BuyItem("coin60");
    }

    public void OnClickedShopItem1()
    {
        GameChargeHandle.Instance.BuyItem("coin388");
    }

    public void OnClickedShopItem2()
    {       
        GameChargeHandle.Instance.BuyItem("coin888");
    }

    public void OnClickedAdFree()
    {

    }

    public void OnClickedBack()
    {
        HidePopup();
    }


    
    public  void PromptPopup(OnCallBack callBack)
    {
        this.callBack = callBack;
        FindObjectOfType<InputTouch>().BLOCK_INPUT = true;
        popupCanvasGroup.gameObject.SetActive(true);
        UpdateUI();
    }
    /// <summary>
    /// Method to hide the popup
    /// </summary>
    private void HidePopup()
    {
        popupCanvasGroup.gameObject.SetActive(false);
        FindObjectOfType<InputTouch>().BLOCK_INPUT = false;
        if(callBack != null)
        {
            callBack();
        }

    }

    public void UpdateUI()
    {
        txtTotalCoin.text = Convert.ToString(Util.GetCoin());
        if (true) // 免费广告
        {
            btnAdFree.gameObject.SetActive(false);
        }
        else
        {
            btnAdFree.gameObject.SetActive(true);
        }
    }
}

