using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cross;
using GameFramework;

//ReSharper disable once CheckNamespace
namespace MuGame
{
    class A3_WingProxy : BaseProxy<A3_WingProxy>
    {
        public const uint ON_LEVEL_EXP_CHANGE = 0;
        public const uint ON_STAGE_CHANGE = 1;
        public const uint ON_LEVEL_AUTO_UPGRADE = 2;
        public const uint ON_SHOW_CHANGE = 3;
        public const uint ON_STAGE_DIFT = 4;


        public A3_WingProxy()
        {
            //添加监听
            addProxyListener(PKG_NAME.S2C_WING_RES, OnWings);
        }

        #region client to server msg
        //获得翅膀
        public void GetWings()
        {
            debug.Log("send_89_op_1_");

            Variant msg = new Variant();
            msg["op"] = 1;
            sendRPC(PKG_NAME.C2S_WING, msg);
        }

        //--升级翅膀等级
        public void SendUpgradeLevel(bool use_yb = false)
        {
            debug.Log("send_89_op_2_useyb_" + use_yb);

            Variant msg = new Variant();
            msg["op"] = 2;
            msg["use_yb"] = use_yb;

            sendRPC(PKG_NAME.C2S_WING, msg);
        }

        //升级翅膀阶级
        public void SendUpgradeStage(int item_num)
        {
            debug.Log("send_89_op_3_item_num_" + item_num);

            Variant msg = new Variant();
            msg["op"] = 3;
            msg["item_num"] = item_num;

            sendRPC(PKG_NAME.C2S_WING, msg);
        }

        //自动升级翅膀等级
        public void SendAutoUpgradeLevel()
        {
            debug.Log("send_89_op_4");

            Variant msg = new Variant();
            msg["op"] = 4;

            sendRPC(PKG_NAME.C2S_WING, msg);

        }

        //--自动使用钻石
        public void SetAuotUse()
        {

        }

        //表示想要更改到哪一阶的显示效果，如果要脱下翅膀，则为0
        public void SendShowStage(int showStage)
        {
            debug.Log("send_89_op_5_showStage" + showStage);

            Variant msg = new Variant();
            msg["op"] = 5;
            msg["show_stage"] = showStage;

            sendRPC(PKG_NAME.C2S_WING, msg);
        }

        #endregion

        #region server to client

        public void OnWings(Variant data)
        {
            debug.Log("wing::" + data.dump());
            int res = data["res"];
            switch (res)
            {
                case 1://获得翅膀信息
                    A3_WingModel.getInstance().InitWingInfo(data);
                    break;
                case 2://获得升级强化结果
                    A3_WingModel.getInstance().SetLevelExp(data);
                    dispatchEvent(GameEvent.Create(ON_LEVEL_EXP_CHANGE, this, data));
                    break;
                case 3://获得进阶结果
                    A3_WingModel.getInstance().SetStageInfo(data);
                    if (A3_WingModel.getInstance().stageUp)
                    {
                        dispatchEvent(GameEvent.Create(ON_STAGE_CHANGE, this, data));
                        A3_WingModel.getInstance().SetShowStage(data);
                        dispatchEvent(GameEvent.Create(ON_SHOW_CHANGE, this, data));
                    }
                    else
                    {
                        dispatchEvent(GameEvent.Create(ON_STAGE_DIFT, this, data));
                    }
                    break;
                case 4: //获得自动升级结果
                    A3_WingModel.getInstance().SetLevelExp(data);
                    dispatchEvent(GameEvent.Create(ON_LEVEL_AUTO_UPGRADE, this, data));
                    
                    break;
                case 5:
                    A3_WingModel.getInstance().SetShowStage(data);
                    dispatchEvent(GameEvent.Create(ON_SHOW_CHANGE, this, data));
                    break;
                default:
                    Globle.err_output(res);
                    break;
            }
        }

        #endregion

    }
}
