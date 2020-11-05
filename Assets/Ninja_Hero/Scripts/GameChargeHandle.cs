using UnityEngine;

using System.Collections;

using System.Runtime.InteropServices;

public class GameChargeHandle : MonoBehaviour
{
    protected static GameChargeHandle instance = null;
    private string curItemId = ""; // 当前的商品id 

    [DllImport("__Internal")]
    private static extern void InitIAPManager();//初始化

    [DllImport("__Internal")]
    private static extern bool IsProductAvailable();//判断是否可以购买    

    [DllImport("__Internal")]
    private static extern void RealBuyProduct(string itemId);// 真正发起购买商品

    public static GameChargeHandle Instance
    {
        get
        {
            if (null == instance)
            {
                instance = new GameChargeHandle();
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }


    public  void BuyItem(string itemId)
    {
#if UNITY_IOS
        if (IsProductAvailable())
        {
            curItemId = itemId;
            RealBuyProduct(curItemId);
        }
        else
        {
            Debug.Log("Product is not available");
        }
#endif
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
#if UNITY_IOS
        InitIAPManager();
#endif
    }

    public void onSuccess(string itemInfo)
    {
        int goldCoin = 60; // 赠送金币
        if(itemInfo == ShopItem.GoldCoin60)
        {
            goldCoin = 60;
        }
        else if(itemInfo == ShopItem.GoldCoin388)
        {
             goldCoin = 388;
        }
        else if(itemInfo == ShopItem.GoldCoin888)
        {
            goldCoin = 888;
        }
        int totalCoin = Util.GetCoin() + goldCoin;
        Util.SetCoin(totalCoin);
        Shop.Instance.UpdateUI();
    }

    public void onFailed(string itemInfo)
    {
       
    }

    public void onCancel(string itemInfo)
    {       
    }
}

