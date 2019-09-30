using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MuGame.Qsmy.model;

namespace MuGame
{
    /// <summary>
    /// 代理喝药,在非挂机状态下也会喝
	/// 买药，在非挂机状态只要勾选也会买
    /// Care!!! 1s 一次
    /// </summary>
    class StateProxy : StateBase
    {
        public static StateProxy Instance = new StateProxy();
        private float timer = 0.0f;
        private float shop_timer = 1000.0f;
        private bool hasInit = false;
        private float buymptimer = 0.0f;
        private float buyhptimer = 0.0f;
        int cdtime = 20;
        int cdtimeM = 20;

        private List<int> hpOrder = new List<int>();
        private List<int> mpOrder = new List<int>();
        private List<int> mhpOrder = new List<int>();

        public override void Enter()
        {//DO NOTHING
        }

        public override void Execute(float delta_time)
        {
            buyhptimer += delta_time;
            buymptimer += delta_time;
            timer += delta_time;
            if (timer < 1.0f)
                return;
            timer -= 1.0f;
            shop_timer += 1.0f;

            if (SelfRole._inst.isDead)
                return;

            if (!hasInit)
            {
                Init();
            }

            TryNormalHp();
            TryNormalMp();

            if (SelfRole.fsm.Autofighting)
            {
                TryShopHp();
            }
        }

        public override void Exit()
        {
        }

        private void Init()
        {
            AutoPlayModel apModel = AutoPlayModel.getInstance();
            SXML xml = apModel.AutoplayXml;

            string str = xml.GetNode("recover_hp").getString("order");
            string[] s_str = str.Split(',');
            for (int i = 0; i < s_str.Length; i++)
            {
                hpOrder.Add(int.Parse(s_str[i]));
            }

            str = xml.GetNode("recover_mp").getString("order");
            s_str = str.Split(',');
            for (int i = 0; i < s_str.Length; i++)
            {
                mpOrder.Add(int.Parse(s_str[i]));
            }

            /*str = xml.GetNode("recover_mhp").getString("order");
            s_str = str.Split(',');
            for (int i = 0; i < s_str.Length; i++)
            {
                mhpOrder.Add(int.Parse(s_str[i]));
            }*/ //Modified

            hasInit = true;
        }

        private void TryNormalHp()
        {
          
            cdtime++;
            if (PlayerModel.getInstance().hp >=
                AutoPlayModel.getInstance().NHpLower * PlayerModel.getInstance().max_hp / 100)
                return;
            if (PlayerModel.getInstance().inSpost) { return; }
            int id = GetNormalHpID();
            if (id == -1)
            {//!--没有药,触发购买
             // 改为不再挂机状态也可自动购买
             //if (!SelfRole.fsm.Autofighting)
             //   return;

                if (AutoPlayModel.getInstance().BuyDrug == 0)
                    return;

                SXML xml = AutoPlayModel.getInstance().AutoplayXml;
                List<SXML> supply_hp = xml.GetNodeList("supply_hp", "playlimit==" + PlayerModel.getInstance().up_lvl);
                uint hp_id = 0;
                uint max_num = 0;
                foreach (SXML x in supply_hp)
                {
                    int playerlevel = x.getInt("playerlevel");
                    hp_id = x.getUint("hp_id");
                    max_num = x.getUint("max_num");
                    if (PlayerModel.getInstance().lvl < playerlevel)
                        break;
                }
                if (hp_id != 0)
                {
                    //a3_ItemData itmdata = a3_BagModel.getInstance().getItemDataById(hp_id);
                    shopDatas itmdata = Shop_a3Model.getInstance().GetShopDataById((int)hp_id);
                    if (itmdata == null || itmdata.value <= 0) return;
                    if (PlayerModel.getInstance().money < itmdata.value)
                    {
                        if (remindNotEnoughMoney)
                        {
                            Globle.err_output(-4000);
                            remindNotEnoughMoney = false;
                        }
                        return;
                    }
                    else remindNotEnoughMoney = true;
                    if (!a3_BagModel.getInstance().getHaveRoom())
                        return;

                    uint num;
                    if (max_num * itmdata.value <= PlayerModel.getInstance().money)
                    {
                        num = max_num;
                    }
                    else
                    {
                        num = (uint)(PlayerModel.getInstance().money / itmdata.value);
                    }
                    if (buyhptimer > 2f)
                    {
                        Shop_a3Proxy.getInstance().BuyStoreItems(hp_id, num);
                        buyhptimer = 0f;
                    }
                }
            }
            else
            {//!--有药==在cd时候不吃药
                SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                SXML s_xml = itemsXMl.GetNode("item", "id==" + (uint)id);
                a3_ItemData item = new a3_ItemData();
                item.tpid = (uint)id;
                item.cd_time = s_xml.getFloat("cd");
                if (cdtime==0)
                {
                    a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
                }
                if (cdtime > item.cd_time)
                {
                    a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
                    cdtime = 0;
                    return;
                }
            }
        }

        /// <summary>
        /// 获取背包里面有的，并按照配置顺序的HP药的ID,没有返回-1
        /// </summary>
        /// <returns></returns>
        private int GetNormalHpID()
        {
            var etor = hpOrder.GetEnumerator();
            while (etor.MoveNext())
            {
                bool hasItem = a3_BagModel.getInstance().hasItem((uint)etor.Current);
                a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)etor.Current);
                bool canUse = item.use_limit < PlayerModel.getInstance().up_lvl 
                    || item.use_limit == PlayerModel.getInstance().up_lvl && item.use_lv <= PlayerModel.getInstance().lvl;
                if (hasItem && canUse)
                {
                    return etor.Current;
                }
            }
            return -1;
        }
        public bool remindNotEnoughMoney = true;
        private void TryNormalMp()
        {

       
            cdtimeM++;
            if (PlayerModel.getInstance().mp >=
                AutoPlayModel.getInstance().NMpLower * PlayerModel.getInstance().max_mp / 100)
                return;
            if (PlayerModel.getInstance().inSpost )
            {
                return;
            }
            int id = GetNormalMpID();
            if (id == -1)
            {//!--没有药,触发购买
             //改为不再挂机状态也可自动购买
             //if (!SelfRole.fsm.Autofighting)
             //   return;

                if (AutoPlayModel.getInstance().BuyDrug == 0)
                    return;

                SXML xml = AutoPlayModel.getInstance().AutoplayXml;
                List<SXML> supply_mp = xml.GetNodeList("supply_mp", "playlimit==" + PlayerModel.getInstance().up_lvl);
                uint mp_id = 0;
                uint max_num = 0;
                foreach (SXML x in supply_mp)
                {
                    int playerlevel = x.getInt("playerlevel");
                    mp_id = x.getUint("mp_id");
                    max_num = x.getUint("max_num");
                    if (PlayerModel.getInstance().lvl < playerlevel)
                        break;
                }

                if (mp_id != 0)
                {
                    //a3_ItemData itmdata = a3_BagModel.getInstance().getItemDataById(mp_id);
                    shopDatas itmdata = Shop_a3Model.getInstance().GetShopDataById((int)mp_id);
                    if (itmdata == null || itmdata.value <= 0) return;
                    if (PlayerModel.getInstance().money < itmdata.value)
                    {
                        if (remindNotEnoughMoney)
                        {
                            Globle.err_output(-4000);
                            remindNotEnoughMoney = false;
                        }
                        return;
                    }
                    else remindNotEnoughMoney = true;
                    if (!a3_BagModel.getInstance().getHaveRoom())
                        return;

                    uint num;
                    if (max_num * itmdata.value <= PlayerModel.getInstance().money)
                    {
                        num = max_num;
                    }
                    else
                    {
                        num = (uint)(PlayerModel.getInstance().money / itmdata.value);                        
                    }
                    if (buymptimer > 2f)
                    {
                        Shop_a3Proxy.getInstance().BuyStoreItems(mp_id, num);
                        buymptimer = 0f;
                    }
                }
            }
            else
            {//!--有药
                //a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
                SXML itemsXMl = XMLMgr.instance.GetSXML("item");
                SXML s_xml = itemsXMl.GetNode("item", "id==" + (uint)id);
                a3_ItemData item = new a3_ItemData();
                item.tpid = (uint)id;
                item.cd_time = s_xml.getFloat("cd");
                if (cdtimeM == 0)
                {
                    a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
                }
                if (cdtimeM > item.cd_time)
                {
                    a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
                    cdtimeM = 0;
                    return;
                }              
            }
        }
        /// <summary>
        /// 获取背包里面有的，并按照配置顺序的MP药的ID,没有返回-1
        /// </summary>
        /// <returns></returns>
        private int GetNormalMpID()
        {
            var etor = mpOrder.GetEnumerator();
            while (etor.MoveNext())
            {
                bool hasItem = a3_BagModel.getInstance().hasItem((uint)etor.Current);
                a3_ItemData item = a3_BagModel.getInstance().getItemDataById((uint)etor.Current);
                bool canUse = item.use_limit < PlayerModel.getInstance().up_lvl
                    || item.use_limit == PlayerModel.getInstance().up_lvl && item.use_lv <= PlayerModel.getInstance().lvl;
                if (hasItem && canUse)
                {
                    return etor.Current;
                }
            }
            return -1;
        }

        private void TryShopHp()
        {
            /*if (PlayerModel.getInstance().hp >=
                AutoPlayModel.getInstance().MHpLower * PlayerModel.getInstance().max_hp / 100)
                return;*/ //Modified

            if (shop_timer < 10.0f)
            {
                return;
            }

            int id = GetShopHpID();
            if (id == -1)
                return;
            a3_BagModel.getInstance().useItemByTpid((uint)id, 1);
            shop_timer = 0.0f;
        }

        private int GetShopHpID()
        {
            var etor = mhpOrder.GetEnumerator();
            while (etor.MoveNext())
            {
                bool hasItem = a3_BagModel.getInstance().hasItem((uint)etor.Current);
                if (hasItem)
                {
                    return etor.Current;
                }
            }
            return -1;
        }
    }
}
