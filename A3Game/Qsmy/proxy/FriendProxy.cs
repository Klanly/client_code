using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
using System.Collections;


namespace MuGame
{

    class FriendProxy : BaseProxy<FriendProxy>
    {

        public static uint EVENT_FRIENDLIST = 1701;
        public static uint EVENT_BLACKLIST = 1702;
        public static uint EVENT_ENEMYLIST = 1703;
        public static uint EVENT_NEARBYLIST = 1704;
        public static uint EVENT_RECOMMEND = 1705;//在线推荐
        public static uint EVENT_LOOKFRIEND = 11; //观察对象
        public static uint EVENT_AGREEAPLYRFRIEND = 1706;//
        public static uint EVENT_DELETEFRIEND = 1707;//删除好友
        public static uint EVENT_RECEIVEADDBLACKLIST = 1708;//添加到黑名单
        public static uint EVENT_DELETEENEMY = 1709;//delete enmey
        public static uint EVENT_ENEMYPOSTION = 1710;//敌人位置
        public static uint EVENT_DELETEBLACKLIST = 1711;//删除黑名单
        public static uint EVENT_REMOVENEARYBY = 1712;//移除附近的人
        public static uint EVENT_ADDNEARYBY = 1713;//添加附近的人
        public static uint EVENT_REFRESHNEARYBY = 1714;//刷新附近的人信息
        public static uint EVENT_RECEIVEAPPLYFRIEND = 1715;//接收到好友申请


        public Dictionary<uint, itemFriendData> FriendDataList = new Dictionary<uint, itemFriendData>();//好友列表
        public Dictionary<uint, itemFriendData> requestFirendList = new Dictionary<uint, itemFriendData>();//好友申请列表
        public List<string> requestFriendListNoAgree = new List<string>();//好友申请列表,还没有同意的

        public Dictionary<uint, itemFriendData> BlackDataList = new Dictionary<uint, itemFriendData>();//黑名单列表
        public List<itemFriendData> NearbyDataList = new List<itemFriendData>();//附近的人列表
        public Dictionary<uint, itemFriendData> EnemyDataList = new Dictionary<uint, itemFriendData>();//附近的人列表
        public List<itemFriendData> RecommendDataList = new List<itemFriendData>();//在线推荐的人列表

        public FriendProxy()
        {
            addProxyListener(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, onfriendinfo);//好友信息
        }

        public void onfriendinfo(Variant data)
        {
            debug.Log("好友信息：" + data.dump());

            FriendType ftp = FriendType.NON;
            if (data.ContainsKey("res"))
            {
                int res = data["res"]._int;
                if (res > 0)
                {
                    ftp = (FriendType)res;
                }
                else
                {
                    Globle.err_output(res);
                }

            }
            switch (ftp)
            {
                case FriendType.NON:
                    break;
                case FriendType.FRIEND:
                    setBuddy(data);
                    setBlackList(data);
                    setEnemyList(data);
                    break;
                case FriendType.ADD_FRIEND:
                    flytxt.instance.fly(ContMgr.getCont("FriendProxy_wait"));
                    break;
                case FriendType.BLACKLIST:
                    receiveAddToBlackList(data);
                    break;
                case FriendType.DELETEFRIEND:
                    setDeleteFriend(data);
                    break;
                case FriendType.AGREEAPPLYFRIEND:
                    setAgreeAplyFriend(data);
                    break;
                case FriendType.SHOWTARGETINFO:
                    dispatchEvent(GameEvent.Create(EVENT_LOOKFRIEND, this, data));
                    break;
                case FriendType.RECEIVEAPPLYFRIEND:
                    ReceiveApplyFriend(data);//接收到对方好友申请
                    break;
                case FriendType.ONLINERECOMMEND:
                    RecommendFriend(data);//一键征友列表
                    break;
                case FriendType.REFUSEADDFRIEND:
                    setRefuseAddFriend(data);
                    break;
                case FriendType.BEREFUSE://加好友被拒
                    setBeRefuse(data);
                    break;

                case FriendType.DELETEENEMY://
                    setDeleteEnemy(data);
                    break;
                case FriendType.ENEMYPOSTION:
                    setEnemyPostion(data);
                    break;
                case FriendType.REMOVEBLACKLIST:
                    setRemoveBlackList(data);
                    break;
                case FriendType.RECEIVEENEMY:
                    setReceiveEnemy(data);
                    break;
                default:
                    break;
            }

        }

        private void setBuddy(Variant data)
        {
            if (data.ContainsKey("buddy"))
            {
                List<Variant> array = data["buddy"]._arr;
                FriendDataList.Clear();
                foreach (var item in array)
                {
                    itemFriendData itemFData = new itemFriendData();
                    itemFData.cid = item["cid"]._uint;
                    itemFData.name = item["name"]._str;
                    itemFData.carr = item["carr"]._uint;
                    itemFData.lvl = item["lvl"]._int;
                    itemFData.zhuan = item["zhuan"]._uint;
                    itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
                    itemFData.combpt = item["combpt"]._uint;
                    itemFData.online = item["online"]._bool;
                    itemFData.mlzd_lv = item["mlzd_diff"]._int;
                    if (!itemFData.online) itemFData.map_id = -1;
                    if (item.ContainsKey("map_id")) itemFData.map_id = item["map_id"]._int;
                    if (item.ContainsKey("llid")) itemFData.llid = (uint)item["llid"]._int;//副本ID
                    itemFData.isNew = false;
                    FriendDataList[item["cid"]._uint] = itemFData;
                }

              
            
                List<uint> FriendCid = new List<uint>(FriendDataList.Keys);
              
                for (int i = 0; i < FriendCid.Count; i++)
                {
                    for (int j = 0; j < FriendCid.Count; j++)
                    {
                       // FriendDataList[FriendCid[i]]
                       if (i<j&&FriendDataList[FriendCid[i]].online == false && FriendDataList[FriendCid[j]].online == true)
                        {
                            itemFriendData temp = FriendDataList[FriendCid[i]];
                            FriendDataList[FriendCid[i]] = FriendDataList[FriendCid[j]];
                            FriendDataList[FriendCid[j]] = temp;
                        }
                    }
                       
                }
             
                dispatchEvent(GameEvent.Create(EVENT_FRIENDLIST, this, data));

            }
        }
        private void setBlackList(Variant data)
        {
            if (data.ContainsKey("blacklist"))
            {
                List<Variant> array = data["blacklist"]._arr;
                BlackDataList.Clear();
                foreach (var item in array)
                {
                    itemFriendData itemFData = new itemFriendData();
                    itemFData.cid = item["cid"]._uint;
                    itemFData.name = item["name"]._str;
                    itemFData.carr = item["carr"]._uint;
                    itemFData.lvl = item["lvl"]._int;
                    itemFData.zhuan = item["zhuan"]._uint;
                    if(item.ContainsKey ("mlzd_diff"))
                        itemFData.mlzd_lv = item["mlzd_diff"]._int;
                    itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
                    itemFData.combpt = item["combpt"]._uint;
                    if (item.ContainsKey("online")) { itemFData.online = item["online"]._bool; if (!itemFData.online) itemFData.map_id = -1; }


                    if (item.ContainsKey("map_id")) itemFData.map_id = item["map_id"]._int;
                    BlackDataList[item["cid"]._uint] = itemFData;
                }
                dispatchEvent(GameEvent.Create(EVENT_BLACKLIST, this, data));

            }
        }
        private void receiveAddToBlackList(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = new itemFriendData();
            itemFData.cid = item["cid"]._uint;
            itemFData.name = item["name"]._str;
            itemFData.carr = item["carr"]._uint;
            itemFData.lvl = item["lvl"]._int;
            itemFData.zhuan = item["zhuan"]._uint;
            if (item.ContainsKey("mlzd_diff"))
                itemFData.mlzd_lv = item["mlzd_diff"]._int;
            itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
            itemFData.combpt = item["combpt"]._uint;
            if (item.ContainsKey("online")) { itemFData.online = item["online"]._bool; if (!itemFData.online) itemFData.map_id = -1; }
            if (item.ContainsKey("map_id")) itemFData.map_id = item["map_id"]._int;
            BlackDataList[item["cid"]._uint] = itemFData;
            flytxt.instance.fly(itemFData.name +ContMgr.getCont("FriendProxy_addblicklist"));

            dispatchEvent(GameEvent.Create(EVENT_RECEIVEADDBLACKLIST, this, data));
            dispatchEvent(GameEvent.Create(EVENT_BLACKLIST, this, data));
            if (FriendDataList.ContainsKey(itemFData.cid))
            {
                FriendDataList.Remove(itemFData.cid);
            }

        }
        private void setEnemyList(Variant data)
        {
            uint tm = 0;
            uint cid = 0;
            if (data.ContainsKey("foes_place"))
            {
                tm = data["foes_place"]["limit_tm"]._uint;
                cid = data["foes_place"]["cid"]._uint;
            }

            if (data.ContainsKey("foes"))
            {
                List<Variant> array = data["foes"]._arr;
                EnemyDataList.Clear();
                foreach (var item in array)
                {
                    itemFriendData itemFData = new itemFriendData();
                    itemFData.cid = item["cid"]._uint;
                    itemFData.name = item["name"]._str;
                    itemFData.carr = item["carr"]._uint;
                    itemFData.lvl = item["lvl"]._int;
                    itemFData.zhuan = item["zhuan"]._uint;
                    if (item.ContainsKey("mlzd_diff"))
                        itemFData.mlzd_lv = item["mlzd_diff"]._int;
                    itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
                    itemFData.combpt = item["combpt"]._uint;
                    itemFData.hatred = item["hatred"]._uint;
                    itemFData.kill_tm = item["kill_tm"]._uint;
                    itemFData.online = item["online"]._bool;
                    if (item.ContainsKey("map_id")) { itemFData.map_id = (int)item["map_id"]._uint; } else { itemFData.map_id = 0; };
                    if (item.ContainsKey("llid")) itemFData.llid = item["llid"]._uint;
                    if (FriendDataList.ContainsKey(itemFData.cid))
                    {
                        if (!FriendDataList[itemFData.cid].online)
                        {
                            itemFData.map_id = -1;
                        }
                        else
                        {

                            itemFData.map_id = FriendDataList[itemFData.cid].map_id;
                        }
                    }

                    if (cid == itemFData.cid)
                    {
                        itemFData.limttm = tm;
                    }

                    EnemyDataList[item["cid"]._uint] = itemFData;
                }
                dispatchEvent(GameEvent.Create(EVENT_ENEMYLIST, this, data));

            }
        }
      
        private void setDeleteFriend(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = new itemFriendData();
            itemFData.cid = item["cid"]._uint;
            if (FriendDataList.ContainsKey(itemFData.cid))
            {
                FriendDataList.Remove(itemFData.cid);
            }
            dispatchEvent(GameEvent.Create(EVENT_DELETEFRIEND, this, data));
        }

        private void setAgreeAplyFriend(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = new itemFriendData();
            itemFData.cid = item["cid"]._uint;
            itemFData.name = item["name"]._str;
            itemFData.carr = item["carr"]._uint;
            itemFData.lvl = item["lvl"]._int;
            itemFData.zhuan = item["zhuan"]._uint;
            if (item.ContainsKey("mlzd_diff"))
                itemFData.mlzd_lv = item["mlzd_diff"]._int;
            itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
            itemFData.combpt = item["combpt"]._uint;
            itemFData.online = item["online"]._bool;
            if (!itemFData.online) itemFData.map_id = -1;
            if (item.ContainsKey("map_id")) itemFData.map_id = item["map_id"]._int;
            if (item.ContainsKey("llid")) itemFData.llid = (uint)item["llid"]._int;//副本ID
            itemFData.isNew = false;
            FriendDataList[item["cid"]._uint] = itemFData;
            if (requestFriendListNoAgree.Contains(itemFData.name))
            {
                flytxt.instance.fly(itemFData.name +ContMgr.getCont("FriendProxy_agreeed"));
                requestFriendListNoAgree.Remove(itemFData.name);
            }
            else
            {
                flytxt.instance.fly(itemFData.name + ContMgr.getCont("FriendProxy_friended"));
            }
            if (BlackDataList.ContainsKey(itemFData.cid))
            {
                sendRemoveBlackList(itemFData.cid);
            }
            dispatchEvent(GameEvent.Create(EVENT_AGREEAPLYRFRIEND, this, data));
            friendList._instance?.onShowOnlineFriendsChanged(true);
            //friendList
        }
        private void ReceiveApplyFriend(Variant data)
        {
            if (GlobleSetting.IGNORE_FRIEND_ADD_REMINDER) return;//客户端开启屏蔽加好友提示

            ArrayList requestInfoData = new ArrayList();
            if (data.ContainsKey("cid")) requestInfoData.Add((uint)data["cid"]);
            if (data.ContainsKey("name")) requestInfoData.Add((string)data["name"]);
            if (data.ContainsKey("carr")) requestInfoData.Add((uint)data["carr"]);
            if (data.ContainsKey("clan_name")) requestInfoData.Add((string)data["clan_name"]);
            if (data.ContainsKey("lvl")) requestInfoData.Add((uint)data["lvl"]);
            if (data.ContainsKey("zhuan")) requestInfoData.Add((uint)data["zhuan"]);
            if (data.ContainsKey("combpt")) requestInfoData.Add((uint)data["combpt"]);
            uint cid = data.ContainsKey("cid") ? data["cid"]._uint : 0;

            if (cid != 0)
            {
                if (BlackDataList.ContainsKey(cid)) return;

                Variant item = data;
                itemFriendData itemFData = new itemFriendData();
                itemFData.cid = item["cid"]._uint;
                itemFData.name = item["name"]._str;
                itemFData.carr = item["carr"]._uint;
                itemFData.lvl = item["lvl"]._int;
                itemFData.zhuan = item["zhuan"]._uint;
                if (item.ContainsKey("mlzd_diff"))
                    itemFData.mlzd_lv = item["mlzd_diff"]._int;
                if (item.ContainsKey("clan_name")) itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;

                itemFData.combpt = item["combpt"]._uint;
                if (item.ContainsKey("online")) { itemFData.online = item["online"]._bool; if (!itemFData.online) itemFData.map_id = -1; }
                if (item.ContainsKey("map_id")) itemFData.map_id = item["map_id"]._int;
                if (item.ContainsKey("llid")) itemFData.llid = (uint)item["llid"]._int;//副本ID
                itemFData.isNew = false;

                if (!requestFriendListNoAgree.Contains((string)data["name"]))
                {

                    if (!requestFirendList.ContainsKey(cid)) requestFirendList.Add(itemFData.cid, itemFData);

                    dispatchEvent(GameEvent.Create(EVENT_RECEIVEAPPLYFRIEND, this, null));//因为可能出现多个加好友请求
                }
                else//表示对方同意加好友
                {

                    requestFriendListNoAgree.Remove(itemFData.name);
                    FriendDataList[item["cid"]._uint] = itemFData;
                    dispatchEvent(GameEvent.Create(EVENT_FRIENDLIST, this, data));
                    flytxt.instance.fly(itemFData.name + ContMgr.getCont("FriendProxy_agree"));
                }

            }
        }
        void RecommendFriend(Variant data)
        {
            if (data.ContainsKey("buddys"))
            {
                List<Variant> array = data["buddys"]._arr;
                //debug.Log("22222222222" + array.Count);
                RecommendDataList.Clear();
                foreach (var item in array)
                {

                    //debug.Log("00000000000000" + FriendDataList.Count);
                    itemFriendData itemFData = new itemFriendData();
                    itemFData.cid = item["cid"]._uint;
                    itemFData.name = item["name"]._str;
                    itemFData.carr = item["carr"]._uint;
                    itemFData.lvl = item["lvl"]._int;
                    itemFData.zhuan = item["zhuan"]._uint;
                    if (item.ContainsKey("mlzd_diff"))
                        itemFData.mlzd_lv = item["mlzd_diff"]._int;
                    debug.Log(itemFData.name);

                    RecommendDataList.Add(itemFData);

                    // else debug.Log("nnnnnnnnnnnnnnn");
                }
                // debug.Log(RecommendDataList.Count + "000000000000000");
                //// debug.Log("lllll" + RecommendDataList.Count);
                // for (int i = 0; i < RecommendDataList.Count; i++)
                // {
                //     debug.Log(RecommendDataList.Count + "000000000000000");
                //     if (FriendDataList.ContainsKey(RecommendDataList[i].cid))
                //         RecommendDataList.Remove(RecommendDataList[i]);
                //  }
                // debug.Log(RecommendDataList.Count + "0007777777777777000");
                // debug.Log("222222" + RecommendDataList.Count);
                // debug.Log("1111111111111" + RecommendDataList.Count);
                //foreach (var it in FriendDataList.Keys)
                //{
                //    for (int i = 0; i < RecommendDataList.Count; i++)
                //        if (FriendDataList[it].cid == RecommendDataList[i].cid)
                //        {
                //            RecommendDataList.Remove(RecommendDataList[i]);
                //        }
                //}
                dispatchEvent(GameEvent.Create(EVENT_RECOMMEND, this, data));
            }
        }
        void setRefuseAddFriend(Variant data)
        {

        }
        public void setBeRefuse(Variant data)
        {
            string name = data["name"]._str;

            flytxt.instance.fly(name + ContMgr.getCont("FriendProxy_noagree"));
        }
        public void setDeleteEnemy(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = new itemFriendData();
            itemFData.cid = item["cid"]._uint;
            if (EnemyDataList.ContainsKey(item["cid"]._uint))
            {
                EnemyDataList.Remove(item["cid"]._uint);
            }

            dispatchEvent(GameEvent.Create(EVENT_DELETEENEMY, this, data));
        }
        public void setEnemyPostion(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = null;

            if (EnemyDataList.ContainsKey(item["cid"]._uint))
            {
                itemFData = EnemyDataList[item["cid"]._uint];
                itemFData.cid = item["cid"]._uint;
                itemFData.map_id = (int)item["map_id"]._uint;
                itemFData.llid = item["llid"]._uint;
                EnemyDataList[item["cid"]._uint] = itemFData;
            }
            dispatchEvent(GameEvent.Create(EVENT_ENEMYPOSTION, this, data));
        }
        public void setRemoveBlackList(Variant data)
        {
            //TODO: removeBlackList
            Variant item = data;

            if (BlackDataList.ContainsKey(item["cid"]._uint))
            {
                BlackDataList.Remove(item["cid"]._uint);
            }
            dispatchEvent(GameEvent.Create(EVENT_DELETEBLACKLIST, this, data));
        }
        public void setReceiveEnemy(Variant data)
        {
            Variant item = data;
            itemFriendData itemFData = new itemFriendData();
            itemFData.cid = item["cid"]._uint;
            itemFData.name = item["name"]._str;
            itemFData.carr = item["carr"]._uint;
            itemFData.lvl = item["lvl"]._int;
            itemFData.zhuan = item["zhuan"]._uint;
            if (item.ContainsKey("mlzd_diff"))
                itemFData.mlzd_lv = item["mlzd_diff"]._int;
            itemFData.clan_name = string.IsNullOrEmpty(item["clan_name"]._str) ? ContMgr.getCont("FriendProxy_wu") : item["clan_name"]._str;
            itemFData.combpt = item["combpt"]._uint;
            if (item.ContainsKey("map_id")) { itemFData.map_id = (int)item["map_id"]._uint; } else { itemFData.map_id = -1; };
            if (item.ContainsKey("llid")) itemFData.llid = item["llid"]._uint;
            itemFData.hatred++;
            if (EnemyDataList.ContainsKey(item["cid"]._uint))
            {
                itemFData.hatred += EnemyDataList[item["cid"]._uint].hatred;
            }

            EnemyDataList[item["cid"]._uint] = itemFData;
            dispatchEvent(GameEvent.Create(EVENT_ENEMYLIST, this, data));
        }
        public void sendfriendlist(FriendType ft)//Done
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)ft;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendAddFriend(uint cid = 0, string name = "", bool isNormal = true)//好友申请
        {
            if (FriendDataList.ContainsKey(cid))
            {
                flytxt.instance.fly(name +ContMgr.getCont("FriendProxy_youarefriend"));
            }
            else
            {
                Variant msg = new Variant();
                msg["buddy_cmd"] = (uint)FriendType.ADD_FRIEND;
                if (cid != 0) msg["cid"] = cid;
                if (!string.IsNullOrEmpty(name)) msg["name"] = name;
                if (!requestFriendListNoAgree.Contains(name) && isNormal)
                {
                    requestFriendListNoAgree.Add(name);
                }
                sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
             
            }
        }
        public void joinHeiList(uint cid = 0, string name = "", bool isNormal = true)//加入黑名单
        {
                Variant msg = new Variant();
                msg["buddy_cmd"] = (uint)FriendType.BLACKLIST;
                if (cid != 0) msg["cid"] = cid;
                if (!string.IsNullOrEmpty(name)) msg["name"] = name;
                if (!requestFriendListNoAgree.Contains(name) && isNormal)
                {
                    requestFriendListNoAgree.Add(name);
                }
                sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }

        public void sendAddBlackList(uint cid = 0, string name = "")//加入黑名单
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.BLACKLIST;
            if (cid != 0) msg["cid"] = cid;
            if (!string.IsNullOrEmpty(name)) msg["name"] = name;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendDeleteFriend(uint cid = 0, string name = "")//删除好友
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.DELETEFRIEND;
            if (cid != 0) msg["cid"] = cid;
            if (!string.IsNullOrEmpty(name)) msg["name"] = name;

            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendAgreeApplyFriend(uint cid) //发送同意好友申请消息
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.AGREEAPPLYFRIEND;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendOnlineRecommend() //获取在线推荐
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.ONLINERECOMMEND;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }

        public void sendRemoveBlackList(uint cid) //删除黑名单
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.REMOVEBLACKLIST;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendRefuseAddFriend(uint cid) //拒绝好友
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.REFUSEADDFRIEND;
            msg["cid"] = cid;
            msg["refuse"] = true;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendEnemyPostion(uint cid) //仇人位置
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.ENEMYPOSTION;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }

        public void sendgetplayerinfo(uint cid)
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = 11;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public void sendDeleteEnemy(uint cid) //删除仇人
        {
            Variant msg = new Variant();
            msg["buddy_cmd"] = (uint)FriendType.DELETEENEMY;
            msg["cid"] = cid;
            sendRPC(PKG_NAME.C2S_ON_ADDFRIEND_BUTTON, msg);
        }
        public enum FriendType
        {
            NON,
            FRIEND,//好友
            ADD_FRIEND = 2,//添加好友
            AGREEAPPLYFRIEND = 3,//同意好友申请
            BLACKLIST = 4,//黑名单
            REFUSEADDFRIEND = 5,//忽略、拒绝好友
            DELETEFRIEND = 6,//删除好友
            REMOVEBLACKLIST = 7,//删除黑名单
            ENEMYPOSTION = 9,//仇人位置
            NEARBY = 10,//附近
            SHOWTARGETINFO = 11,//查看对象
            DELETEENEMY = 12,//删除仇人
            ONLINERECOMMEND = 13,//在线推荐

            RECEIVEAPPLYFRIEND = 15,//接收到好友申请
            RECEIVEENEMY = 16,//enmey
            BEREFUSE = 17//被拒绝好友
        }
        public bool checkAddFriend(string name = "") //添加好友检测
        {
            int friendCount = FriendDataList.Count;
            if (friendCount >= PlayerModel.getInstance().up_lvl * 10 + 50)
            {
                flytxt.instance.fly(ContMgr.getCont("FriendProxy_max"));
                return true;
            }
            foreach (KeyValuePair<uint, itemFriendData> item in FriendDataList)
            {
                if (item.Value.name.Equals(name))
                {
                    flytxt.instance.fly(name + ContMgr.getCont("FriendProxy_listhave"));
                    return true;
                }
            }
            return false;
        }
        public bool checkAddheiFriend(string name = "") //添加黑名单检测
        {
            int friendCount = FriendDataList.Count;
            if (friendCount >= PlayerModel.getInstance().up_lvl * 10 + 50)
            {
                flytxt.instance.fly(ContMgr.getCont("FriendProxy_max"));
                return true;
            }
            foreach (KeyValuePair<uint, itemFriendData> item in FriendDataList)
            {
                if (item.Value.name.Equals(name))
                {
                    return true;
                }
            }

            return false;
        }
        public void removeNearyByLeave(uint cid)
        {
            Variant data = new Variant();
            data["cid"] = cid;
            dispatchEvent(GameEvent.Create(EVENT_REMOVENEARYBY, this, data));
        }
        public void addNearyByPeople(uint iid)
        {
            Variant data = new Variant();
            data["iid"] = iid;
            dispatchEvent(GameEvent.Create(EVENT_ADDNEARYBY, this, data));
        }
        public void reFreshProfessionInfo(ArrayList arry)
        {
            Variant data = new Variant();
            data["cid"] = (uint)arry[0];
            if (arry.Count == 2)
            {
                data["combpt"] = arry[1] == null ? 0 : (int)arry[1];
                dispatchEvent(GameEvent.Create(EVENT_REFRESHNEARYBY, this, data));
            }

        }
    }
}
