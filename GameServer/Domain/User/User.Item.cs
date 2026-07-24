

using CommonStruct;
using static Mysqlx.Expect.Open.Types.Condition.Types;

public partial class User : Entity
{
    private void LoadDBItem(List<Item> itemList)
    {
        foreach (var v in itemList)
        {
            _itemList.Add(v.tid, v);
        }
    }

    public Item? FindItem(int tid)
    {
        if (false == _itemList.TryGetValue(tid, out var item))
            return null;
        
        return item;
    }

    public bool UpdateItem(Item item, Action? notify = null)
    {
        if (null == item)
        {
            Logger.CRITICAL(this, $"not exist item...");
            return false;
        }

        // 노티 처리가 필요하다면 우선 처리.
        notify?.Invoke();

        // 클라에 알림.
        Send(new C2G.RS_ITEM_UPDATE(item._tid, item._count));

        // DB 처리.
        DBManager.UpdateItem(this, item);

        return true;
    }

    public bool UpdateItem(int tid, int count)
    {
        if (0 > count)
        {
            Logger.CRITICAL(this, $"failed to update item... item tid( {tid} ), count( {count} )");
            return false;
        }

        var item = FindItem(tid);
        if (null == item)
        {
            item = new Item { _tid = tid, _count = count };
            _itemList.TryAdd(item.tid, item);
        }
        item._count = count;

        return UpdateItem(item);
    }

    public bool AddItem(int tid, int count, Action? notify = null)
    {
        if (0 == count)
            return false;

        var item = FindItem(tid);
        if (null == item)
        {
            item = new Item { _tid = tid, _count = count };
            _itemList.TryAdd(item.tid, item);
        }
        else
        {
            item._count += count;
        }

        return UpdateItem(item);
    }

    public bool UseItem(int tid, int useCount = 1)
    {
        var item = FindItem(tid);
        if (null == item) return false;

        // 개수 확인
        if (item.count < useCount)
            return false;

        bool useItem = true;
        if(null != item.skillData)
        {
            // 쿨타임 확인.
            if (item.cooltime > DateTime.Now.ToFileTime())
                return false;

            var game = GameManager.Get.Find(_gameKey);
            if (null == game) return false;

            // 방에 아이템 사용(스킬)
            useItem = game.UseItem(this, item, (item) => 
            {
                // 스킬 아이템 사용.
            });
        }

        if (useItem)
        {
            // 사용 처리.
            item.Use();

            Send(new C2G.RS_ITEM_USE
            {
                itemInfo = new ItemInfo
                {
                    key = item.key,
                    tid = item.tid,
                    count = item.count,
                    remainCooltime = item.remainCooltime
                }
            });

            // DB 처리.
            DBManager.UpdateItem(this, item);

            // [REPORT] 아이템 사용
            WebManager.Get.ReportUseItem(pid, uid, nickname, item.tid);
        }

        return true;
    }

    public void ResetCooltimeItem()
    {
        foreach (var v in _itemList)
        {
            v.Value.ResetCooltime();
        }
    }

    public bool IsEnoughItem(int itemTid, int count = 1)
    {
        var item = FindItem(itemTid);
        if (null == item) return false;

        if (count > item.count)
            return false;

        return true;
    }

    public void SendItemList()
    {
        if (0 >= _itemList.Count)
            return;

        Send(new C2G.RS_ITEM_LIST(_itemList));
    }

    Dictionary<int, Item> _itemList = new Dictionary<int, Item>();
}
