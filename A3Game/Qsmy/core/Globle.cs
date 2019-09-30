using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using Cross;
public enum ENUM_QSMY_PLATFORM
{
    QSPF_None = 0, //没有平台，初始化的数据
    QSPF_Debug = -1, //内部测试，不需要经过平台登入
    QSPF_LINKSDK = 1, //对接平台的SDK
};

public enum ENUM_SDK_PLATFORM
{
    QISJ_QUICK = 1,         //SDK3月份测试
    QISJ_RYJTDREAM = 2,    //SDK俊梦
    TW_SDK = 3,            //台湾sdk
    YN_SDK = 4,            //越南sdk
    DL_SDK = 5,            //当乐sdk
}

public enum ENUM_SDK_CHILD
{
    none = 0,
    ios_cr = 1,
    ios_jx = 2,
    ios_ym = 3,
    ios_ly = 4,
    ios_dy = 5,
    ios_ymi = 6,
    ios_ml = 7,
    ios_zx  = 8,
    ios_yv=9

}

namespace MuGame
{
    public class Globle
    {
        static public bool A3_DEMO = false;
        static public bool isHardDemo = false;
        static public bool inGame = false;
        public static string Lan = "";
        static public gameST game_CrossMono;

        static public string DATA_PATH;

        static public int DebugMode = 2;//0为非debug,1为带登录的非debug，2为debug
        static public ENUM_QSMY_PLATFORM QSMY_Platform_Index = ENUM_QSMY_PLATFORM.QSPF_None;
        static public ENUM_SDK_PLATFORM QSMY_SDK_Index = ENUM_SDK_PLATFORM.QISJ_QUICK;
        static public ENUM_SDK_CHILD QSMY_SDK_CHILD = ENUM_SDK_CHILD.none;
        static public int QSMY_CLIENT_VER = 2;      //当前客户端版本号
        static public int QSMY_CLIENT_LOW_VER = 0;  //最低可登入的客户端版本号
        static public bool UNCLOSE_BEGINLOADING = true;

        static public string YR_srvlists__platform = "nil"; //platform：Kinside-SDK所定义的PID，例如360：qihooandroid
        static public string YR_srvlists__platuid = "1"; //platuid：渠道用户ID
        static public string YR_srvlists__sign = "nil"; //sign：渠道登录验证
        static public string YR_srvlists__slurl = "nil"; //slurl:获取服务器列表的url
        static public string QSMY_game_ver = "nil";
        static public string WebUrl = "http://cdn.qsmy.hulai.com/qsmy/";
        public static string VER;           //小版本号（热更）
        public static string CLIENT_VER;    //大版本号（整包更新）
        public static string CLIENT_URL;    //整包更新下载地址

        static public string YR_role_enter_time = "";

        static public int m_nTestMonsterID;

        public static List<ServerData> lServer;
        public static Dictionary<int, ServerData> dServer;
        public static ServerData curServerD;
        public static int defServerId = 0;

        static public void initServer(List<Variant> d)
        {
            lServer = new List<ServerData>();
            dServer = new Dictionary<int, ServerData>();

            foreach (Variant item in d)
            {
                ServerData sd = new ServerData();

                sd.sid = item["sid"];
                sd.sids = item["sids"];
                sd.server_name = StringUtils.unicodeToStr(item["server_name"]);
                sd.close = item["close"]._int == 1;
                sd.combine_sid = item["combine_sid"];
                sd.def = item["def"]._int == 1;
                sd.recomm = item["recomm"]._int == 1;
                sd.srvnew = item["srvnew"]._int == 1;
                
                sd.srv_status = item["srv_status"]._int;
                if(sd.srv_status == 0)
                    sd.srv_status = 1;//默认服务器流畅

                sd.login_url = item["login_url"];
                sd.do_url = item["do_url"];

                lServer.Add(sd);
                dServer[sd.sid] = sd;


                if (sd.def)
                    defServerId = sd.sid;
            }

            lServer.Sort((left, right) =>
            {
                if (left.sid > right.sid)
                    return 1;
                else if (left.sid == right.sid)
                    return 0;
                else
                    return -1;
            });

            if (defServerId == 0 && lServer.Count > 0)
                defServerId = lServer[lServer.Count - 1].sid;

        }


        public static void setDebugServerD(uint sid, string login_url)
        {
            if (Globle.DebugMode != 2)
                return;

            ServerData d = new ServerData();
            d.sid = (int)sid;
            d.login_url = login_url;
            d.do_url = login_url;
            curServerD = d;
        }


        private static int lastUnFocusTimer = 0;

        public static void OnApplicationFocus(bool isFocus)
        {

            if (!inGame)
                return;

            if (DebugMode == 2)
                return;

            debug.Log("连接状态：：" + NetClient.instance.isConnect);
            if (!NetClient.instance.isConnect)
            {
                InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.DISCONECT);
            }

            //GameSdkMgr.record_quit();
        }

        static public string getStrJob(int id)
        {
            return ContMgr.getCont("comm_job" + id);
        }



        static public Color COLOR_YELLOW = new Color(1.0f, 1.0f, 0.01f);
        static public Color COLOR_WHITE = new Color(1.0f, 1.0f, 1.0f);
        static public Color COLOR_GREEN = new Color(0f, 1f, 0f);
        static public Color COLOR_BLUE = new Color(0.4f, 1.0f, 1.0f);
        static public Color COLOR_PURPLE = new Color(1f, 0f, 1f);
        static public Color COLOR_GOLD = new Color(1f, 0.5f, 0.04f);
        static public Color COLOR_RED = new Color(0.97f, 0.05f, 0.05f);


        static public Color getColorByQuality(int quality)
        {
            if (quality == 1)
                return COLOR_WHITE;
            if (quality == 2)
                return COLOR_GREEN;
            if (quality == 3)
                return COLOR_BLUE;
            if (quality == 4)
                return COLOR_PURPLE;
            if (quality == 5)
                return COLOR_GOLD;
            if (quality == 6)
                return COLOR_RED;
            if (quality == 7)
                return COLOR_RED;

            return COLOR_WHITE;
        }
        static public Color getColorById(uint id)
        {
            if (id == 1)
                return COLOR_WHITE;
            if (id == 2)
                return COLOR_GREEN;
            if (id == 3)
                return COLOR_BLUE;
            if (id == 4)
                return COLOR_PURPLE;
            if (id == 5)
                return COLOR_GOLD;
            if (id == 6)
                return COLOR_RED;
            return COLOR_WHITE;
        }
        static public string getColorStrByQuality(string name, int quality)
        {
            if (quality == 1)
                return "<color=#ffffff>" + name + "</color>";
            if (quality == 2)
                return "<color=#00FF00>" + name + "</color>";
            if (quality == 3)
                return "<color=#66FFFF>" + name + "</color>";
            if (quality == 4)
                return "<color=#FF00FF>" + name + "</color>";
            if (quality == 5)
                return "<color=#f7790a>" + name + "</color>";
            if (quality == 6)
                return "<color=#f90e0e>" + name + "</color>";
            if (quality == 7)
                return "<color=#f90e0e>" + name + "</color>";

            return "<color=#ffffff>" + name + "</color>";
        }

        static public Color getColorForChatMsgByQuality(int quality)
        {
            if (quality == 1)
                return new Color(0xFF, 0xFF, 0xFF);
            if (quality == 2)
                return new Color(0x00, 0xFF, 0x00);
            if (quality == 3)
                return new Color(0x66, 0xFF, 0xFF);
            if (quality == 4)
                return new Color(0xFF, 0x00, 0xFF);
            if (quality == 5)
                return new Color(0xF7, 0x79, 0x0A);
            if (quality == 6)
                return new Color(0xF9, 0x0E, 0x0E);
            if (quality == 7)
                return new Color(0xF9, 0x0E, 0x0E);
            return new Color(0xFF, 0xFF, 0xFF);
        }
        static public string formatTime(int second, bool showhour = true)
        {
            int hour = second / 3600;
            second = second % 3600;
            int minute = second / 60;
            second = second % 60;

            string m = minute.ToString();
            string s = second.ToString();
            string h = hour.ToString();

            if (m.Length == 1)
            {
                m = "0" + m;
            }

            if (s.Length == 1)
            {
                s = "0" + s;
            }

            string hourFlag = "";
            if (hour > 0)
            {
                if (showhour && h.Length == 1)
                {
                    hourFlag = "0" + hour + ":";
                }
                else
                    hourFlag = hour + ":";
            }
            else
            {
                if (showhour)
                    hourFlag = "00:";
            }

            return hourFlag + m + ":" + s;
        }

        static public String getEquipTextByType(int type_id)
        {
            return ContMgr.getCont("globle_equip" + type_id);
        }

        static public String getValueTextById(int id)
        {
            return ContMgr.getCont("globle_value" + id);
        }

        public static Dictionary<string, int> AttNameIDDic = new Dictionary<string, int>()
        {
            {"strength", 1},
            {"agiligty", 2},
            {"constituion", 3},
            {"intelligence", 4},
            {"wisdom", 34},
            {"max_attack", 5},
            {"physics_def",6},
            {"magic_def", 7},
            {"fire_att", 8},
            {"ice_att", 9},
            {"light_att", 10},
            {"fire_def", 11},
            {"ice_def", 12},
            {"light_def", 13},
            {"max_hp", 14},
            {"max_mp", 15},
            {"crime", 16},
            {"mp_abate", 17},
            {"hp_suck", 18},
            {"physics_dmg_red", 19},
            {"magic_dm_red", 20},
            {"skill_damage", 21},
            {"fatal_att", 22},
            {"fatal_dodge", 23},
            {"max_hp_add", 24},
            {"max_mp_add", 25},
            {"hp_recovery", 26},
            {"mp_recovery", 27},
            {"mp_suck", 28},
            {"magic_shield", 29},
            {"exp_add", 30},
            {"blessing", 31},
            {"knowledge_add", 32},
            {"fatal_damage", 33},
            {"fire_def_add", 35},
            {"ice_def_add", 36},
            {"light_def_add", 37},
            {"min_attack", 38},
            {"double_damage_rate", 39},
            {"reflect_damage_rate", 40},
            {"ignore_crit_rate", 41},
            {"crit_add_hp", 42},
            {"hit", 43},
            {"dodge", 44},
        };

        static public String getAttrAddById(int id, int value, bool add = true)
        {
            string str = getAttrNameById(id);

            if (id == 17)
                add = false;


            if (id == 16)
            {
                str = str + ":" + value;
            }
            else if (id == 17 || id == 19 || id == 20 || id == 24 || id == 25 || id == 29 || id == 30 || id == 31 || id == 32
                || id == 33 || id == 35 || id == 36 || id == 37 || id == 39 || id == 40 || id == 17 || id == 41)
            {
                if (add)
                    str = str + "+" + (float)value / 10 + "%";
                else
                    str = str + "-" + (float)value / 10 + "%";
            }
            else
            {
                if (add)
                    str = str + "+" + value;
                else
                    str = str + "-" + value;
            }
            return str;
        }
        static public String getAttrAddById(int id, float value, bool add = true)
        {
            string str = getAttrNameById(id);

            if (id == 17)
                add = false;


            if (id == 16)
            {
                str = str + ":" + value;
            }
            else if (id == 17 || id == 19 || id == 20 || id == 24 || id == 25 || id == 29 || id == 30 || id == 31 || id == 32
                || id == 33 || id == 35 || id == 36 || id == 37 || id == 39 || id == 40 || id == 17 || id == 41)
            {
                if (add)
                    str = str + "+" + (float)value / 10 + "%";
                else
                    str = str + "-" + (float)value / 10 + "%";
            }
            else
            {
                if (add)
                    str = str + "+" + value;
                else
                    str = str + "-" + value;
            }
            return str;
        }


        static public String getAttrAddById_value(int id, int value, bool add = true)
        {
            string str = "";

            if (id == 19 || id == 20 || id == 17)
                add = false;


            if (id == 16)
            {
                str = str + ":" + value;
            }
            else if (id == 17 || id == 19 || id == 20 || id == 24 || id == 25 || id == 29 || id == 30 || id == 31 || id == 32
                || id == 33 || id == 35 || id == 36 || id == 37 || id == 39 || id == 40 || id == 17 || id == 41)
            {
                if (add)
                    str = str + "+" + (float)value / 10 + "%";
                else
                    str = str + "-" + (float)value / 10 + "%";
            }
            else
            {
                if (add)
                    str = str + "+" + value;
                else
                    str = str + "-" + value;
            }
            return str;

        }

        static public string getString(string str)
        {
            return ContMgr.getCont("globle_attr" + str);
        }
        static public String getAttrNameById(int id)
        {
            return ContMgr.getCont("globle_attr" + id);
        }

        static public string getRankAttrNameById(int id) 
        {
            return ContMgr.getCont("rank_attr" + id);
        }

        static public void setTimeScale(float scale)
        {
            Time.timeScale = scale;
            Time.fixedDeltaTime = scale * Time.timeScale;
        }

        static public string getBigText(uint num)
        {
            int count = (int)Math.Floor((double)num / 10000);
            string money;
            if (count > 0)
                money = count + ContMgr.getCont("globle_money");

            else
                money = num.ToString();
            return money;
        }

        public static string getStrTimeNomal(int _time = 0)
        {
            if (_time == 0)
                _time = muNetCleint.instance.CurServerTimeStamp;
            int timeStamp = _time;

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);

            return dtResult.ToString("yyyy-MM-dd hh:mm:ss");
        }

        public static string getStrTime(int _time = 0, bool showYear = false, bool showmine = true)
        {
            if (_time == 0)
                _time = muNetCleint.instance.CurServerTimeStamp;
            int timeStamp = _time;

            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            DateTime dtResult = dtStart.Add(toNow);

            string date = dtResult.ToShortDateString().ToString();
            string time = dtResult.ToLongTimeString().ToString();
            string[] date_arr = date.Split('/');
            string[] time_arr = time.Split(':');
            string[] pm_arr = time.Split(' ');

            string result = "";
            if (time_arr.Length == 3)
            {
                if (showYear)
                    result = ContMgr.getCont("comm_date1", new List<string> { date_arr[2], date_arr[0], date_arr[1], time_arr[0], time_arr[1] });    //    + "年" +  + "月" + + "日" + " " + (int.Parse(time_arr[0]) + timeadd) + "时" + time_arr[1] + "分";
                else
                    result = ContMgr.getCont("comm_date2", new List<string> { date_arr[0], date_arr[1], time_arr[0], time_arr[1] });    //    + "年" +  + "月" + + "日" + " " + (int.Parse(time_arr[0]) + timeadd) + "时" + time_arr[1] + "分";
                if (showmine == false)
                    result = ContMgr.getCont("comm_date3", new List<string> { date_arr[0], date_arr[1], time_arr[0] });
            }
            else if (time_arr.Length == 4)
            {
                int timeadd = pm_arr[1] == "PM" ? 12 : 0;
                if (showYear)
                    result = ContMgr.getCont("comm_date1", new List<string> { date_arr[2], date_arr[0], date_arr[1], (int.Parse(time_arr[0]) + timeadd).ToString(), time_arr[1] });    //    + "年" +  + "月" + + "日" + " " + (int.Parse(time_arr[0]) + timeadd) + "时" + time_arr[1] + "分";
                else
                    result = ContMgr.getCont("comm_date2", new List<string> { date_arr[0], date_arr[1], (int.Parse(time_arr[0]) + timeadd).ToString(), time_arr[1] });    //    + "年" +  + "月" + + "日" + " " + (int.Parse(time_arr[0]) + timeadd) + "时" + time_arr[1] + "分";
                if (showmine == false)
                    result = ContMgr.getCont("comm_date3", new List<string> { date_arr[0], date_arr[1], (int.Parse(time_arr[0]) + timeadd).ToString() });
            }


            return result;
        }


        static public float getParticleSystemLength(Transform transform)
        {
            ParticleSystem[] particleSystems = transform.GetComponentsInChildren<ParticleSystem>();
            float maxDuration = 0;
            foreach (ParticleSystem ps in particleSystems)
            {
                if (ps.enableEmission)
                {
                    if (ps.loop)
                    {
                        return -1f;
                    }
                    float dunration = 0f;
                    if (ps.emissionRate <= 0)
                    {
                        dunration = ps.startDelay + ps.startLifetime;
                    }
                    else
                    {
                        dunration = ps.startDelay + Mathf.Max(ps.duration, ps.startLifetime);
                    }
                    if (dunration > maxDuration)
                    {
                        maxDuration = dunration;
                    }
                }
            }

            Animator[] arrAni = transform.GetComponentsInChildren<Animator>();
            float maxDuration2 = 0;
            foreach (Animator ani in arrAni)
            {
                AnimatorStateInfo info = ani.GetCurrentAnimatorStateInfo(0);

                float dunration = info.length;
                if (dunration > maxDuration2)
                    maxDuration2 = dunration;
            }


            return maxDuration;
        }
        static public void err_output(int res)
        {
            if (res == -5012)
            {//不处理比武场切换英雄的报错
                return;
            }
            string str = ContMgr.getError(res.ToString());
            if (flytxt.instance != null)
                flytxt.instance.fly(str);
            else
                Debug.LogError(str);
            //if (res == -4000 || res == -1002)
            //{
            //    if (!PlayerModel.getInstance().isFirstRechange)
            //    {//优先弹首冲
            //        InterfaceMgr.getInstance().open(InterfaceMgr.FIRST_RECHANGE);
            //    }
            //    else
            //    {
            //        InterfaceMgr.getInstance().open(InterfaceMgr.TAELS);
            //    }
            //}
            //if (res == -4003 || res == -1001)
            //{
            //    InterfaceMgr.getInstance().open(InterfaceMgr.RECHARGE);
            //}
        }
    }

    public class ServerData
    {
        public int sid;
        public int sids;
        public string server_name = "nil";
        public bool close;//=1表示关服了，0开启
        public int combine_sid;//合服后的sid，默认为0
        public bool def;//为1表示为默认服，默认为0
        public bool recomm;//为表示为推荐服，默认为0
        public bool srvnew;//为1表示为新开服，默认为0
        public int srv_status;//为1表示为服务器的运营状态，默认为0，1流畅、2良好、3拥挤、4爆满、5维护
        public String login_url = "nil";
        public String do_url = "nil";
    }

    public class serverData
    {
        public string ip;
        public uint port;
        public uint sid;
        public string server_config_url;
        public uint clnt;
    }

}