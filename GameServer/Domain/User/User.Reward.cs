

using C2G;
using Common;
using Microsoft.VisualBasic.ApplicationServices;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.Cms;
using System.Collections.Generic;
using System.Runtime.Serialization;


public partial class User : Entity
{
    public bool GiveReward(REWARD_TYPE rewardType, int rewardCount = 0, Action? notify = null)
    {
        var rewardData = T_RewardData.Get(rewardType);
        if (null == rewardData) return false;

        for(int i = 0; i < rewardData.CurrencyType.Count; ++i)
        {
            var type  = rewardData.CurrencyType[i];
            int count = 0;

            if (0 < rewardCount)
            {
                count = rewardCount;
            }
            else
            {
                if (rewardData.CurrencyType.Count == rewardData.CurrencyCount.Count)
                {
                    count = rewardData.CurrencyCount[i];
                }
                else if (1 == rewardData.CurrencyCount.Count)
                {
                    count = rewardData.CurrencyCount[0];
                }
                else if (2 == rewardData.CurrencyCount.Count)
                {
                    count = UTIL.GetRandomRange(rewardData.CurrencyCount[0], rewardData.CurrencyCount[1]);
                }
                else
                {
                    continue;
                }
            }

            switch (type)
            {
                case CURRENCY_TYPE.DIA:         
                case CURRENCY_TYPE.MOBILE_DATA:
                    {
                        AddCurrency(type, count, notify);
                    }
                    break;
            }
        }

        for (int i = 0; i < rewardData.ItemTid.Count; ++i)
        {
            var itemTid = rewardData.ItemTid[i];
            int count   = 0;

            if (0 < rewardCount)
            {
                count = rewardCount;
            }
            else
            {
                if (rewardData.ItemTid.Count == rewardData.ItemCount.Count)
                {
                    count = rewardData.ItemCount[i];
                }
                else if (1 == rewardData.ItemCount.Count)
                {
                    count = rewardData.ItemCount[0];
                }
                else if (2 == rewardData.ItemCount.Count)
                {
                    count = UTIL.GetRandomRange(rewardData.ItemCount[0], rewardData.ItemCount[1]);
                }
                else
                {
                    continue;
                }
            }

            AddItem(itemTid, count);
        }

        return true;
    }

    public bool GiveReward(REWARD_TYPE rewardType)
    {
        return GiveReward(rewardType, 0, null);
    }
    
    public bool GiveReward(REWARD_TYPE rewardType, Action? notify)
    {
        return GiveReward(rewardType, 0, notify);
    }
}