
using Common;
using Microsoft.VisualBasic.ApplicationServices;
using NerdFox.GF.vo;
using NerdFox.http;
using NerdFox.http.define;
using static ShopManager;

public partial class WebManager : BaseManager<WebManager>
{
    public bool GooglePurchaseVerify(long iapSeq, string nickname, GooglePayloadData payloadData, string receipt, Action<bool> onResult)
    {
        PostEx(() => 
        {
            string store            = "google";
            bool isPurchase         = false;
            string purchase_token   = "";
            string productId        = "";
            string purchase_state   = "";
            string purchaseState    = "";

            try
            {
                Logger.INFO($"## {iapSeq}, {nickname} ##, [ InAppPurchase ] ★ 200 ★ -- store( {store} ), orderId( {payloadData.orderId} ), productId( {payloadData.productId} )");

                NerdFoxConnector connector = NerdFoxConnector.Create();
                if (null != connector)
                {
                    connector.CtxPath = ServerConfig.GetNerdFoxHost();
                }
                connector.SetGameType(NERDFOX_GAME_TYPE.GF);

                NerdFoxPacket? recvPacket = connector.GooglePurchaseVerify(receipt) ?? null;
                if (recvPacket?.IsError() ?? true)
                {
                    Logger.FAIL($"## {iapSeq}, {nickname} ##, failed to purchase verify... errorCode( {recvPacket?.GetErrorCode()} ), errorMessage( {recvPacket?.GetErrorMessage()} )");
                    return;
                }

                Logger.INFO($"## {iapSeq}, {nickname} ##, [ InAppPurchase ] ★ 300 ★ -- store( {store} ), orderId( {payloadData.orderId} ), productId( {payloadData.productId} )");

                /*
                  purchase_token, productId, game_type, purchase_state, purchaseState
                */
                purchase_token  = recvPacket.GetResponseString("purchase_token");
                productId       = recvPacket.GetResponseString("productId");
                purchase_state  = recvPacket.GetResponseString("purchase_state");
                purchaseState   = recvPacket.GetResponseString("purchaseState");


                // 결제 완료 확인.
                isPurchase = (purchase_state.Equals("COMPLETED") && purchaseState.Equals("0"));
                if(!isPurchase)
                {
                    var message = recvPacket.GetResponseString("message");
                    if (!string.IsNullOrEmpty(message))
                    {
                        Logger.CRITICAL_PRINT($"## {iapSeq}, {nickname} ##, failed to InAppPurchase... store( {store} ), orderId( {payloadData.orderId} ), messge( {message} )");
                    }
                }
            }
            finally
            {
                if (!isPurchase)
                {
                    Logger.CRITICAL_PRINT($"## {iapSeq}, {nickname} ##, failed to InAppPurchase... store( {store} ), orderId( {payloadData.orderId} ), purchase_token( {purchase_token} ), productId( {productId} ), purchase_state( {purchase_state} ), purchaseState( {purchaseState} )");
                }

                onResult?.Invoke(isPurchase);
            }
        });

        return true;
    }

    public bool ApplePurchaseVerify(long iapSeq, string nickname, ApplePayloadData payloadData, string receipt, string transactionId, string productId, Action<bool> onResult)
    {
        PostEx(() =>
        {
            string  store           = "apple";
            bool    isPurchase      = false;
            string  purchase_token  = "";
            string  purchase_state  = "";
            string  purchaseState   = "";

            try
            {
                Logger.INFO($"## {iapSeq}, {nickname} ##, [ InAppPurchase ] ★ 200 ★ -- store( {store} ), transactionId( {transactionId} ), productId( {productId} )");

                NerdFoxConnector connector = NerdFoxConnector.Create();
                if (null != connector)
                {
                    connector.CtxPath = ServerConfig.GetNerdFoxHost();
                }
                connector.SetGameType(NERDFOX_GAME_TYPE.GF);

                NerdFoxPacket? recvPacket = connector.ApplePurchaseVerify(store, receipt, transactionId, productId) ?? null;
                if (recvPacket?.IsError() ?? true)
                {
                    Logger.FAIL($"## {iapSeq}, {nickname} ##, failed to purchase verify... errorCode( {recvPacket?.GetErrorCode()} ), errorMessage( {recvPacket?.GetErrorMessage()} )");
                    return;
                }

                // purchase_state, purchaseState
                purchase_state  = recvPacket.GetResponseString("purchase_state");
                purchaseState   = recvPacket.GetResponseString("purchaseState");

                Logger.INFO($"## {iapSeq}, {nickname} ##, [ InAppPurchase ] ★ 300 ★ -- store( {store} ), transactionId( {transactionId} ), productId( {productId} ), purchase_state( {purchase_state} ), purchaseState( {purchaseState} )");


                // 결제 완료 확인.
                isPurchase = (purchase_state.Equals("COMPLETED") && purchaseState.Equals("0"));
                if (!isPurchase)
                {
                    var message = recvPacket.GetResponseString("message");
                    if (!string.IsNullOrEmpty(message))
                    {
                        Logger.CRITICAL_PRINT($"## {iapSeq}, {nickname} ##, failed to InAppPurchase... store( {store} ), transactionId( {transactionId} ), messge( {message} )");
                    }
                }
            }
            finally
            {
                if (!isPurchase)
                {
                    Logger.CRITICAL_PRINT($"## {iapSeq}, {nickname} ##, failed to InAppPurchase... store( {store} ), transactionId( {transactionId} ), purchase_token( {purchase_token} ), productId( {productId} ), purchase_state( {purchase_state} ), purchaseState( {purchaseState} )");
                }

                onResult?.Invoke(isPurchase);
            }
        });

        return true;
    }
}
