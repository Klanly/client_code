using System;
using System.Collections.Generic;
using System.Text;
using GameFramework;
using Cross;
using System.Collections;

namespace MuGame
{
    class welfareProxy : BaseProxy<welfareProxy>
    {
        
        public static bool b_zhuan = false;
        bool b_leijizhongzhi = false;
        bool b_leijixiaofei = false;
        bool b_leijichongzhi_today= false;
        public  void showIconLight(bool b = true)
        {
            //if (b_zhuan==false && b_leijichongzhi_today==false && b_leijixiaofei==false && b_leijichongzhi_today==false)
            if(b)
            {
                debug.Log( "亮图标" );
                IconAddLightMgr.getInstance().showOrHideFire( "open_Light_awardCenter" , null );
            }              
            else
            {
               

                debug.Log( "关图标" );
                IconAddLightMgr.getInstance().showOrHideFire( "close_Light_awardCenter" , null );
            }
                
        }
        public static uint SHOWFIRSTRECHARGE = 4701;//显示首充按钮
        public static uint SHOWDAILYRECHARGE = 4702;//显示每日奖励
        public static uint UPLEVELAWARD = 4703;//等级奖励
        public static uint ACCUMULATERECHARGE = 4704;//累积充值
        public static uint ACCUMULATECONSUMPTION = 4705;//累积消费
        public static uint ACCUMULATETODAYRECHARGE = 4706;//今日累积充值
        public static uint SUCCESSGETFIRSTRECHARGEAWARD = 4707;//成功领取首充奖励
        public static uint totalRecharge;//总计充值
        public static uint totalXiaofei;//总计消费
        public static uint firstRecharge;//首充奖励
        public static uint todayTotal_recharge;//今日总计充值
        public List<Variant> dengjijiangli;//等级奖励
        public List<Variant> leijichongzhi;//累积充值
        public List<Variant> leijixiaofei;//累积消费
        public List<Variant> dailyGifts;//日充值
        public List<int> dailyGift=new List<int>();//日充值
        public bool _isShowEveryDataLogin = false;
        int  old_day=-1;
        public welfareProxy()
        {
            _isShowEveryDataLogin = false;
            addProxyListener(PKG_NAME.S2C_ACTIVE, onActive);
        }
        void onActive(Variant data){
            debug.Log("奖励信息:"+data.dump());
            int res = data["res"];
            /*背包满了，七日奖励提示不消失*/
            if (res == -1101 && a3_everydayLogin._instans && a3_expbar.instance)
            {
                a3_expbar.instance.getGameObjectByPath("operator/LightTips/everyDayLogin").SetActive(true);
            }
            if (res < 0) { Globle.err_output(res); return; }
           


            switch (res)
            {
                case 0:
                      setClose(data);
                    break;
                case 1:
                    setWelfare(data);//福利相关
                    if (data.ContainsKey ("first_recharge")) {
                        if (data["first_recharge"] == 1) {
                            a3_liteMinimap.instance.onSuccessGetFirstRechargeAward(null);

                        }
                    }
                    break;
                case 2:
                    setGetFirstRechargeAward(data);
                    break;
                case 3:
                    setOnlineTime(data);//在线时间奖励
                    break;
                case 4:
                    setTotalLoginAward(data);//累计登录奖励
                    break;
                case 5:
                    setLvlAward(data);//等级奖励
                    break;
                case 6:
                    setAccumulateRecharge(data);//累积充值
                    break;
                case 7:
                    setAccumulateConsumption(data);//累积消费
                    break;
                case 8:                  
                    setDayRechargeAward(data);//日充值奖励
                    break;
                case 9:
                    A3_AwardCenterServer.getInstance().SendMsg(A3_AwardCenterServer.SERVER_SELFDATA);
                    setTotalData(data);
                    break;
                default:
                    break;
            }
      
        }
        /////////////////////S2C////////////////////////////////////////////////
        void  setClose(Variant data)
        {
            int close = data["close"];
        }
        void setWelfare(Variant data)//福利相关
        {
  
            totalRecharge = data["total_recharge"];//总计充值
            totalXiaofei = data["total_xiaofei"];//总计消费
            firstRecharge = data["first_recharge"];//首冲

            Variant sendData = new Variant();
            sendData["show"] = firstRecharge <= 0 ? true : false;
            dispatchEvent(GameEvent.Create(SHOWFIRSTRECHARGE, this, sendData));
            //在线时间
            Variant onlineTimeData = data["zaixianshijian"];
            uint statusType = onlineTimeData["status_type"];//奖励buff类型
            uint endTm = onlineTimeData["end_tm"];//buff结束时间
            uint statusCount = onlineTimeData["status_count"];//领取在线奖励的次数
            //累计登录奖励
            Variant leijidenglu = data["leijidenglu"];
            uint last_day = leijidenglu["last_day"];//最后领取的时间
            uint day_count = leijidenglu["day_count"];//领奖的天数位置
            debug.Log(last_day.ToString()+"--->>");
            bool canGetAward = CheckTime(last_day);

            if (day_count < 7 && canGetAward && FunctionOpenMgr.instance.Check(FunctionOpenMgr.SEVEN_DAY))
            {
                //凌晨刷新
                if((old_day==DateTime.Now.Day &&_isShowEveryDataLogin)||!_isShowEveryDataLogin)
                {
                    old_day = DateTime.Now.Day;
                    ArrayList arry = new ArrayList();
                    arry.Add(last_day);       //最后领取的时间
                    arry.Add(day_count);      //领奖的天数位置
                    InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.A3_EVERYDAYLOGIN, arry);
                    _isShowEveryDataLogin = true;

                    if (a3_expbar.instance != null)
                        a3_expbar.instance.getGameObjectByPath("operator/LightTips/everyDayLogin").SetActive(true);
                }


            }
            //日充值奖励
            Variant richongjiangli=data["richongjiangli"];
            todayTotal_recharge = richongjiangli["total_recharge"];//今天充值了多少

           // dailyGifts = richongjiangli["gift"]._arr;//领取过的礼包

            foreach (int i in richongjiangli["gift"]._arr)
            {
                if (!dailyGift.Contains(i))
                    dailyGift.Add(i);
            }
            //b_leijichongzhi_today= WelfareModel.getInstance().for_jinrichongzhi(dailyGift);
            //等级礼包
            dengjijiangli = data["dengjijiangli"]._arr;
            b_zhuan=WelfareModel.getInstance().for_dengjilibao(dengjijiangli);       
            //累计充值
            leijichongzhi = data["leijichongzhi"]._arr;
            b_leijizhongzhi = WelfareModel.getInstance().for_leijichongzhi(leijichongzhi);
            //累计消费
            leijixiaofei = data["leijixiaofei"]._arr;
            b_leijixiaofei = WelfareModel.getInstance().for_leixjixiaofei(leijixiaofei);


            //showIconLight();
        }

        private bool CheckTime(uint last_day)
        {
            bool b = false;
            DateTime lastLoginTime = GetTime(last_day.ToString());
            TimeSpan d = DateTime.Now - lastLoginTime;
          if (d.TotalSeconds>0)
          {
                if (d.Days > 0) b = true;
                if (DateTime.Now.Year == lastLoginTime.Year && DateTime.Now.Month == lastLoginTime.Month && (DateTime.Now.Day - lastLoginTime.Day) > 0)
                {
                    b = true;
                }
                if (DateTime.Now.Year == lastLoginTime.Year && DateTime.Now.Month > lastLoginTime.Month)
                {
                    b = true;
                }

            }
            return b;
        }
        void setGetFirstRechargeAward(Variant data)//成功领取首充奖励
        {
            dispatchEvent(GameEvent.Create(SUCCESSGETFIRSTRECHARGEAWARD, this, data));
        }
        void setOnlineTime(Variant data)//在线时间奖励
        {
            uint status_type = data["status_type"];//奖励BUFF类型
            uint end_tm = data["end_tm"];//BUFF结束时间
            uint status_count = data["status_count"];//领取在线奖励的次数

        }
        void setTotalLoginAward(Variant data)//累积登陆奖励
        {
            uint last_day = data["last_day"];//最后领取的时间
            uint day_count = data["day_count"];//领奖的天数位置
            Variant sendData=new Variant();
            sendData["last_day"]=last_day;
            sendData["day_count"]=day_count;
            sendData["show"] = true;
            dispatchEvent(GameEvent.Create(SHOWDAILYRECHARGE, this, sendData));
        }
        void setLvlAward(Variant data)//等级奖励
        {
            uint gift_id = data["gift_id"];//礼包id

            dengjijiangli.Add(gift_id);
            b_zhuan = WelfareModel.getInstance().for_dengjilibao(dengjijiangli);
            //showIconLight();
            dispatchEvent(GameEvent.Create(UPLEVELAWARD,this,data));
        }
        void setAccumulateRecharge(Variant data)//累积充值
        {
            uint gift_id = data["gift_id"];//礼包id
            b_leijizhongzhi = WelfareModel.getInstance().for_leijichongzhi(leijichongzhi);
            //showIconLight();
            dispatchEvent(GameEvent.Create(ACCUMULATERECHARGE, this, data));
        }
        void setAccumulateConsumption(Variant data)//累积消费
        {
            uint gift_id = data["gift_id"];//礼包id
            b_leijixiaofei = WelfareModel.getInstance().for_leixjixiaofei(leijixiaofei);
           // showIconLight();
            dispatchEvent(GameEvent.Create(ACCUMULATECONSUMPTION, this, data));
        }
        void setDayRechargeAward(Variant data)//日充值奖励
        {
            
            int gift_id = data["gift_id"];//礼包id
            if(dailyGift.Contains(gift_id))
            {

            }
            else
            {
                dailyGift.Add(gift_id);
            }
            if (a3_awardCenter._instance)
                a3_awardCenter._instance.RefreshInfo();
            //b_leijichongzhi_today = WelfareModel.getInstance().for_jinrichongzhi(dailyGift);
            //showIconLight();
            //dispatchEvent(GameEvent.Create(ACCUMULATETODAYRECHARGE, this, data));
        }
        void setTotalData(Variant data)
        {
            //累积消费
            if (data.ContainsKey("total_xiaofei"))
            {
                uint total_xiaofei = data["total_xiaofei"];
                b_leijixiaofei = WelfareModel.getInstance().for_leixjixiaofei(leijixiaofei);
               // showIconLight();
            }
            //累积充值
            if (data.ContainsKey("total_recharge")) 
            {
                totalRecharge = data["total_recharge"];
                b_leijizhongzhi = WelfareModel.getInstance().for_leijichongzhi(leijichongzhi);
               // showIconLight();

            }
            //累积日充值
            if (data.ContainsKey("richong"))
            {
                uint richong = data["richong"];
                todayTotal_recharge = richong;
                if (a3_awardCenter._instance)
                    a3_awardCenter._instance.RefreshInfo();
                // b_leijichongzhi_today = WelfareModel.getInstance().for_jinrichongzhi(dailyGift);
                // showIconLight();
            }
              
        }
        private DateTime GetTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime); return dtStart.Add(toNow);
        }

        /////////////////////C2S///////////////////////////////////////
      public  void sendWelfare(ActiveType at, uint id=uint.MaxValue)
        {
            Variant msg = new Variant();
            msg["welfare_type"] = (uint)at;
            if (id != uint.MaxValue)
            {
                msg["id"] = id;
            }
            sendRPC(PKG_NAME.C2S_ACTIVE, msg);
        }
       public enum ActiveType
        {
            close = 0,//关闭状态
            selfWelfareInfo = 1,
            firstRechange = 2,
            onLineTimeAward = 3,
            accumulateLogin = 4,//累积登陆
            lvlAward = 5,//等级奖励
            accumulateRecharge = 6,//累积充值
            accumulateConsumption = 7,//累积消费
            dayRechargeAward = 8//日充值奖励

        }

    }
}
