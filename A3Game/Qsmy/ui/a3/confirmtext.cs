using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MuGame;
using Cross;

namespace MuGame
{
    class confirmtext : Window
    {
        public static Variant v;
        public static void showDeleChar(Variant charinfo)
        {
            v = charinfo;
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.CONFIRM_TEXT);
        }



        Text txtInfo1; Text txtInfo2;
        Text txtDesc;
        Text placeholder;
        InputField input;

        public override void init()
        {
            InterfaceMgr.getInstance().ui_async_open(InterfaceMgr.FLYTXT);
            InterfaceMgr.openByLua("flytxt");
            txtInfo1 = getComponentByPath<Text>("info1");
            txtInfo2 = getComponentByPath<Text>("info2");
            txtDesc = getComponentByPath<Text>("desc");
            input = getComponentByPath<InputField>("input");
            placeholder = getComponentByPath<Text>("input/Placeholder");

            getComponentByPath<Button>("y").onClick.AddListener(onYClick);
            getComponentByPath<Button>("n").onClick.AddListener(onNClick);


            getComponentByPath<Text>("desc").text = ContMgr.getCont("confirmtext_0");
            getComponentByPath<Text>("y/Text").text = ContMgr.getCont("confirmtext_1");
            getComponentByPath<Text>("n/Text").text = ContMgr.getCont("confirmtext_2");
        }

        public override void onShowed()
        {
            input.text = "";
            placeholder.text = "";

            string job = Globle.getStrJob(v["carr"]);
            string lv = v["lvl"]._int.ToString();
            string name = v["name"];
            string zhuan = v["zhua"]._int.ToString();
 //      name=     name.Replace("\0", "");

      // name = name.Replace("@", "");


       txtInfo1.text = ContMgr.getCont("comm_delechar_info1", name);
       txtInfo2.text = ContMgr.getCont("comm_delechar_info2",  job, zhuan, lv);
            txtDesc.text = ContMgr.getCont("comm_dele");
        }




        void onYClick()
        {
            if (input.text.ToLower() == "delete")
            {
                UIClient.instance.dispatchEvent(
                   GameEvent.Create(UI_EVENT.UI_ACT_DELETE_CHAR, this, GameTools.createGroup("cid", v["cid"])));

                InterfaceMgr.getInstance().close(InterfaceMgr.CONFIRM_TEXT);
            }
            else
            {
                
                flytxt.instance.fly(ContMgr.getCont("comm_inputerror"));
            }
        }

        void onNClick()
        {
            InterfaceMgr.getInstance().close(InterfaceMgr.CONFIRM_TEXT);
        }

    }
}
