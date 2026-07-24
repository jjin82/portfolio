
using Common;
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.http;
using System.Security.Cryptography;
using System.Windows.Forms;
using static Mysqlx.Notice.Warning.Types;


public partial class User : Entity
{
    public void BuyProduct(int shopTid, int count)
    {
        ShopManager.Get.BuyProduct(this, shopTid, count, () =>
        {
            GiveProduct(shopTid, count);
        });
    }

    public void InAppPurchase(string? store, string? receipt, string productId)
    {
        Logger.INFO(this, $"[ InAppPurchase ] receipt( {receipt} )");

        switch (store)
        {
            case Store.Google:
                {
                    ShopManager.Get.GoogleInAppPurchase(this, receipt, (shopTid) =>
                    {
                        // 구매 했음.
                        SetHasMadeIAP();

                        // 상품 지급.
                        GiveProduct(shopTid, 0);
                    });
                }
                break;
            case Store.Apple:
                {
                    ShopManager.Get.AppleInAppPurchase(this, receipt, productId, (shopTid) =>
                    {
                        // 구매 했음.
                        SetHasMadeIAP();

                        // 상품 지급.
                        GiveProduct(shopTid, 0);
                    });
                }
                break;
        }
    }

    public void GiveProduct(int shopTid, int count = 1)
    {
        var shopData = T_ShopData.Get(shopTid);
        if (null == shopData || false == shopData.Sale)
        {
            SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
            return;
        }

        // 광고 상품은 판매 불가.
        if (shopData.ShopType.Equals(SHOP_TYPE.AD))
        {
            SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
            return;
        }

        var rewardData = T_RewardData.Get(shopData.RewardType);
        if (null == rewardData)
        {
            SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
            return;
        }

        if (!GiveReward(shopData.RewardType, count))
        {
            SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
            return;
        }

        ExecuteMission(MISSION_TYPE.SHOP_BUY_ONE_PLUS_ONE, shopTid);
    }
}


