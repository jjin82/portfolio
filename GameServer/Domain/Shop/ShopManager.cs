using Common;
using Microsoft.VisualBasic.ApplicationServices;
using Mysqlx.Crud;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using static CommonStruct.IapReceipt;


public partial class ShopManager : BaseManager<ShopManager>
{
    public ShopManager()
    {
    }

    public override bool Initialize()
    {
        lock (_productInfos)
        {
            _productInfos.Clear();
            foreach (var e in T_ShopData.GetAll())
            {
                if (false == e.Sale)
                    continue;

                _productInfos.TryAdd(e.TID, e.TID);
            }
        }

        return true;
    }

    public void UpdateProductList()
    {
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($" [ Shop Product List ]");
        Logger.INFO_PRINT("");
        Logger.INFO_PRINT($"■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■  ■");
        Logger.INFO_PRINT("");

        Initialize();

        // 클라이언트 전송.
        SendProductList();
    }

    public void BuyProduct(User user, int shopTid, int count, Action action)
    {
        if (null == user)
            return;

        // 데이터 체크
        var shopData = T_ShopData.Get(shopTid);
        if (null == shopData || false == shopData.Sale)
        {
            user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
            return;
        }

        // 재화 사용.
        if(false == user.UseCurrency(shopData.PaymentCurrencyType, (int)(shopData.PaymentCount * count)))
        {
            user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ENOUGH_CURRENCY_USED_PURCHASE);
            return;
        }

        action?.Invoke();
    }

    public void GoogleInAppPurchase(User user, string? receipt, Action<int> onResult)
    {
        if (null == user)
            return;

        string store = Store.Google;

        // 라이브 서비스가 아닐때만.
        if (false == ServerConfig.IsLive())
        {
            // 결제 리메인 처리 동작 지원.
            if (Program.IsRemainIAP())
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] test remain InAppPurchase.... store( {store} ), receipt( {receipt} )");
                user.Send(new C2G.RS_SHOP_IAP_COMPLETED(store, "test remain InAppPurchase..."));
                return;
            }
        }

        if (string.IsNullOrEmpty(receipt))
            return;

        string orderId      = "";
        string productId    = "";

        try
        {
            // 루트 객체를 역직렬화
            var root = JsonConvert.DeserializeObject<Root>(receipt);
            if (null == root)
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] not exist receipt.... store( {store} ), receipt( {receipt} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // Unity Tool에서 결제
            if (root.Store.Equals("fake"))
            {
                Logger.FAIL(user, $"[ InAppPurchase ] Unity does not support payment processing.... store( {store} ), receipt( {receipt} )");
                user.Send(new C2G.RS_SHOP_IAP_COMPLETED(Store.Google, "Unity does not support payment processing..."));
                return;
            }

            // Payload를 역직렬화
            var payload = JsonConvert.DeserializeObject<GooglePayload>(root.Payload);
            if (null == payload)
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] not exist root.Payload.... store( {store} ), receipt( {receipt} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // payload의 json 필드를 역직렬화
            var payloadData = JsonConvert.DeserializeObject<GooglePayloadData>(payload.json);
            if (null == payloadData)
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] not exist payload.json.... store( {store} ), receipt( {receipt} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // 오더 아이디 확보.
            orderId     = payloadData.orderId;
            productId   = payloadData.productId;

            // 결제 시퀀스 확보.
            long iapSeq = System.DateTime.Now.Ticks;

            Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 100 ★ -- store( {store} ), orderId( {orderId} ), productId( {productId} )");

            if (string.IsNullOrEmpty(productId))
            {
                Logger.CRITICAL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] The InAppPurchase is not valid.... store( {store} ), orderId( {orderId} ), productId( {productId} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            T_ShopData? shopData = T_ShopData.GetAll().Find(x => x.GoogleProductID.Equals(productId));
            if (null == shopData || (false == shopData.Sale))
            {
                Logger.CRITICAL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] not exist product.... store( {store} ), orderId( {orderId} ), productId( {productId} ), google( {shopData.GoogleProductID} ), apple( {shopData.AppleProductID} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // 현금만 가능.
            if (!shopData.PaymentCurrencyType.Equals(CURRENCY_TYPE.CASH))
            {
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_ISSUE_CURRENCY_USED_PURCHASE);
                return;
            }

            // 구매가 완료 된 오더.
            if (_completedIAP.ContainsKey(orderId))
            {
                user.Send(new C2G.RS_SHOP_IAP_COMPLETED(Store.Google, productId, orderId, root.TransactionID));
                Logger.FAIL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] order id already completed.... store( {store} ), orderId( {orderId} ), productId( {productId} )");
                return;
            }

            // 진행중인 오더 아이디 인지 확인.
            if (_pendingIAP.TryGetValue(orderId, out var timeOver))
            {
                if (timeOver > DateTime.Now.ToFileTime())
                    return;
            }

            var newTimeOver = DateTime.Now.AddSeconds(30).ToFileTime();
            _pendingIAP.AddOrUpdate(orderId, newTimeOver, (key, oldValue) => oldValue = newTimeOver);


            // 구글 영수증 유효성 확인.
            WebManager.Get.GooglePurchaseVerify(iapSeq, user.nickname, payloadData, receipt, (isPurchase) =>
            {
                Post(() =>
                {
                    try
                    {
                        // [DB] 컨슘 처리 내역 DB 확인.
                        if (DBManager.IsConsumeIAP(orderId))
                        {
                            // 구매 완료 등록.
                            _completedIAP.TryAdd(orderId, productId);
                            user.Send(new C2G.RS_SHOP_IAP_COMPLETED(Store.Google, productId, orderId, root.TransactionID));

                            Logger.FAIL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] order id already completed.... store( {store} ), orderId( {orderId} ), productId( {productId} )");
                            return;
                        }

                        Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 400 ★ -- store( {store} ), orderId( {orderId} ), productId( {productId} ), is purchase( {isPurchase} )");
                            
                        if (!isPurchase)
                        {
                            user.SendResultCode(RESULT_CODE.FAIL_IAP);
                            return;
                        }
                            
                        // [DB] 컨슘 처리 DB 기록.
                        if (!DBManager.ConsumeIAP(user, orderId, productId))
                        {
                            user.SendResultCode(RESULT_CODE.FAIL_IAP);
                            return;
                        }

                        // 구매 완료 등록.
                        _completedIAP.TryAdd(orderId, productId);

                        // 구매 완료 처리.
                        user.Send(new C2G.RS_SHOP_IAP_COMPLETED(Store.Google, productId, orderId, root.TransactionID));

                        Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 500 ★ -- store( {store} ), orderId( {orderId} ), productId( {productId} ), is purchase( {isPurchase} )");

                        // 구매 상품 지급.
                        user.Post(() =>
                        {
                            // 유저로 결제 결과 처리.
                            onResult?.Invoke(shopData.TID);

                            Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 600 ★ -- store( {store} ), orderId( {orderId} ), productId( {productId} ), is purchase( {isPurchase} )");
                        });
                    }
                    finally
                    {
                        _pendingIAP.TryRemove(orderId, out var _);
                    }
                });
            });
        }
        catch (Exception ex)
        {
            _pendingIAP.TryRemove(orderId, out var _);

            Logger.EXCEPTION(user, ex, receipt);
        }
    }

    public void AppleInAppPurchase(User user, string receipt, string productId, Action<int> onResult)
    {
        if (null == user || string.IsNullOrEmpty(receipt) || string.IsNullOrEmpty(productId))
            return;

        string store = Store.Apple;

        // 라이브 서비스가 아닐때만.
        if (false == ServerConfig.IsLive())
        {
            // 결제 리메인 처리 동작 지원.
            if (Program.IsRemainIAP())
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] test remain InAppPurchase.... store( {store} ), receipt( {receipt} )");
                user.Send(new C2G.RS_SHOP_IAP_COMPLETED(store, "test remain InAppPurchase..."));
                return;
            }
        }

        string transactionId = "";

        if (string.IsNullOrEmpty(receipt))
            return;

        try
        {
            // 루트 객체를 역직렬화
            var root = JsonConvert.DeserializeObject<Root>(receipt);
            if (null == root)
            {
                Logger.CRITICAL(user, $"[ InAppPurchase ] not exist receipt.... store( {store} ), receipt( {receipt} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // apple 영수증 파싱.
            var payloadData = AppleReceiptUtil.ParseAppleReceipt(root.Payload);

            // 트랜젝션 아이디 확보.
            transactionId = root.TransactionID;

            // 프로덕트 아이디 확보.
            if (string.IsNullOrEmpty(productId))
            {
                productId = payloadData.ProductId;
            }

            // 결제 시퀀스 확보.
            long iapSeq = System.DateTime.Now.Ticks;

            Logger.INFO($"## {iapSeq}, {user?.nickname ?? "no name"} ##, [ InAppPurchase ] ★ 100 ★ -- store( {store} ), transactionId( {transactionId} ), productId( {productId} )");

            if (string.IsNullOrEmpty(productId))
            {
                Logger.CRITICAL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] The InAppPurchase is not valid.... store( {store} ), transactionId( {transactionId} ), productId( {productId} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            T_ShopData? shopData = T_ShopData.GetAll().Find(x => x.AppleProductID.Equals(productId));
            if (null == shopData || (false == shopData.Sale))
            {
                Logger.CRITICAL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] not exist product.... store( {store} ), transactionId( {transactionId} ), productId( {productId} ), google( {shopData.GoogleProductID} ), apple( {shopData.AppleProductID} )");
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_NOT_ITEM_FOR_SALE);
                return;
            }

            // 현금만 가능.
            if (!shopData.PaymentCurrencyType.Equals(CURRENCY_TYPE.CASH))
            {
                user.SendResultCode(RESULT_CODE.FAIL_SHOP_ISSUE_CURRENCY_USED_PURCHASE);
                return;
            }

            // 구매가 완료 된 오더.
            if (_completedIAP.ContainsKey(transactionId))
            {
                user.Send(new C2G.RS_SHOP_IAP_COMPLETED(store, productId, transactionId, transactionId));
                Logger.FAIL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] order id already completed.... store( {store} ), orderId( {transactionId} ), productId( {productId} )");
                return;
            }

            // 진행중인 오더 아이디 인지 확인.
            if (_pendingIAP.TryGetValue(transactionId, out var timeOver))
            {
                if (timeOver > DateTime.Now.ToFileTime())
                    return;
            }

            var newTimeOver = DateTime.Now.AddSeconds(30).ToFileTime();
            _pendingIAP.AddOrUpdate(transactionId, newTimeOver, (key, oldValue) => oldValue = newTimeOver);

            // 영수증 유효성 확인.
            WebManager.Get.ApplePurchaseVerify(iapSeq, user?.nickname ?? "no name", payloadData, receipt, transactionId, productId, (isPurchase) =>
            {
                Post(() =>
                {
                    try
                    {
                        // [DB] 컨슘 처리 내역 DB 확인.
                        if (DBManager.IsConsumeIAP(transactionId))
                        {
                            // 구매 완료 등록.
                            _completedIAP.TryAdd(transactionId, productId);
                            user.Send(new C2G.RS_SHOP_IAP_COMPLETED(store, productId, transactionId, transactionId));

                            Logger.FAIL($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] order id already completed.... store( {store} ), orderId( {transactionId} ), productId( {productId} )");
                            return;
                        }

                        Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 400 ★ -- store( {store} ), orderId( {transactionId} ), productId( {productId} ), is purchase( {isPurchase} )");

                        if (!isPurchase)
                        {
                            user.SendResultCode(RESULT_CODE.FAIL_IAP);
                            return;
                        }

                        // [DB] 컨슘 처리 DB 기록.
                        if (!DBManager.ConsumeIAP(user, transactionId, productId))
                        {
                            user.SendResultCode(RESULT_CODE.FAIL_IAP);
                            return;
                        }

                        // 구매 완료 등록.
                        _completedIAP.TryAdd(transactionId, productId);

                        // 구매 완료 처리.
                        user.Send(new C2G.RS_SHOP_IAP_COMPLETED(store, productId, transactionId, transactionId));

                        Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 500 ★ -- store( {store} ), orderId( {transactionId} ), productId( {productId} ), is purchase( {isPurchase} )");

                        // 구매 상품 지급.
                        user.Post(() =>
                        {
                            // 유저로 결제 결과 처리.
                            onResult?.Invoke(shopData.TID);

                            Logger.INFO($"## {iapSeq}, {user.nickname} ##, [ InAppPurchase ] ★ 600 ★ -- store( {store} ), orderId( {transactionId} ), productId( {productId} ), is purchase( {isPurchase} )");
                        });
                    }
                    finally
                    {
                        _pendingIAP.TryRemove(transactionId, out var _);
                    }
                });
            });
        }
        catch (Exception ex)
        {
            _pendingIAP.TryRemove(transactionId, out var _);

            Logger.EXCEPTION(user, ex, receipt);
        }
    }

    public void SendProductList(User? user = null)
    {
        var sendPacket = new C2G.RS_SHOP_PRODUCT_LIST_GET();
        foreach(var shopTid in _productInfos.Values)
        {
            sendPacket.In(shopTid);
        }

        if (null == user)
        {
            Network.SendAll(sendPacket); 
        }
        else
        {
            user.Send(sendPacket);
        }
    }


    // ===================================================================================================================
    //
    //
    private ConcurrentDictionary<int, int>       _productInfos   = new ConcurrentDictionary<int, int>();         // 상품 정보.
    private ConcurrentDictionary<string, long>   _pendingIAP     = new ConcurrentDictionary<string, long>();     // 구매 진행 중.
    private ConcurrentDictionary<string, string> _completedIAP   = new ConcurrentDictionary<string, string>();   // 구매 완료.
}


// ===================================================================================================================
//
// 영수증 정보 Json 포맷
//
public partial class ShopManager : BaseManager<ShopManager>
{
    public class Root
    {
        public string Payload       { get; set; }
        public string Store         { get; set; }
        public string TransactionID { get; set; }   
    }

    public class GooglePayload
    {
        public string       json        { get; set; }
        public string       signature   { get; set; }
        public List<string> skuDetails  { get; set; }
    }

    public class GooglePayloadData
    {
        public string   orderId         { get; set; }
        public string   packageName     { get; set; }
        public string   productId       { get; set; }
        public long     purchaseTime    { get; set; }
        public int      purchaseState   { get; set; }
        public string   purchaseToken   { get; set; }
        public int      quantity        { get; set; }
        public bool     acknowledged    { get; set; }
    }

    public class ApplePayloadData
    {
        public string? BundleId { get; set; }
        public string? AppVersion { get; set; }
        public string? ReceiptDate { get; set; }

        public string? ProductId { get; set; }
        public string? TransactionId { get; set; }
        public string? OriginalTransactionId { get; set; }
        public string? PurchaseDate { get; set; }
        public string? OriginalPurchaseDate { get; set; }
        public string? ExpiresDate { get; set; }
        public string? CancellationDate { get; set; }
    }

    // ====== 내부 파싱용 (복잡한 ASN.1 → 수집 후 고르기) ======
    internal class InAppReceipt
    {
        public string? ProductId { get; set; }
        public string? TransactionId { get; set; }
        public string? OriginalTransactionId { get; set; }
        public string? PurchaseDate { get; set; }
        public string? OriginalPurchaseDate { get; set; }
        public string? ExpiresDate { get; set; }
        public string? CancellationDate { get; set; }

        public bool HasAny =>
            !string.IsNullOrEmpty(ProductId) ||
            !string.IsNullOrEmpty(TransactionId) ||
            !string.IsNullOrEmpty(OriginalTransactionId) ||
            !string.IsNullOrEmpty(PurchaseDate) ||
            !string.IsNullOrEmpty(OriginalPurchaseDate) ||
            !string.IsNullOrEmpty(ExpiresDate) ||
            !string.IsNullOrEmpty(CancellationDate);
    }

    public static class AppleReceiptUtil
    {
        /// <summary>
        /// Base64 Payload → ApplePayloadData (대표 인앱 1개만 채움: 최신 purchase_date 우선)
        /// </summary>
        public static ApplePayloadData ParseAppleReceipt(string base64Payload)
        {
            var outDto = new ApplePayloadData();
            if (string.IsNullOrWhiteSpace(base64Payload))
                return outDto;

            // 1) Base64 → CMS
            byte[] raw = Convert.FromBase64String(base64Payload);
            var cms = new CmsSignedData(raw);

            // 2) SignedContent bytes
            byte[] content;
            using (var ms = new MemoryStream())
            {
                cms.SignedContent.Write(ms);
                content = ms.ToArray();
            }

            // 3) 최상위 ASN.1 파싱
            var topInApps = new List<InAppReceipt>();
            using (var topStream = new Asn1InputStream(content))
            {
                var topObj = topStream.ReadObject();
                ParseTopLevel(topObj, outDto, topInApps);
            }

            // 4) 인앱 중 대표 1개 선택 (product_id 있는 것 중 purchase_date 최신 우선)
            var chosen = ChoosePrimaryInApp(topInApps);
            if (chosen != null)
            {
                outDto.ProductId = chosen.ProductId;
                outDto.TransactionId = chosen.TransactionId;
                outDto.OriginalTransactionId = chosen.OriginalTransactionId;
                outDto.PurchaseDate = chosen.PurchaseDate;
                outDto.OriginalPurchaseDate = chosen.OriginalPurchaseDate;
                outDto.ExpiresDate = chosen.ExpiresDate;
                outDto.CancellationDate = chosen.CancellationDate;
            }

            return outDto;
        }

        // ---------- Top-level ----------
        private static void ParseTopLevel(Asn1Encodable obj, ApplePayloadData dto, List<InAppReceipt> inApps)
        {
            if (obj is Asn1Set set)
            {
                foreach (Asn1Encodable e in set) ParseTopLevel(e, dto, inApps);
                return;
            }

            if (obj is Asn1Sequence seq)
            {
                if (seq.Count >= 3 && seq[0] is DerInteger)
                {
                    int type = int.Parse(((DerInteger)seq[0]).Value.ToString());
                    byte[] data = Asn1OctetString.GetInstance(seq[2]).GetOctets();

                    switch (type)
                    {
                        case 2: dto.BundleId = ReadValueLoose(data); break;
                        case 3: dto.AppVersion = ReadValueLoose(data); break;
                        case 12: dto.ReceiptDate = ReadValueLoose(data); break;
                        case 17:
                            var list = ExtractInApps(data);
                            if (list.Count > 0) inApps.AddRange(list);
                            break;
                    }
                }
                else
                {
                    foreach (var e in seq) ParseTopLevel(e, dto, inApps);
                }
            }
            // 그 외 타입은 무시
        }

        // ---------- In-App 수집 ----------
        private static List<InAppReceipt> ExtractInApps(byte[] data)
        {
            var result = new List<InAppReceipt>();
            ExtractInAppsRecursive(data, result);
            return result;
        }

        private static void ExtractInAppsRecursive(byte[] data, List<InAppReceipt> outList)
        {
            using var ais = new Asn1InputStream(data);
            var obj = ais.ReadObject();
            if (obj == null) return;

            if (obj is Asn1OctetString os)
            {
                ExtractInAppsRecursive(os.GetOctets(), outList);
                return;
            }

            if (obj is Asn1Set set)
            {
                var builder = new InAppReceipt();
                foreach (var e in set)
                {
                    if (e is Asn1OctetString eos)
                    {
                        var innerList = new List<InAppReceipt>();
                        ExtractInAppsRecursive(eos.GetOctets(), innerList);
                        MergeIntoBuilder(builder, innerList);
                    }
                    else if (e is Asn1Sequence childSeq)
                    {
                        if (!TryParseTripletsInto(childSeq, builder))
                        {
                            foreach (var item in childSeq)
                            {
                                if (item is Asn1Sequence attr && attr.Count >= 3 && attr[0] is DerInteger)
                                {
                                    int t = int.Parse(((DerInteger)attr[0]).Value.ToString());
                                    byte[] valBytes = Asn1OctetString.GetInstance(attr[2]).GetOctets();
                                    ApplyIapField(builder, t, ReadValueLoose(valBytes));
                                }
                                else
                                {
                                    var innerList = new List<InAppReceipt>();
                                    ExtractInAppsRecursive(item.GetEncoded(), innerList);
                                    MergeIntoBuilder(builder, innerList);
                                }
                            }
                        }
                    }
                    else
                    {
                        var innerList = new List<InAppReceipt>();
                        ExtractInAppsRecursive(e.GetEncoded(), innerList);
                        MergeIntoBuilder(builder, innerList);
                    }
                }
                if (builder.HasAny) outList.Add(builder);
                return;
            }

            if (obj is Asn1Sequence seq)
            {
                var builder = new InAppReceipt();
                bool any = TryParseTripletsInto(seq, builder);
                if (any)
                {
                    if (builder.HasAny) outList.Add(builder);
                    return;
                }

                foreach (var e in seq)
                {
                    var innerList = new List<InAppReceipt>();
                    ExtractInAppsRecursive(e.GetEncoded(), innerList);
                    if (innerList.Count == 0) continue;
                    foreach (var iap in innerList)
                        if (iap.HasAny) outList.Add(iap);
                }
                return;
            }
        }

        private static bool TryParseTripletsInto(Asn1Sequence seq, InAppReceipt builder)
        {
            if (seq.Count < 3) return false;
            int i = 0;
            bool any = false;

            while (i + 2 < seq.Count)
            {
                if (seq[i] is not DerInteger typeInt || seq[i + 1] is not DerInteger)
                    break;

                int t = int.Parse(typeInt.Value.ToString());
                var valEnc = seq[i + 2];

                string text = valEnc switch
                {
                    Asn1OctetString aos => ReadValueLoose(aos.GetOctets()),
                    _ => ReadEncAsString(valEnc)
                };

                ApplyIapField(builder, t, text);
                any = true;
                i += 3;
            }

            return any;
        }

        private static void MergeIntoBuilder(InAppReceipt builder, List<InAppReceipt> list)
        {
            foreach (var x in list)
            {
                if (string.IsNullOrEmpty(builder.ProductId) && !string.IsNullOrEmpty(x.ProductId)) builder.ProductId = x.ProductId;
                if (string.IsNullOrEmpty(builder.TransactionId) && !string.IsNullOrEmpty(x.TransactionId)) builder.TransactionId = x.TransactionId;
                if (string.IsNullOrEmpty(builder.OriginalTransactionId) && !string.IsNullOrEmpty(x.OriginalTransactionId)) builder.OriginalTransactionId = x.OriginalTransactionId;
                if (string.IsNullOrEmpty(builder.PurchaseDate) && !string.IsNullOrEmpty(x.PurchaseDate)) builder.PurchaseDate = x.PurchaseDate;
                if (string.IsNullOrEmpty(builder.OriginalPurchaseDate) && !string.IsNullOrEmpty(x.OriginalPurchaseDate)) builder.OriginalPurchaseDate = x.OriginalPurchaseDate;
                if (string.IsNullOrEmpty(builder.ExpiresDate) && !string.IsNullOrEmpty(x.ExpiresDate)) builder.ExpiresDate = x.ExpiresDate;
                if (string.IsNullOrEmpty(builder.CancellationDate) && !string.IsNullOrEmpty(x.CancellationDate)) builder.CancellationDate = x.CancellationDate;
            }
        }

        private static void ApplyIapField(InAppReceipt iap, int t, string text)
        {
            switch (t)
            {
                case 1702: iap.ProductId = text; break;
                case 1703: iap.TransactionId = text; break;
                case 1705: iap.OriginalTransactionId = text; break;
                case 1704: iap.PurchaseDate = text; break;
                case 1706: iap.OriginalPurchaseDate = text; break;
                case 1708: iap.ExpiresDate = text; break;
                case 1712: iap.CancellationDate = text; break;
                default: break;
            }
        }

        // ---------- 대표 인앱 선택 ----------
        private static InAppReceipt? ChoosePrimaryInApp(List<InAppReceipt> inApps)
        {
            if (inApps == null || inApps.Count == 0) return null;

            // product_id가 있는 항목만 우선
            var withProduct = inApps.Where(x => !string.IsNullOrEmpty(x.ProductId)).ToList();
            if (withProduct.Count == 0) withProduct = inApps; // 그래도 없으면 전체에서 고름

            // purchase_date 최신 우선
            InAppReceipt? pick = null;
            DateTimeOffset best = DateTimeOffset.MinValue;

            foreach (var x in withProduct)
            {
                if (!TryParseRfc3339(x.PurchaseDate, out var t))
                {
                    // 날짜 없으면 가장 먼저 발견된 것 후보
                    if (pick == null) pick = x;
                    continue;
                }

                if (t > best)
                {
                    best = t;
                    pick = x;
                }
            }

            return pick ?? withProduct.FirstOrDefault();
        }

        private static bool TryParseRfc3339(string? s, out DateTimeOffset dto)
        {
            dto = default;
            if (string.IsNullOrEmpty(s)) return false;

            var formats = new[]
            {
            "yyyy-MM-dd'T'HH:mm:ss'Z'",
            "yyyy-MM-dd'T'HH:mm:ss.FFF'Z'",
            "yyyy-MM-dd'T'HH:mm:ssK",
            "yyyy-MM-dd HH:mm:ss 'Etc/GMT'"
        };

            return DateTimeOffset.TryParseExact(
                s, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out dto);
        }

        // ---------- 공용 유틸 ----------
        private static string ReadValueLoose(byte[] inner)
        {
            try
            {
                using var ais = new Asn1InputStream(inner);
                var obj = ais.ReadObject();
                return obj switch
                {
                    DerUtf8String utf8 => utf8.GetString(),
                    DerIA5String ia5 => ia5.GetString(),
                    DerPrintableString ps => ps.GetString(),
                    DerVisibleString vs => vs.GetString(),
                    DerInteger di => di.Value.ToString(),
                    Asn1OctetString aos => TryDecodeStringFallback(aos.GetOctets()),
                    _ => obj?.ToString() ?? ""
                };
            }
            catch
            {
                return TryDecodeStringFallback(inner);
            }
        }

        private static string ReadEncAsString(Asn1Encodable enc) =>
            enc switch
            {
                DerUtf8String utf8 => utf8.GetString(),
                DerIA5String ia5 => ia5.GetString(),
                DerPrintableString ps => ps.GetString(),
                DerVisibleString vs => vs.GetString(),
                DerInteger di => di.Value.ToString(),
                Asn1OctetString aos => TryDecodeStringFallback(aos.GetOctets()),
                _ => enc.ToString()
            };

        private static string TryDecodeStringFallback(byte[] bytes)
        {
            try { return Encoding.UTF8.GetString(bytes).Trim('\0'); }
            catch
            {
                try { return Encoding.ASCII.GetString(bytes).Trim('\0'); }
                catch { return BitConverter.ToString(bytes); }
            }
        }
    }
}
