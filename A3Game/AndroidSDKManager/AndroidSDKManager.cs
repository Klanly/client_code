using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;
using System.IO;


// 这个久的类写的太繁琐复杂了， 现在已经废弃，请使用AndroidPlotformSDK
namespace MuGame
{
    //public static class AndroidSDKManager
    //{
    //    private static GRGameSDK _sdk;
    //    public static bool actFlag = false;
    //    private static GRGameSDK SDK
    //    {
    //        get
    //        {
    //            if (_sdk == null)
    //                _sdk = new GRGameSDK();

    //            return _sdk;
    //        }
    //    }

    //    public enum SDKType
    //    {
    //        login,          //登录
    //        loginout,       //登出/切换
    //        lanPay,         //传入充值数据
    //        pay,            //充值
    //        userCenter,     //用户中心
    //        bbs,            //BBS
    //        feedback,       //feedback
    //        showbalance,    //showbalance
    //        antiAddiction,  //防沉迷查询
    //        realNameSignIn, //实名认证
    //        lanServer,      //传入服务器数据
    //        selectServer,   //上报选服
    //        lanRole,        //传入数据
    //        createRole,     //创角上报
    //        enterGame,      //进入上报
    //        roleUpgrade,    //升级上报
    //        exitPage,       //退出游戏 

    //    }
    //    /// <summary>
    //    /// 登录
    //    /// </summary>
    //    /// <returns></returns>
    //    public static void Login()
    //    {	
    //        actFlag = true;
    //        SDK.SetAndroidSDKDocking(SDKType.login.ToString());
    //    }

    //    /// <summary>
    //    /// 充值
    //    /// </summary>
    //    /// <param name="v">
    //    /// serverId         服务器ID
    //    /// serverName       服务器名称
    //    /// serverName       服务器描述
    //    /// roleId           角色ID
    //    /// roleName         角色名称
    //    /// productId        物品ID
    //    /// productName      物品名称
    //    /// productDesc      物品描述
    //    /// productPrice     物品单价
    //    /// productCount     物品数量
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool Payment(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanPay.ToString(), JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.pay.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 用户中心
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool UserCenter()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.userCenter.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 登出/切换
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool LoginOut()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.loginout.ToString());
    //        return true;
    //    }

    //    public static bool BBS()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.bbs.ToString());
    //        return true;
    //    }

    //    public static bool FeedBack()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.feedback.ToString());
    //        return true;
    //    }

    //    public static bool ShowBalance()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.showbalance.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 防沉迷查询
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool AntiAddiction()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.antiAddiction.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 实名认证
    //    /// </summary>
    //    /// <returns></returns>
    //    public static bool RealNameSignIn()
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.realNameSignIn.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 选服上报
    //    /// </summary>
    //    /// <param name="v">
    //    /// roleServerName   选择服务器名称
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool SelectServer(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanServer.ToString() , JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.selectServer.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 创角上报
    //    /// </summary>
    //    /// <param name="v">
    //    /// roleId            角色ID
    //    /// roleName          角色名称
    //    /// roleLevel         角色等级
    //    /// roleGold          角色游戏币
    //    /// roleYb            角色元宝
    //    /// roleServerId      角色所在服务器ID
    //    /// roleServerName    角色所在服务器名称
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool CreateRole(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanRole.ToString(), JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.createRole.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 进入上报
    //    /// </summary>
    //    /// <param name="v">
    //    /// roleId            角色ID
    //    /// roleName          角色名称
    //    /// roleLevel         角色等级
    //    /// roleGold          角色游戏币
    //    /// roleYb            角色元宝
    //    /// roleServerId      角色所在服务器ID
    //    /// roleServerName    角色所在服务器名称
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool EnterGame(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanRole.ToString(), JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.enterGame.ToString());
    //        return true;
    //    }
    //    /// <summary>
    //    /// 升级上报
    //    /// </summary>
    //    /// <param name="v">
    //    /// roleId            角色ID
    //    /// roleName          角色名称
    //    /// roleLevel         角色等级
    //    /// roleGold          角色游戏币
    //    /// roleYb            角色元宝
    //    /// roleServerId      角色所在服务器ID
    //    /// roleServerName    角色所在服务器名称
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool RoleUpgrade(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanRole.ToString(), JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.roleUpgrade.ToString());
    //        return true;
    //    }

    //    /// <summary>
    //    /// 退出游戏
    //    /// </summary>
    //    /// <param name="v">
    //    /// roleId            角色ID
    //    /// roleName          角色名称
    //    /// roleLevel         角色等级
    //    /// roleGold          角色游戏币
    //    /// roleYb            角色元宝
    //    /// roleServerId      角色所在服务器ID
    //    /// roleServerName    角色所在服务器名称
    //    /// </param>
    //    /// <returns></returns>
    //    public static bool ExitPage(Variant v)
    //    {
    //        if (GameConstant.PLAFORM_TYPE == GameConstant.PlatformType.NULL)
    //            return false;

    //        SDK.SetAndroidSDKDocking(SDKType.lanRole.ToString(), JsonManager.VariantToString(v));
    //        SDK.SetAndroidSDKDocking(SDKType.exitPage.ToString());
    //        return true;
    //    }

    //    static private os _gameOs;
    //    static public os gameOs
    //    {
    //        get{
    //            return _gameOs;
    //        }
    //    }
    //    public static void SetReceiveString(string jsonString, os o )
    //    {
    //        if( _gameOs == null )
    //        {
    //            _gameOs = o;
    //            //(_gameOs as osImpl).onResize( 
    //            //	GameConstant.SCREEN_DEF_WIDTH, 
    //            //	GameConstant.SCREEN_DEF_HEIGHT 
    //            //);
    //        }

    //        Variant v = JsonManager.StringToVariant(jsonString);
    //        LGPlatInfo.inst.onSdkCallBack( v );
    //        //if (v["cmd"]._str == SDKType.login.ToString())
    //        //{
    //        //	LGPlatInfo.onSelectPlatuid(v);
    //        //}
    //        //else if (v["cmd"]._str == SDKType.loginout.ToString())
    //        //{
    //        //}
    //        //else
    //        //	DebugTrace.print("Ererr: no`t find cmdType <" + v["cmd"]._str + ">");
    //    }

    //}



    public class JsonManager
    {

        static Dictionary<string, string> data_value = new Dictionary<string, string>();
        public static Variant StringToVariant(string jsonString, bool one = true)
        {
			 
            if (one)
            {
                data_value.Clear();
            }

            Variant v = new Variant();

			if(  jsonString.Length <= 0 )
			{
				DebugTrace.print( " >>>>>>>>>>>> JsonManager StringToVariant err! <<<<<<<<<<<< " );
				return v;
			}

            jsonString = jsonString.Remove(0,1);
            jsonString = jsonString.Remove(jsonString.Length - 1);

            jsonString = setMtool(jsonString, '[', ']', "data_arr");
            jsonString = setMtool(jsonString, '{', '}', "data_value");

            string[] jsons = jsonString.Split(',');
            foreach(string s in jsons)
            {
                string si = s.Replace("\"", "");
                si = si.Replace("\\", "");
                string[] vs = si.Split(':');
                if (vs.Length < 2)
                {
                    if (vs.Length > 0)
                    {
                        if (vs[0].Contains("data_value"))
                        {
                            v._arr.Add(StringToVariant(data_value[vs[0]], false));
                        }
                        else if (vs[0].Contains("data_arr"))
                        {
                            v._arr.Add(StringToVariant(data_value[vs[0]], false));

                        }
                        else
                        {
                            DebugTrace.print("Erorr: jsons Text Erorr");
                            return null;
                        }
                    }
                    else
                    {
                        DebugTrace.print("Erorr: jsons Text Erorr");
                        return null;
                    }
                }
                else 
                {
                    if (vs.Length > 2)
                    {
                        for (int i = 2; i < vs.Length; i++)
                            vs[1] += ":" + vs[i];
                    }

                    if (vs[1].Contains("data_value"))
                    {
                        v[vs[0]] = StringToVariant(data_value[vs[1]], false);
                    }
                    else if (vs[1].Contains("data_arr"))
                    {
                        v[vs[0]] = StringToVariant(data_value[vs[1]], false);
                    }
                    else
                        v[vs[0]] = vs[1];
                }
            }

            return v;
        }

        public static string VariantToString(Variant v)
        {
            string str = "{";

            foreach (var va in v.Keys)
            {
                str += "\"" + va + "\"" + ":" + "\"" + v[va]._str + "\"" + ",";
            }
            str = str.Remove(str.Length - 1);
            str += "}";

            return str;
        }


        static string setMtool(string jsonString, char m, char mg, string data)
        {
        
            if (!jsonString.Contains(m) && !jsonString.Contains(mg))
                return jsonString;

            int i1 = jsonString.IndexOf(m);

            int t1 = 0;
            int t2 = 0;
            string str = "";
            for (int i = 0; i < jsonString.Length; i++)
            {

                if (i >= i1)
                {
                    str += jsonString[i];
                    if (jsonString[i] == m)
                        t1++;
                    else if (jsonString[i] == mg)
                        t2++;

                    if (t1 == t2)
                        break;

                }
            }

            if (str == "")
                return jsonString;

            jsonString = jsonString.Replace(str, data + data_value.Count);
            data_value[data + data_value.Count] = str;

            return setMtool(jsonString, m, mg, data);
        }
    }



}
