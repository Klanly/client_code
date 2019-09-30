using System.Text;
using GameFramework;
using Cross;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace MuGame
{
    class ChatProxy : BaseProxy<ChatProxy>
    {
        public const uint S2C_SYS_NOTICE = 159;

        public static uint lis_sys_notice = 159;

        public ChatProxy()
        {
            addProxyListener(PKG_NAME.S2C_CHAT_MSG_RES, onTalking);
            addProxyListener(PKG_NAME.S2C_CHAT_MSG, getPublish);
            addProxyListener(S2C_SYS_NOTICE, getNotice);
        }

        void onTalking(Variant data)
        {
            debug.Log("发送服务器成功" + data.dump());
            UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.GO_PUBLISH, this, data));
        }

        void getPublish(Variant data)
        {
            if (data.ContainsKey("tp") && data["tp"] != 10)
                UnityEngine.Debug.Log("聊天所有信息" + data.dump());
            if ( data.ContainsKey("cid"))
            {
                  uint cid = data["cid"]._uint;
                  if (FriendProxy.getInstance().BlackDataList.ContainsKey(cid))return;
            }
            if (data.ContainsKey("res"))
            {
                int res = data["res"];
                if (res < 0)//-158
                {
                    Globle.err_output(res);
                    return;
                }
                if (res == 1)//私聊玩家存在情况下
                {
                    a3_chatroom._instance.meSays(false);
                }
            }
            else
            {
                switch ((ChatToType)data["tp"]._int)
                {
                    case ChatToType.Nearby:
                        if(!data.ContainsKey("url"))
                        {
                            uint cid = data["cid"]._uint;
                            string msg = data["msg"]._str;
                            foreach (KeyValuePair<uint, ProfessionRole> item in OtherPlayerMgr._inst.m_mapOtherPlayerSee)
                            {
                                if (item.Value.m_unCID == cid)
                                {
                                    PlayerChatUIMgr.getInstance().show(item.Value, analysisStrName(msg));
                                }
                            }

                        }
                       
                        break;
                    case ChatToType.PrivateSecretlanguage:
                        if (GlobleSetting.IGNORE_PRIVATE_INFO) return;//客户端开启屏蔽私聊信息
                        break;
                }
                // UIClient.instance.dispatchEvent(GameEvent.Create(UI_EVENT.GET_PUBLISH, this, data));

                if (data["tp"] == 10)
                {//系统消息特殊处理，走马灯的同时，聊天框也要显示
                    if (broadcasting.instance != null)
                        broadcasting.instance.addGonggaoMsg(data["msg"]);

                    data["tp"] = (int)ChatToType.SystemMsg;
                    if (a3_chatroom._instance != null)
                        a3_chatroom._instance.otherSays(data);
                }
                else if (data["tp"] == 11)
                {
                    data["tp"] = (int)ChatToType.LegionSystemMsg;
                    if (data.ContainsKey("guard_time"))
                    {
                        if (A3_LegionModel.getInstance().myLegion.lvl > 1)
                            data["msg"] = ContMgr.getCont(("clan_log_12"), new List<string>() { (3 - data["guard_time"]).ToString(), (A3_LegionModel.getInstance().myLegion.lvl - 1).ToString() });
                        else
                            data["msg"] = ContMgr.getCont(("clan_log_11"), new List<string>() { (3 - data["guard_time"]).ToString() });

                        if (a3_chatroom._instance != null)
                            a3_chatroom._instance.otherSays(data);
                    }
                }
                else if (data["tp"] == 12) {
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_CITYWARTIP);
                }
                else
                {
                    if (a3_chatroom._instance != null)
                    {
                        List<Variant> lp = null;
                        if (data.ContainsKey("xtp"))
                        {
                            int xtp = data["xtp"]._int;
                            switch (xtp)
                            {
                                case 1:
                                    data = AnalysisData(data, out lp, xtp);
                                    if (lp.Count == 6 &&
                                        lp[5].ContainsKey("tid") &&
                                        TeamProxy.getInstance()?.MyTeamData != null &&
                                        lp[5]["tid"] == TeamProxy.getInstance().MyTeamData.teamId &&
                                        PlayerModel.getInstance().IsCaptain) //反馈仅对队长可见
                                        flytxt.instance.fly(ContMgr.getCont("a3_currentTeamPanel_in_fb2"));
                                    break;
                                default: break;
                            }
                        }
                        a3_chatroom._instance.otherSays(data, lp);
                    }
                }
            }
        }
        Variant AnalysisData(Variant data,out List<Variant> lp,int xtp)
        {
            lp = new List<Variant>();
            if (data.ContainsKey("msg"))
            {
                switch (xtp)
                {
                    case 1:
                        string[] arrMsg = data["msg"]._str.Split(':');
                        string teamTarget = ContMgr.getCont("a3_teamtarget_" + int.Parse(arrMsg[1]));
                        if (int.Parse(arrMsg[2]) != 0)
                        {
                            string diffName = ContMgr.getCont("a3_a3_counterpartdiffname_" + int.Parse(arrMsg[1]));
                            teamTarget = teamTarget + "(" + diffName + ")";
                        }
                        string msg = ContMgr.getCont("fb_msg_start1") + data["name"] + ContMgr.getCont("fb_msg_start2") + teamTarget + ContMgr.getCont("fb_msg_start3") + ContMgr.getCont("fb_msg_start4");
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = ContMgr.getCont("fb_msg_start1") });
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = data["name"], ["r"] = 0xFF, ["g"] = 0x00, ["b"] = 0x00 });
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = ContMgr.getCont("fb_msg_start2") });
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = teamTarget, ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B });
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = ContMgr.getCont("fb_msg_start3") });
                        lp.Add(new Variant { ["tp"] = data["tp"], ["msg"] = ContMgr.getCont("fb_msg_start4"), ["r"] = 0xE3, ["g"] = 0xB3, ["b"] = 0x6B , ["tid"] = int.Parse(arrMsg[0]) });
                        data["msg"] = msg;
                        data["custom"] = true;
                        break;
                }
            }
            return data;
        }
        void getNotice(Variant data)
        {
            debug.Log("走马灯159" + data.dump());
            dispatchEvent(GameEvent.Create(lis_sys_notice, this, data));
        }

        public void sendMsg(string words, string name, uint type,bool isvoice,uint xtp = 0 )
        {
            Variant msg = new Variant();
            msg["tp"] = type;
            msg["name"] = name;
          

            if(isvoice)
            {
                msg["msg"] = "";
                msg["url"] = words;
            }
            else
                msg["msg"] = words;

            if (xtp != 0)
                msg["xtp"] = xtp;
            else 
                analysisStr(words, msg);
            sendRPC(PKG_NAME.S2C_CHAT_MSG, msg);


            //屏蔽装备和坐标
            //if (msg.ContainsKey("coordinate") || msg.ContainsKey("itm_ids"))
            //    return;

            //添加聊天上报
            string path = "sid=" + Globle.curServerD.sid + "&uid=" + PlayerModel.getInstance().uid + "&cid=" + PlayerModel.getInstance().cid + "&tp=" + type
                + "&cname=" + name + "&vip=" + PlayerModel.getInstance().vip + "&content=" + words;
            //debug.Log("聊天上报::::::::::::::::::" + Globle.curServerD.do_url + "?msg=1" + path);
            HttpAppMgr.POSTSvrstr(Globle.curServerD.do_url + "?msg=1", path, onHttpChatMsg);
        }

        public void onHttpChatMsg(string str)
        {
            debug.Log("聊天上报返回:" + str);
        }

        StringBuilder analysisStr(string str, Variant msg)
        {
            //  
            StringBuilder strBuilder = new StringBuilder();
            string newStr = str.Replace("]", "[");
            string[] strMsgArr = newStr.Split('[');//Regex.Split(newStr, @"\[.*?\]");
            for (int i = 0; i < strMsgArr.Length; i++)
            {
                if (string.IsNullOrEmpty(strMsgArr[i])) continue;
                strBuilder.Append(strMsgArr[i]);
                string posOrItemStr = strMsgArr[i].Trim();
                Match match = Regex.Match(posOrItemStr, @"\(.*?,.*?\)");//用\(\d{1,3},\d{1,3})

                if (match.Success)//大体可以确认是个坐标位置
                {
                    msg["coordinate"] = true;
                }
                else
                {
                    if (posOrItemStr.Contains("#"))
                    {
                        uint id;
                        uint.TryParse(posOrItemStr.Split('#')[1], out id);
                        if (id != 0)
                        {
                            if (msg["itm_ids"] == null) msg["itm_ids"] = new Variant();

                            msg["itm_ids"].pushBack(id);
                        }
                    }
                }
            }
            return strBuilder;
        }
       public string analysisStrName(string msg)
        {
            //  
            string strName=string.Empty;
            if (!msg.Contains("[")) return msg;
            string newStr = msg.Replace("]", "[");
            string[] strMsgArr = newStr.Split('[');//Regex.Split(newStr, @"\[.*?\]");
            for (int i = 0; i < strMsgArr.Length; i++)
            {
                if (string.IsNullOrEmpty(strMsgArr[i])) continue;
           
                string posOrItemStr = strMsgArr[i].Trim();
                Match match = Regex.Match(posOrItemStr, @"\(.*?,.*?\)");

                if (match.Success)//大体可以确认是个坐标位置
                {
                    strName+= "["+posOrItemStr+"]";
                }
                else
                {
                    if (posOrItemStr.Contains("#"))
                    {
                       
                      strName+="["+posOrItemStr.Split('#')[0]+"]";

                    }
                    else
                    {
                        strName +=posOrItemStr;
                    }
                }
            }
            return strName;
    
        }

    }
}
